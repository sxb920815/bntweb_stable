using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Autofac;
using BntWeb.Environment;
using BntWeb.FileSystems.Media;
using BntWeb.LimitBuy.Models;
using BntWeb.Mall;
using BntWeb.Mall.Models;
using BntWeb.OrderProcess;
using BntWeb.OrderProcess.Models;

namespace BntWeb.LimitBuy.ApiModels
{
    public class QueuuOrderInfo
    {
        public string Id { get; set; }

        public string OrderNo { get; set; }

        public decimal PayFee { get; set; }

        public DateTime CreateTime { get; set; }
    }

    public class OrderModel
    {
        /// <summary>
        /// 秒杀活动Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public string Consignee { get; set; }

        /// <summary>
        /// 省份编号
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 城市编号
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区县编号
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 街道/乡镇编号
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 四级行政区全称的合集
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        /// <summary>
        /// 最佳送货时间
        /// </summary>
        public string BestTime { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 折抵积分数量
        /// </summary>
        public int Integral { get; set; }

        public string CouponId { get; set; }
        public bool IsBuy { get; set; }

        public bool IsTurn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GoodsId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public List<OrderGoodsModel> Goods { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorInfo { set; get; }
    }

    public class OrderGoodsModel
    {
        public Guid Id { get; set; }

        public int Quantity { get; set; }
    }

    public class SimpleOderModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 物流状态
        /// </summary>
        public ShippingStatus ShippingStatus { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public PayStatus PayStatus { get; set; }
        /// <summary>
        /// 评价状态
        /// </summary>
        public EvaluateStatus EvaluateStatus { get; set; }
        /// <summary>
        /// 退款状态
        /// </summary>
        public OrderRefundStatus RefundStatus { get; set; }

        /// <summary>
        /// 物流费用
        /// </summary>
        public decimal ShippingFee { get; set; }
        /// <summary>
        /// 商品总价
        /// </summary>
        public decimal GoodsAmount { get; set; }
        /// <summary>
        /// 积分折抵金额
        /// </summary>
        public decimal IntegralMoney { get; set; }
        /// <summary>
        /// 实付总额=订单总额-折扣-折抵
        /// </summary>
        public decimal PayFee { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string Consignee { get; set; }

        /// <summary>
        /// 行政区划名
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Tel { get; set; }

        ///// <summary>
        ///// 最佳送货时间
        ///// </summary>
        //public string BestTime { get; set; }

        /// <summary>
        /// 物流方式名称
        /// </summary>
        public string ShippingName { get; set; }

        /// <summary>
        /// 物流公司代码
        /// </summary>
        public string ShippingCode { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string ShippingNo { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        ///// <summary>
        ///// 是否已经投诉
        ///// </summary>
        //public bool HasComplaint { get; set; }

        public List<SimpleOrderGoods> OrderGoods { get; set; }

        public SimpleOderModel(Order order)
        {
            Id = order.Id;
            OrderNo = order.OrderNo;
            OrderStatus = order.OrderStatus;
            ShippingStatus = order.ShippingStatus;
            PayStatus = order.PayStatus;
            EvaluateStatus = order.EvaluateStatus;
            RefundStatus = order.RefundStatus;
            ShippingFee = order.ShippingFee;
            GoodsAmount = order.GoodsAmount;
            IntegralMoney = order.IntegralMoney;
            PayFee = order.PayFee;
            Consignee = order.Consignee;
            RegionName = order.PCDS;
            Address = order.Address;
            Tel = order.Tel;
            //BestTime = order.BestTime;
            Memo = order.Memo;
            ShippingName = order.ShippingName;
            ShippingCode = order.ShippingCode;
            ShippingNo = order.ShippingNo;
            CreateTime = order.CreateTime;
            OrderGoods = order.OrderGoods.Select(g => new SimpleOrderGoods(g, order)).ToList();

            //if (OrderStatus == OrderStatus.Completed)
            //{
            //    var feedbackService = HostConstObject.Container.Resolve<IFeedbackService>();
            //    int totalCount;
            //    HasComplaint =
            //        feedbackService.GetFeedbackListBySourceId(order.MemberId.ToGuid(), order.Id,
            //            Feedback.Models.FeedbackType.Complaint, "Order", 1, 1, out totalCount).Count > 0;
            //}
        }
    }

    public class SimpleOrderGoods
    {

        /// <summary>
        /// 商品Id
        /// </summary>
        public Guid GoodsId { get; set; }

        /// <summary>
        /// 单品Id
        /// </summary>
        public Guid SingleGoodsId { get; set; }

        /// <summary>
		/// 数量
		/// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }

        /// <summary>
        /// 属性组合值
        /// </summary>
        public string GoodsAttribute { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 最大可退款金额
        /// </summary>
        public decimal MaxRefundAmount { get; set; }

        /// <summary>
        /// 退款状态
        /// </summary>
        public OrderRefundStatus RefundStatus { get; set; }

        public SimplifiedStorageFile GoodsImage { get; set; }

        public SimpleOrderGoods(OrderGoods goods, Order order)
        {
            GoodsId = goods.GoodsId;
            SingleGoodsId = goods.SingleGoodsId;
            Quantity = goods.Quantity;
            GoodsName = goods.GoodsName;
            GoodsAttribute = goods.GoodsAttribute;
            Unit = goods.Unit;
            Price = goods.Price;
            RefundStatus = goods.RefundStatus;
            var fileService = HostConstObject.Container.Resolve<IStorageFileService>();
            var goodsImage = fileService.GetFiles(goods.Id, OrderProcessModule.Key, "GoodsImage").FirstOrDefault();
            GoodsImage = goodsImage?.Simplified();

            MaxRefundAmount = Price * Quantity;
            if (order.IntegralMoney > 0)
            {
                MaxRefundAmount += order.IntegralMoney * (Price * Quantity / order.GoodsAmount);
            }
        }
    }

    public class OrderCalculationModel
    {
        public Guid? AddressId { get; set; }

        public List<Guid> CartIds { get; set; }

        public List<SubmitOrderGoods> SingleGoods { get; set; }

        public string LimitBuyId { get; set; }
    }

    public class SubmitOrderGoods
    {
        public Guid SingleGoodsId { get; set; }

        public int Quantity { get; set; }
    }

    /// <summary>
    /// 订单确认商品信息
    /// </summary>
    public class OrderCalculationGoodsModel
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public Guid GoodsId { get; set; }

        /// <summary>
        /// 单品Id
        /// </summary>
        public Guid SingleGoodsId { get; set; }
        /// <summary>
        /// 属性组合值
        /// </summary>
        public string GoodsAttribute { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string GoodsName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 主图
        /// </summary>
        public SimplifiedStorageFile GoodsImage { set; get; }

        public OrderCalculationGoodsModel(Cart model)
        {
            GoodsId = model.GoodsId;
            SingleGoodsId = model.SingleGoodsId;
            Quantity = model.Quantity;
            GoodsName = model.GoodsName;
            GoodsAttribute = model.GoodsAttribute;
            Price = model.Price;

            var fileService = HostConstObject.Container.Resolve<IStorageFileService>();

            var goodsImage = fileService.GetFiles(SingleGoodsId, MallModule.Instance.InnerKey, GoodsId.ToString()).FirstOrDefault() ??
                             fileService.GetFiles(GoodsId, MallModule.Key, "MainImage").FirstOrDefault();
            GoodsImage = goodsImage?.Simplified();
        }

        public OrderCalculationGoodsModel(SingleGoods model, int quantity)
        {
            GoodsId = model.GoodsId;
            SingleGoodsId = model.Id;
            Quantity = quantity;
            GoodsName = model.Goods.Name;
            GoodsAttribute = string.Join(",", model.Attributes.Select(me => me.AttributeValue).ToList());
            Price = model.Price;

            var fileService = HostConstObject.Container.Resolve<IStorageFileService>();

            var goodsImage = fileService.GetFiles(SingleGoodsId, MallModule.Instance.InnerKey, GoodsId.ToString()).FirstOrDefault() ??
                             fileService.GetFiles(GoodsId, MallModule.Key, "MainImage").FirstOrDefault();
            GoodsImage = goodsImage?.Simplified();
        }

        public OrderCalculationGoodsModel(LimitSingleGoods model, int quantity)
        {
            GoodsId = model.GoodsId;
            SingleGoodsId = model.Id;
            Quantity = quantity;
            GoodsName = model.SingleGoodsName;
            GoodsAttribute = model.Specification;
            Price = model.LimitPrice;

            var fileService = HostConstObject.Container.Resolve<IStorageFileService>();

            var goodsImage = fileService.GetFiles(SingleGoodsId, MallModule.Instance.InnerKey, GoodsId.ToString()).FirstOrDefault() ??
                             fileService.GetFiles(GoodsId, MallModule.Key, "MainImage").FirstOrDefault();
            GoodsImage = goodsImage?.Simplified();
        }

    }



    public enum ReturnModel
    {
        /// <summary>
        /// 已抢购成功，不可再次抢购
        /// </summary>
        [Description("已抢购成功，不可再次抢购")]
        HasBuy = 1,
        /// <summary>
        /// 抢购未开始
        /// </summary>
        [Description("抢购未开始")]
        NotState = 2,
        /// <summary>
        /// 没有库存
        /// </summary>
        [Description("没有库存")]
        NoStock = -1,
        /// <summary>
        /// 成功加入队列
        /// </summary>
        [Description("成功加入队列")]
        PlusQueue = 0,
        /// <summary>
        /// 未轮到
        /// </summary>
        [Description("未轮到")]
        NotTurn = -2,
        /// <summary>
        /// 抢购失败
        /// </summary>
        [Description("抢购失败")]
        BuyFail = -3,
        /// <summary>
        /// 未抢过
        /// </summary>
        [Description("未抢过")]
        NotBuy = -4
    }

}