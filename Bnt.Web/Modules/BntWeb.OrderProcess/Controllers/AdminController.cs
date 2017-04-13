using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.Logging;
using BntWeb.Logistics.Models;
using BntWeb.OrderProcess.Models;
using BntWeb.Mvc;
using BntWeb.OrderProcess.Services;
using BntWeb.Security;
using BntWeb.Services;
using BntWeb.Utility.Extensions;
using BntWeb.Wallet.Services;
using BntWeb.Web.Extensions;

namespace BntWeb.OrderProcess.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IOrderService _orderService;

        public AdminController(ICurrencyService currencyService, IOrderService orderService)
        {
            _currencyService = currencyService;
            _orderService = orderService;

            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewOrderKey })]
        public ActionResult List()
        {
            return View();
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewOrderKey })]
        public ActionResult ListOnPage()
        {
            var result = new DataTableJsonResult();

            //取参数值
            int draw, pageIndex, pageSize, totalCount;
            string sortColumn;
            bool isDesc;
            Request.GetDatatableParameters(out draw, out pageIndex, out pageSize, out sortColumn, out isDesc);
            result.draw = draw;

            //取查询条件
            var orderNo = Request.Get("extra_search[OrderNo]");
            var checkOrderNo = string.IsNullOrWhiteSpace(orderNo);

            var consignee = Request.Get("extra_search[Consignee]");
            var checkConsignee = string.IsNullOrWhiteSpace(consignee);

            var orderStatus = Request.Get("extra_search[OrderStatus]");
            var checkOrderStatus = string.IsNullOrWhiteSpace(orderStatus);
            var orderStatusInt = orderStatus.To<int>();

            var refundStatus = Request.Get("extra_search[RefundStatus]");
            var checkRefundStatus = string.IsNullOrWhiteSpace(refundStatus);
            var refundStatusInt = refundStatus.To<int>();

            var payStatus = Request.Get("extra_search[PayStatus]");
            var checkPayStatus = string.IsNullOrWhiteSpace(payStatus);
            var payStatusInt = payStatus.To<int>();

            var shippingStatus = Request.Get("extra_search[ShippingStatus]");
            var checkShippingStatus = string.IsNullOrWhiteSpace(shippingStatus);
            var shippingStatusInt = shippingStatus.To<int>();

            var createTimeBegin = Request.Get("extra_search[CreateTimeBegin]");
            var checkCreateTimeBegin = string.IsNullOrWhiteSpace(createTimeBegin);
            var createTimeBeginTime = createTimeBegin.To<DateTime>();

            var createTimeEnd = Request.Get("extra_search[CreateTimeEnd]");
            var checkCreateTimeEnd = string.IsNullOrWhiteSpace(createTimeEnd);
            var createTimeEndTime = createTimeEnd.To<DateTime>();

            Expression<Func<Order, bool>> expression =
                l => (checkOrderNo || l.OrderNo.Contains(orderNo)) &&
                     (checkConsignee || l.Consignee.Contains(consignee)) &&
                     l.OrderStatus != OrderStatus.Deleted &&
                     (checkOrderStatus || (int)l.OrderStatus == orderStatusInt) &&
                     (checkRefundStatus || (int)l.RefundStatus == refundStatusInt) &&
                     (checkPayStatus || (int)l.PayStatus == payStatusInt) &&
                     (checkShippingStatus || (int)l.ShippingStatus == shippingStatusInt) &&
                     (checkCreateTimeBegin || l.CreateTime >= createTimeBeginTime) &&
                     (checkCreateTimeEnd || l.CreateTime <= createTimeEndTime);


            //分页查询
            var list = _currencyService.GetListPaged<Order>(pageIndex, pageSize, expression, out totalCount, new[] { new OrderModelField { PropertyName = sortColumn, IsDesc = isDesc } }, new[] { "OrderGoods" }).Select(o => new ViewModels.SimpleOderModel(o));

            result.data = list;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewOrderKey })]
        public ActionResult Detail(Guid orderId)
        {
            var order = _orderService.Load(orderId);

            ViewBag.Shippings = _currencyService.GetList<Shipping>(s => s.Status == Logistics.Models.ShippingStatus.Enabled).ToJson();

            return View(order);
        }

        /// <summary>
        /// 关闭订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageOrderKey })]
        public ActionResult Close(Guid orderId, string memo)
        {
            var result = new DataJsonResult();
            if (!_orderService.ChangeOrderStatus(orderId, OrderStatus.Closed, null, null, null, memo))
            {
                result.ErrorCode = "关闭订单出现异常错误";
            }
            return Json(result);
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="shippingId"></param>
        /// <param name="shippingName"></param>
        /// <param name="shippingCode"></param>
        /// <param name="shippingNo"></param>
        /// <returns></returns>
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageOrderKey })]
        public ActionResult Delivery(Guid orderId, Guid shippingId, string shippingName, string shippingCode, string shippingNo)
        {
            var result = new DataJsonResult();
            var order = _orderService.Load(orderId);
            if (order.OrderStatus != OrderStatus.WaitingForDelivery)
            {
                result.ErrorMessage = "订单状态不合法";
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    if (_orderService.SetShippingInfo(orderId, shippingId, shippingName, shippingCode, shippingNo))
                        if (_orderService.ChangeOrderStatus(orderId, OrderStatus.WaitingForReceiving, null, Models.ShippingStatus.Shipped, null, "订单发货"))
                            //提交
                            scope.Complete();
                }

                //删除订单发货提醒记录
                _currencyService.DeleteByConditon<OrderDeliveryReminder>(x => x.OrderId == orderId);
            }

            return Json(result);
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="shippingId"></param>
        /// <param name="shippingName"></param>
        /// <param name="shippingCode"></param>
        /// <param name="shippingNo"></param>
        /// <returns></returns>
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageOrderKey })]
        public ActionResult ChangeShipping(Guid orderId, Guid shippingId, string shippingName, string shippingCode, string shippingNo)
        {
            var result = new DataJsonResult();
            var order = _orderService.Load(orderId);
            if (order.OrderStatus != OrderStatus.WaitingForReceiving)
            {
                result.ErrorMessage = "订单状态不合法";
            }
            else
            {
                if (!_orderService.SetShippingInfo(orderId, shippingId, shippingName, shippingCode, shippingNo))
                {
                    result.ErrorMessage = "物流信息更新失败";
                }
            }

            return Json(result);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ManageOrderKey })]
        public ActionResult ChangePrice(Guid orderId, Guid orderGoodsId, decimal goodsPrice)
        {
            var result = new DataJsonResult();
            try
            {
                if (!_orderService.ChangePrice(orderId, orderGoodsId, goodsPrice))
                {
                    result.ErrorMessage = "异常错误，修改失败";
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }


            return Json(result);
        }
    }
}