using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BntWeb.Caching;
using BntWeb.Config.Models;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.LimitBuy.ApiModels;
using BntWeb.LimitBuy.Models;
using BntWeb.LimitBuy.Services;
using BntWeb.Logistics.Services;
using BntWeb.Mall.Models;
using BntWeb.Mall.Services;
using BntWeb.MemberBase.Models;
using BntWeb.OrderProcess.Services;
using BntWeb.Services;
using BntWeb.Utility.Extensions;
using BntWeb.Wallet.Services;
using BntWeb.WebApi.Filters;
using BntWeb.WebApi.Models;

namespace BntWeb.LimitBuy.ApiControllers
{

    public class RushBuyController : BaseApiController
    {
        private readonly ICurrencyService _currencyService;
        private readonly IQueueService _queueService;
        private readonly ILimitSingleGoodsService _limitSingleGoodsService;
        private readonly IShippingAreaService _shippingAreaService;
        private readonly IWalletService _walletService;
        private readonly IConfigService _configService;


        public RushBuyController(IQueueService queueService, ICurrencyService currencyService,ILimitSingleGoodsService limitSingleGoodsService, IShippingAreaService shippingAreaService, IWalletService walletService, IConfigService configService)
        {
            _queueService = queueService;
            _currencyService = currencyService;
            _limitSingleGoodsService = limitSingleGoodsService;
            _shippingAreaService = shippingAreaService;
            _walletService = walletService;
            _configService = configService;
        }


        /// <summary>
        /// 如果有库存且用户没有参与过则每个请求都加入队列
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ApiResult RushBuy([FromBody]OrderModel submitOrder)
        {
            var userId = AuthorizedUser.Id;
            submitOrder.UserId = userId;
            submitOrder.UserName = AuthorizedUser.UserName;
            // 加入队列
            var hasEnqueue = _queueService.Enqueue(submitOrder);
            switch (hasEnqueue)
            {
                case (int)ReturnModel.HasBuy:
                    throw new WebApiInnerException("0002", "您已成功抢到该商品，请将机会留给他人吧");
                case (int)ReturnModel.NotState:
                    throw new WebApiInnerException("0003", "抢购尚未开始");
                case (int)ReturnModel.NoStock:
                    throw new WebApiInnerException("0004", "已经被抢完啦！");
            }
            return new ApiResult();
        }
        /// <summary>
        /// 检测抢购请求是否已处理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [BasicAuthentication]
        public ApiResult IsTurn(string id)
        {
            var result = new ApiResult();
            string errorInfo = "";
            var res = _queueService.IsTurn(AuthorizedUser.Id, id,out errorInfo);
            switch (res)
            {
                case (int)ReturnModel.NotTurn:
                    throw new WebApiInnerException("0002", "正在排队，请耐心等待");
                case (int)ReturnModel.NoStock:
                    throw new WebApiInnerException("0004", "已经抢完啦！");
                case (int)ReturnModel.BuyFail:
                    throw new WebApiInnerException("0004", errorInfo);
                case (int)ReturnModel.HasBuy:
                    //var order = _orderService.GetLimitOrder(AuthorizedUser.Id, id);
                    //if (order!=null)
                    //{
                    //    result.SetData(new QueuuOrderInfo
                    //    {
                    //        Id=order.Id.ToString(),
                    //        OrderNo=order.OrderNo,
                    //        PayFee=order.PayFee,
                    //        CreateTime=order.CreateTime
                    //    });
                    //}
                    var orderInfos = _queueService.UpdateCacheMamager(AuthorizedUser.Id, id);
                    result.SetData(orderInfos);
                    return result;
                default:
                    throw new WebApiInnerException("0003", "网络被挤爆了，请重试!");
            }

        }

        /// <summary>
        /// 获取秒杀商品列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult LimitGoodsList(int type, int pageNo = 1, int limit = 10)
        {
            int totalCount;

            var list = _limitSingleGoodsService.GetListForNotExpired(type, pageNo, limit, out totalCount);

            foreach (var item in list)
            {
                if (item.EndTime <= DateTime.Now && item.Status != LimitSingleGoodsStatus.End && item.Status != LimitSingleGoodsStatus.Delete)
                {
                    item.Status = LimitSingleGoodsStatus.End;
                    _limitSingleGoodsService.UpdateLimitSingleGoodsStatus(item);
                }
                if (item.BeginTime <= DateTime.Now && item.Status != LimitSingleGoodsStatus.InSale && item.EndTime > DateTime.Now && item.Status != LimitSingleGoodsStatus.Delete)
                {
                    item.Status = LimitSingleGoodsStatus.InSale;
                    _limitSingleGoodsService.UpdateLimitSingleGoodsStatus(item);
                }
                if (item.BeginTime >= DateTime.Now && item.Status != LimitSingleGoodsStatus.NotInSale && item.Status != LimitSingleGoodsStatus.Delete)
                {
                    item.Status = LimitSingleGoodsStatus.NotInSale;
                    _limitSingleGoodsService.UpdateLimitSingleGoodsStatus(item);
                }
            }
            var result = new ApiResult();
            var limitGoods = list.Select(x => new ListLimitGoodsModel(x, "")).OrderBy(x => x.BeginTime).ToList();

            // 查询剩余商品数量
            var data = new
            {
                TotalCount = totalCount,
                Goods = limitGoods
            };

            result.SetData(data);
            return result;
        }

        /// <summary>
        /// 秒杀商品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult LimitGoods(Guid id)
        {
            var result = new ApiResult();
            var model = _limitSingleGoodsService.GetItem(id);

            if (model.EndTime <= DateTime.Now && model.Status != LimitSingleGoodsStatus.End && model.Status != LimitSingleGoodsStatus.Delete)
            {
                model.Status = LimitSingleGoodsStatus.End;
                _limitSingleGoodsService.UpdateLimitSingleGoodsStatus(model);
                //_currencyService.Update(model);
            }
            if (model.BeginTime <= DateTime.Now && model.Status != LimitSingleGoodsStatus.InSale && model.EndTime > DateTime.Now && model.Status != LimitSingleGoodsStatus.Delete)
            {
                model.Status = LimitSingleGoodsStatus.InSale;
                _limitSingleGoodsService.UpdateLimitSingleGoodsStatus(model);
                //_currencyService.Update(model);
            }
            if (model.BeginTime >= DateTime.Now && model.Status != LimitSingleGoodsStatus.NotInSale && model.Status != LimitSingleGoodsStatus.Delete)
            {
                model.Status = LimitSingleGoodsStatus.NotInSale;
                _limitSingleGoodsService.UpdateLimitSingleGoodsStatus(model);
                //_currencyService.Update(model);
            }

            var goods = _currencyService.GetSingleById<Goods>(model.GoodsId);

            var data = new ViewLimitGoodsModel(model, goods.Description);
            result.SetData(data);
            return result;
        }

        /// <summary>
        /// 获取库存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult Stock(Guid id)
        {
            var result = new ApiResult();
            // 查询剩余商品数量
            result.SetData(_queueService.HasStock(id.ToString()));
            return result;
        }

        /// <summary>
        /// 获取购买者头像
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult GetMemberImages(Guid id, int pageNo = 1, int limit = 10)
        {
            var result = new ApiResult();
            int totalCount = 0;
            var limitBuy = _queueService.GetLimitByRanking(id, out totalCount, pageNo, limit).Select(x => new LimitByOrder(x));
            var data = new
            {
                TotalCount = totalCount,
                Ranking = limitBuy
            };

            result.SetData(data);
            return result;
        }

        /// <summary>
        /// 订单计算
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ApiResult OrderCalculation([FromBody]OrderCalculationModel calculationModel)
        {
            //计算商品总价
            var goodsAmount = 0M;
            var isFreeShipping = false;
            var goodsWeight = 0M;

            int goodsNum = 0;
            List<OrderCalculationGoodsModel> goods = new List<OrderCalculationGoodsModel>();

            if (calculationModel.SingleGoods != null)
                foreach (var item in calculationModel.SingleGoods)
                {
                    var singleGoods =_limitSingleGoodsService.LoadFullLimitSingleGoods(calculationModel.LimitBuyId.ToGuid());
                    if (singleGoods?.Goods == null)
                        throw new WebApiInnerException("0003", "存在非法或者失效的商品");
                    if (singleGoods.Stock < item.Quantity)
                        throw new WebApiInnerException("0004", $"【{singleGoods.Goods.Name}】库存不足");

                    goodsAmount += singleGoods.LimitPrice * item.Quantity;
                    goodsWeight += (singleGoods.Weight == 0 ? singleGoods.Goods.UsualWeight : singleGoods.Weight);
                    if (singleGoods.Goods.FreeShipping)
                    {
                        isFreeShipping = true;
                    }
                    goodsNum += item.Quantity;
                    goods.Add(new OrderCalculationGoodsModel(singleGoods, item.Quantity));
                }

            //获取收货地址
            MemberAddress address;
            if (calculationModel.AddressId == null)
            {
                address = _currencyService.GetList<MemberAddress>(me => me.MemberId == AuthorizedUser.Id)
                    .OrderByDescending(x => x.IsDefault)
                    .FirstOrDefault();
            }
            else
            {
                address = _currencyService.GetSingleById<MemberAddress>(calculationModel.AddressId);
            }

            //计算物流费用
            var shippingFee = address == null || isFreeShipping ? 0 : _shippingAreaService.GetAreaFreight(address.Province, address.City,goodsWeight);

            //可用积分
            var integralWallet = _walletService.GetWalletByMemberId(AuthorizedUser.Id, Wallet.Models.WalletType.Integral);

            var systemConfig = _configService.Get<SystemConfig>();
            var result = new ApiResult();
            var data = new
            {
                Goods = new
                {
                    TotalQuantity = goodsNum,
                    List = goods
                },
                Addresses = address == null ? null : new
                {
                    Id = address.Id,
                    Contacts = address.Contacts,
                    Phone = address.Phone,
                    RegionName = address.RegionName.Replace(",", ""),
                    Address = address.Address,
                    Postcode = address.Postcode,
                    Province = address.Province,
                    City = address.City,
                    District = address.District,
                    Street = address.Street,
                    IsDefault = address.IsDefault
                },
                GoodsAmount = goodsAmount,
                ShippingFee = shippingFee,
                AvailableIntegral = integralWallet?.Available ?? 0,
                IntegralDiscountRate = systemConfig.DiscountRate
            };
            result.SetData(data);
            return result;
        }

    }
}