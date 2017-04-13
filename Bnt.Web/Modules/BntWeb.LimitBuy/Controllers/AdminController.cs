using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.LimitBuy.Models;
using BntWeb.LimitBuy.Services;
using BntWeb.LimitBuy.ViewModels;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Validation;
using BntWeb.Web.Extensions;

namespace BntWeb.LimitBuy.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IQueueService _queueService;
        private readonly ILimitBuyOrderService _limitBuyOrderService;
        private readonly ILimitSingleGoodsService _limitSingleGoodsService;
        public AdminController(ICurrencyService currencyService, IQueueService queueService, ILimitBuyOrderService limitBuyOrderService, ILimitSingleGoodsService limitSingleGoodsService)
        {
            _currencyService = currencyService;
            _queueService = queueService;
            _limitBuyOrderService = limitBuyOrderService;
            _limitSingleGoodsService = limitSingleGoodsService;
        }

        // GET: Admin

        [AdminAuthorize()]
        public ActionResult List()
        {
            return View();
        }

        [AdminAuthorize()]
        public ActionResult ListOnPage()
        {
            var result = new DataTableJsonResult();

            int draw, pageIndex, pageSize, totalCount;
            string sortColumn;
            bool isDesc;
            Request.GetDatatableParameters(out draw, out pageIndex, out pageSize, out sortColumn, out isDesc);
            result.draw = draw;

            //var list = _currencyService.GetListPaged<LimitSingleGoods>(pageIndex, pageSize, out totalCount,
            //    new OrderModelField {PropertyName = sortColumn, IsDesc = isDesc});
            var list = _limitSingleGoodsService.GetAll(pageIndex, pageSize, out totalCount, new OrderModelField { PropertyName = sortColumn, IsDesc = isDesc });
            result.data = list;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize()]
        public ActionResult GoodsListOnPage()
        {
            var result = new DataTableJsonResult();

            int draw, pageIndex, pageSize, totalCount;
            string sortColumn;
            bool isDesc;
            Request.GetDatatableParameters(out draw, out pageIndex, out pageSize, out sortColumn, out isDesc);
            result.draw = draw;
            //取查询条件
            var name = Request.Get("extra_search[Name]");

            var list = _limitBuyOrderService.GetSingleGoodsList(pageIndex, pageSize, name, out totalCount,
               new OrderModelField { PropertyName = sortColumn, IsDesc = isDesc });
            result.data = list;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize()]
        public ActionResult Edit(Guid? id = null)
        {
            LimitSingleGoods goods = null;
            if (id != null && id != Guid.Empty)
            {
                goods = _currencyService.GetSingleById<LimitSingleGoods>(id);
                if (goods != null)
                {

                }
            }
            if (goods == null)
            {
                goods = new LimitSingleGoods
                {
                    Id = KeyGenerator.GetGuidKey(),
                    BeginTime = DateTime.Now,
                    EndTime = DateTime.Now.AddDays(1)
                };
            }

            return View(goods);
        }

        [AdminAuthorize()]
        public ActionResult EditOnPost(LimitSingleGoods model)
        {
            var result = new DataJsonResult();
            if (model.BeginTime >= model.EndTime || model.BeginTime.AddMinutes(5) < DateTime.Now)
            {
                result.ErrorMessage = "开始时间不能大于结束时间不能小于当前时间";
                result.Success = false;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            var dt = DateTime.Now;
            var goods = _currencyService.GetSingleById<LimitSingleGoods>(model.Id);
            if (model.BeginTime > dt)
            {
                model.Status = LimitSingleGoodsStatus.NotInSale;
            }
            if (model.EndTime <= dt)
            {
                model.Status = LimitSingleGoodsStatus.End;
            }

            if ((model.EndTime > dt && model.BeginTime <= dt.AddMinutes(2)))
            {
                model.Status = LimitSingleGoodsStatus.InSale;
                try
                {
                    _queueService.CreateGoodsCache(model.Id.ToString(), model.Stock, model.BeginTime, model.EndTime);
                    _queueService.Start();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            }

            if (goods == null)
            {
                model.CreateTime = dt;
                _currencyService.Create(model);
            }
            else
            {
                _currencyService.Update(model);
                _queueService.UpdateGoodsStock(model.Id.ToString(), model.Stock);

            }
            return Json(new DataJsonResult());
        }


        [AdminAuthorize()]
        public ActionResult Delete(Guid id)
        {

            var result = new DataJsonResult();
            result.Success = _limitSingleGoodsService.Delete(id);
            return Json(result);
        }
    }
}