using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Transactions;
using BntWeb.Caching;
using BntWeb.Config.Models;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.LimitBuy.ApiModels;
using BntWeb.Logging;
using BntWeb.Logistics.Services;
using BntWeb.Mall;
using BntWeb.Mall.Models;
using BntWeb.Mall.Services;
using BntWeb.OrderProcess.Models;
using BntWeb.OrderProcess.Services;
using BntWeb.Services;
using BntWeb.Wallet.Models;
using BntWeb.Wallet.Services;
using BntWeb.WebApi.Models;

namespace BntWeb.LimitBuy.Services
{
    public class QueueService : IQueueService
    {
        private readonly ICacheManager _cacheManager;
        private readonly ISignals _signals;
        private readonly Queue<OrderModel> _queue;
        readonly object _lockerQueue = new object();
        readonly object _lockerOrder = new object();
        readonly object _lockerGoods = new object();
        private readonly EventWaitHandle _wh = new AutoResetEvent(false);
        private readonly IGoodsService _goodsService;
        private readonly IStorageFileService _storageFileService;
        private readonly IShippingAreaService _shippingAreaService;
        private readonly IWalletService _walletService;
        private readonly IConfigService _configService;
        private readonly IOrderService _orderService;
        //private readonly ICouponService _couponService;
        //private readonly IVoucherService _voucherServcie;
        private readonly ICurrencyService _currencyService;

        private int _start;
        private Thread _thread;
        public ILogger Logger { get; set; }


        public QueueService(ICacheManager cacheManager, ISignals signals, IGoodsService goodsService, IShippingAreaService shippingAreaService, IStorageFileService storageFileService, IWalletService walletService, IConfigService configService, IOrderService orderService, ICurrencyService currencyService)
        {
            _cacheManager = cacheManager;
            _signals = signals;
            _goodsService = goodsService;
            _shippingAreaService = shippingAreaService;
            _storageFileService = storageFileService;
            _walletService = walletService;
            _configService = configService;
            _orderService = orderService;
            _currencyService = currencyService;
            //_voucherServcie = voucherServcie;
            //_couponService = couponService;
            _queue = new Queue<OrderModel>();
            _start = 0;

            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// 队列处理,队列为空则等待信号
        /// </summary>
        public void Start()
        {
            if (_start != 0)
            {
                _wh.Set();
                return;
            }
            _thread = new Thread(Process);
            _thread.Start();
            _start = 1;
        }

        public bool Stop()
        {
            if (_start >= 0) return false;
            _signals.Trigger("Order_Cancle");
            _signals.Trigger("OrderInfo_Cancle");
            Logger.Operation($"秒杀：删除缓存Order,OrderInfo", LimitBuyModule.Instance);

            _thread.Abort();
            _start = 0;
            return true;
        }
        public void Process()
        {
            while (true)
            {
                OrderModel info = null;
                //判断队列长度是否>0
                lock (_lockerQueue)
                {
                    if (_queue != null && _queue.Count > 0)
                    {
                        info = _queue.Dequeue();
                    }
                }
                if (info != null)
                {
                    //判断是否已到抢购时间,是否有缓存，是否已抢购成功过,判断是否已经超过抢购时间
                    var goods = _cacheManager.TryGet<string, GoodsModel>(info.Id.ToString());
                    if (HasBuy(info.UserId, info.Id.ToString()) <= 0 && HasStock(info.Id.ToString()) > 0 && DateTime.Now >= goods.BeginTime && DateTime.Now <= goods.EndTime)
                    {
                        //生成订单
                        try
                        {
                            CreateOrder(info);
                            Logger.Operation($"秒杀：{info.UserId},{info.UserName}抢购{info.Id}成功生成秒杀订单", LimitBuyModule.Instance);
                        }
                        catch (Exception e)
                        {
                            Logger.Error(e.Message);
                            //订单缓存更改为false,抢购失败
                            info.ErrorInfo = e.Message;
                            UpdateIsBuy(false, true, info);
                        }
                    }
                    else
                    {
                        //订单缓存更改为false,抢购失败
                        UpdateIsBuy(false, true, info);
                    }
                }
                else
                {
                    _start = -1;
                    _wh.WaitOne();
                }

                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// 创建商品缓存
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="stock"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        public bool CreateGoodsCache(string goodsId, int stock, DateTime beginTime, DateTime endTime)
        {
            try
            {
                //创建订单缓存
                _cacheManager.Get("Order", x =>
                {
                    x.Monitor(_signals.When("Order_Cancle"));
                    return new Dictionary<string, OrderModel>();
                });

                //创建商品缓存
                if (_cacheManager.TryGet<string, GoodsModel>(goodsId) != null)
                {
                    if (DateTime.Now > endTime)
                        _signals.Trigger(goodsId + "_Cancle");
                    return false;
                }
                var goods = new GoodsModel
                {
                    Id = goodsId,
                    Stock = stock,
                    BeginTime = beginTime,
                    EndTime = endTime
                };
                _cacheManager.Get(goodsId, x =>
                {
                    x.Monitor(_signals.When(goodsId + "_Cancle"));
                    return goods;
                });
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
                throw new Exception(e.Message);
            }

        }


        /// <summary>
        /// 加入队列，返回1表示该用户已抢购成功，不可再抢,2表示抢购未开始，-1表示没有库存，0表示成功加入队列
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Enqueue(OrderModel model)
        {
            //是否有库存
            if (HasStock(model.Id.ToString()) <= 0) return (int)ReturnModel.NoStock;
            //判断该用户是否已经抢到商品
            var result = HasBuy(model.UserId, model.Id.ToString());
            if (result > 0)
            {
                return result;
            }

            lock (_lockerQueue)
            {
                _queue.Enqueue(model);
            }
            if (result == 0)
            {
                //订单缓存是否轮到字段更改为false
                UpdateIsBuy(false, false, model);
            }
            _wh.Set();
            return (int)ReturnModel.PlusQueue;
        }

        /// <summary>
        /// 更改Order缓存
        /// </summary>
        /// <param name="isBuy"></param>
        /// <param name="isTurn"></param>
        /// <param name="info"></param>
        public void UpdateIsBuy(bool isBuy, bool isTurn, OrderModel info)
        {
            //订单缓存更改为false
            lock (_lockerOrder)
            {
                var order = _cacheManager.TryGet<string, Dictionary<string, OrderModel>>("Order");
                if (order.ContainsKey(info.UserId + "," + info.Id))
                {
                    order[info.UserId + "," + info.Id].IsTurn = isTurn;
                    order[info.UserId + "," + info.Id].IsBuy = isBuy;
                    order[info.UserId + "," + info.Id].ErrorInfo = info.ErrorInfo;
                    _cacheManager.Replace("Order", x =>
                    {
                        x.Monitor(_signals.When("Order_Cancle"));
                        return order;
                    });

                }
                else
                {
                    info.IsBuy = isBuy;
                    info.IsTurn = isTurn;
                    order.Add(info.UserId + "," + info.Id, info);
                    _cacheManager.Replace("Order", x =>
                    {
                        x.Monitor(_signals.When("Order_Cancle"));
                        return order;
                    });
                }
            }
        }

        /// <summary>
        /// 判断是否有库存
        /// </summary>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public int HasStock(string goodsId)
        {
            var goods = _cacheManager.TryGet<string, GoodsModel>(goodsId);
            return goods?.Stock ?? 0;
        }

        /// <summary>
        /// 更改抢购商品库存
        /// </summary>
        /// <param name="goodsId">商品Id</param>
        /// <param name="isPlus">是否是加</param>
        /// <param name="db"></param>
        public void UpdateGoodsStock(string goodsId, bool isPlus, LimitBuyDbContext db)
        {
            var goodsIdForGuid = Guid.Parse(goodsId);
            var dbGoods = db.LimitSingleGoods.FirstOrDefault(x => x.Id.Equals(goodsIdForGuid));
            if (dbGoods != null)
            {
                lock (_lockerGoods)
                {
                    var goods = _cacheManager.TryGet<string, GoodsModel>(goodsId);
                    if (isPlus)
                    {
                        goods.Stock++;
                        dbGoods.Stock++;
                    }
                    else
                    {
                        goods.Stock--;
                        dbGoods.Stock--;
                    }
                    _cacheManager.Replace(goodsId, x => goods);
                    db.SaveChanges();
                }

            }

        }
        /// <summary>
        /// 更改商品缓存
        /// </summary>
        /// <param name="goodsId"></param>
        /// <param name="stock"></param>
        public void UpdateGoodsStock(string goodsId, int stock)
        {
            lock (_lockerGoods)
            {
                var goods = _cacheManager.TryGet<string, GoodsModel>(goodsId);
                if (goods == null) return;
                goods.Stock = stock;
                _cacheManager.Replace(goodsId, x => goods);
            }
        }

        /// <summary>
        /// 判断是否已抢购成功，返回1表示已抢购成功，0表示抢购失败，-4表示未抢过，2表示抢购未开始
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="goodsId"></param>
        /// <returns></returns>
        public int HasBuy(string userId, string goodsId)
        {
            var order = _cacheManager.TryGet<string, Dictionary<string, OrderModel>>("Order");
            if (order == null) return (int)ReturnModel.NotState;
            if (order.ContainsKey(userId + "," + goodsId) && order[userId + "," + goodsId].IsBuy)
                return (int)ReturnModel.HasBuy;
            if (order.ContainsKey(userId + "," + goodsId) && !order[userId + "," + goodsId].IsBuy)
                return 0;
            return (int)ReturnModel.NotBuy;
        }

        /// <summary>
        /// 是否轮到,返回1表示抢购成功,0表示抢购失败,-2表示未轮到
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <param name="errorInfo"></param>
        /// <returns></returns>
        public int IsTurn(string userId, string id,out string errorInfo)
        {
            errorInfo = "";
            var order = _cacheManager.TryGet<string, Dictionary<string, OrderModel>>("Order");
            if (order != null && order.ContainsKey(userId + "," + id) && order[userId + "," + id].IsTurn)
            {
                errorInfo = order[userId + "," + id].ErrorInfo;
                if (order[userId + "," + id].IsBuy)
                    return (int) ReturnModel.HasBuy;//抢购成功，有必要时返回订单Id
                else if (!string.IsNullOrWhiteSpace(errorInfo))
                    return (int)ReturnModel.BuyFail;//抢购失败，返回错误信息
                else if(!string.IsNullOrWhiteSpace(errorInfo) && errorInfo.Contains("库存不足"))
                    return (int)ReturnModel.NoStock;//抢完了，没有库存
                else
                    return (int)ReturnModel.BuyFail;
            }
            else
            {
                return (int) ReturnModel.NotTurn;
            }
        }

        /// <summary>
        /// 生成订单
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public void CreateOrder(OrderModel info)
        {
            lock (_lockerOrder)
            {
                var notShipping = _shippingAreaService.NotShippingArea(info.City) ??
                  _shippingAreaService.NotShippingArea(info.Province);
                if (notShipping != null)
                {
                    throw new Exception("您提交的地址不在我们的配送范围内！");
                }

                var singelGoodsModel = info.Goods.FirstOrDefault();
                if (info.Goods == null || singelGoodsModel == null)
                    throw new Exception("存在非法或者失效的商品");
                var limitBuyId = info.Id;
                var goodsId = singelGoodsModel.Id;
                var order = new Order
                {
                    MemberId = info.UserId,
                    MemberName = info.UserName,
                    OrderStatus = OrderStatus.PendingPayment,
                    ShippingStatus = ShippingStatus.Unshipped,
                    PayStatus = PayStatus.Unpaid,
                    EvaluateStatus = EvaluateStatus.NotEvaluated,
                    Consignee = info.Consignee,
                    Province = info.Province,
                    City = info.City,
                    District = info.District,
                    Street = info.Street,
                    PCDS = info.RegionName,
                    Address = info.Address,
                    Tel = info.Tel,
                    BestTime = info.BestTime,
                    Memo = info.Memo,
                    Integral = info.Integral,
                    NeedShipping = true,
                    PayOnline = true,
                    CreateTime = DateTime.Now,
                    ModuleKey = LimitBuyModule.Key,
                    ModuleName = LimitBuyModule.DisplayName,
                    SourceType = "LimitBuy"
                };
                using (var db = new LimitBuyDbContext())
                {

                    //加载商品
                    var orderGoods = new List<OrderGoods>();

                    //var singleGoods = _goodsService.LoadFullSingleGoods(goodsId);

                    var singleGoods = _goodsService.LoadFullSingleGoods(goodsId);
                    if (singleGoods?.Goods == null || singleGoods.Goods.Status != GoodsStatus.InSale)
                        throw new Exception("存在非法或者失效的商品");

                    var singleGood = db.LimitSingleGoods.FirstOrDefault(x => x.Id.Equals(info.Id));
                    if (singleGood == null ||singleGood.Quantity<1)
                        throw new Exception("库存不足");

                    var goodsImage = _storageFileService.GetFiles(singleGoods.Id, MallModule.Instance.InnerKey, singleGoods.Goods.Id.ToString()).FirstOrDefault() ??
                                     _storageFileService.GetFiles(singleGoods.GoodsId, MallModule.Key, "MainImage").FirstOrDefault();
                    orderGoods.Add(new OrderGoods
                    {
                        GoodsId = singleGoods.Goods.Id,
                        GoodsNo = singleGoods.Goods.GoodsNo,
                        GoodsName = singleGoods.Goods.Name,
                        SingleGoodsId = singleGoods.Id,
                        SingleGoodsNo = singleGoods.SingleGoodsNo,
                        GoodsAttribute = string.Join(",", singleGoods.Attributes.Select(a => a.AttributeValue).ToArray()),
                        Unit = singleGoods.Unit,
                        Price = singleGood.LimitPrice,
                        Quantity = 1,
                        IsReal = true,
                        GoodsImage = goodsImage,
                        FreeShipping = singleGoods.Goods.FreeShipping,
                        Weight = singleGoods.Weight,
                        LimitGoodsId = info.Id

                    });
                    //计算商品总价
                    order.GoodsAmount = singleGood.LimitPrice;

                    //计算物流费用
                    var goodsWeight = orderGoods.Sum(g => g.Weight);
                    order.ShippingFee = orderGoods.Any(g => g.FreeShipping) ? 0 : _shippingAreaService.GetAreaFreight(order.Province, order.City, goodsWeight);
                    //计算订单总价
                    order.OrderAmount = order.GoodsAmount + order.ShippingFee;
                    //事务控制
                    using (var scope = new TransactionScope())
                    {
                        //计算积分折抵
                        if (order.Integral > 0)
                        {
                            var integralWallet = _walletService.GetWalletByMemberId(order.MemberId, WalletType.Integral);
                            if (integralWallet == null || integralWallet.Available < order.Integral)
                                throw new Exception("可用积分不足");

                            var systemConfig = _configService.Get<SystemConfig>();
                            //计算剩余积分总共可以抵扣多少钱
                            var maxDiscountMoney = integralWallet.Available / 100 * systemConfig.DiscountRate;
                            if (maxDiscountMoney >= order.GoodsAmount)
                            {
                                order.Integral = (int)(order.GoodsAmount / systemConfig.DiscountRate * 100);
                            }

                            string error;
                            _walletService.Draw(order.MemberId, WalletType.Integral, order.Integral, "抵扣订单", out error);
                            if (string.IsNullOrWhiteSpace(error))
                            {
                                order.IntegralMoney = (decimal)order.Integral / 100 * systemConfig.DiscountRate;
                            }
                        }
                        //计算优惠券折抵
                        decimal couponPrice = 0;
                        //if (!string.IsNullOrEmpty(info.CouponId))
                        //{
                        //    var voucher =
                        // _voucherServcie.GetVoucherByMemberId(info.UserId.ToGuid()).Count(x => x.Id == order.VoucherId && x.StartTime <= DateTime.Now && x.EndTime.AddDays(1) >= DateTime.Now && x.IsUser == UseStatus.InUse && x.UseStatus == 0);

                        //    if (voucher < 1)
                        //        throw new WebApiInnerException("0004", "该优惠券不可用");
                        //    if (_voucherServcie.SetVoucherRelationStatus(info.UserId.ToGuid(), order.VoucherId, 1))
                        //    {
                        //        var denomination = _voucherServcie.GetSingleById(info.CouponId.ToGuid())?.Denomination;
                        //        if (denomination != null)
                        //            couponPrice = (decimal)denomination;
                        //    }
                        //}
                        //计算需要支付的费用
                        order.PayFee = order.GoodsAmount - order.IntegralMoney + order.ShippingFee - couponPrice;
                        if (order.PayFee == 0)
                        {
                            order.OrderStatus = OrderStatus.WaitingForDelivery;
                            order.PayStatus = PayStatus.Paid;
                        }
                        else
                        {
                            order.UnpayFee = order.PayFee;
                        }
                        _orderService.SubmitOrder(order, orderGoods);
                        //库存减1
                        UpdateGoodsStock(info.Id.ToString(), false, db);
                        //订单缓存更改为true,抢购成功
                        UpdateIsBuy(true, true, info);

                        //清理购物车相关数据
                        _currencyService.DeleteByConditon<Cart>(
                            c => c.MemberId.Equals(info.UserId) && c.SingleGoodsId.Equals(goodsId));
                        //提交
                        scope.Complete();
                    }

                    //todo 添加订单缓存
                    var orderInfo = _cacheManager.Get("OrderInfo", x =>
                    {
                        x.Monitor(_signals.When("OrderInfo_Cancle"));
                        return new Dictionary<string, QueuuOrderInfo>();
                    });
                    orderInfo.Add(order.MemberId + "," + limitBuyId, new QueuuOrderInfo
                    {
                        Id = order.Id.ToString(),
                        OrderNo = order.OrderNo,
                        PayFee = order.PayFee,
                        CreateTime = order.CreateTime
                    });
                    _cacheManager.Replace("OrderInfo", x =>
                    {
                        x.Monitor(_signals.When("OrderInfo_Cancle"));
                        return orderInfo;
                    });
                }
            }
        }

        public QueuuOrderInfo UpdateCacheMamager(string memberId, string goodsId)
        {
            var orderInfo = _cacheManager.TryGet<string, Dictionary<string, QueuuOrderInfo>>("OrderInfo");
            if (orderInfo != null && orderInfo.ContainsKey(memberId + "," + goodsId))
            {
                return orderInfo[memberId + "," + goodsId];
            }
            return null;
        }

        /// <summary>
        /// 抢购排行
        /// </summary>
        /// <param name="limitBuyId"></param>
        /// <param name="totalCount"></param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public List<Order> GetLimitByRanking(Guid limitBuyId, out int totalCount, int pageNo = 1, int limit = 10)
        {
            using (var dbContext = new LimitBuyDbContext())
            {
                var query = from o in dbContext.Order
                            where dbContext.OrderGoods.Any(og => og.LimitGoodsId == limitBuyId && og.OrderId == o.Id)
                            orderby o.CreateTime
                            select o;
                totalCount = query.Count();

                return query.Skip((pageNo - 1) * limit).Take(limit).ToList();
            }
        }

    }
}
