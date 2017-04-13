using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using BntWeb.Coupon.Models;
using BntWeb.Coupon.Services;
using BntWeb.Coupon.ViewModels;
using BntWeb.Data;
using BntWeb.Data.Services;

using BntWeb.FileSystems.Media;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.Web.Extensions;

namespace BntWeb.Coupon.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICurrencyService _currencyService;
        

        /// <summary>
        /// 构造
        /// </summary>
        public AdminController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [AdminAuthorize]
        public ActionResult List()
        {
            return View();
        }

        [AdminAuthorize]
        public ActionResult ListOnPage()
        {
            var result = new DataTableJsonResult();

            //取参数值
            int draw, pageIndex, pageSize, totalCount;
            string sortColumn;
            bool isDesc;
            Request.GetDatatableParameters(out draw, out pageIndex, out pageSize, out sortColumn, out isDesc);
            result.draw = draw;


            //分页查询
            var list = _currencyService.GetListPaged<Models.Coupon>(pageIndex, pageSize, out totalCount, new OrderModelField { PropertyName = sortColumn, IsDesc = isDesc });

            result.data = list;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize]
        public ActionResult Create()
        {
            Models.Coupon coupon = new Models.Coupon();
            coupon.Id = Guid.Empty;
            coupon.ExpiryType = ExpiryType.ExpiryByDay;
            coupon.CouponType = CouponType.FullCut;
            return View("Edit", coupon);
        }

        [AdminAuthorize]
        public ActionResult Edit(Guid id)
        {
            Models.Coupon coupon = null;
            if (id == Guid.Empty)
            {
                coupon = new Models.Coupon { Id = Guid.Empty };
            }
            else
            {
                coupon = _currencyService.GetSingleById<Models.Coupon>(id);
                Argument.ThrowIfNull(coupon, "优惠券信息不存在");
            }
            return View(coupon);
        }


        [AdminAuthorize]
        public ActionResult EditOnPost(ViewCoupon model)
        {
            var result = new DataJsonResult();
            try
            {
                Models.Coupon coupon = null;
                if (model.Id != Guid.Empty)
                    coupon = _currencyService.GetSingleById<Models.Coupon>(model.Id);
                bool isNew = coupon == null;
                if (isNew)
                {
                    coupon = new Models.Coupon
                    {
                        Id = KeyGenerator.GetGuidKey(),
                        CreateTime = DateTime.Now,
                        Status = Models.CouponStatus.Enable
                    };
                }
                coupon.CouponType = model.CouponType;
                if (model.CouponType == CouponType.FullCut)
                {
                    if(model.Money <=0)
                        throw new Exception("优惠金额不能小于0");
                    coupon.Minimum = model.Minimum;
                    coupon.Money = model.Money;
                }
                else
                {
                    if (model.Discount < 0.1M || model.Discount>10)
                        throw new Exception("优惠折扣必需在0.1-10之间");
                    coupon.Discount = model.Discount;
                }
                coupon.Title = model.Title;
                coupon.Quantity = model.Quantity;
                coupon.ExpiryType = model.ExpiryType;
                

                if (model.ExpiryType == ExpiryType.ExpiryByDay)
                {
                    coupon.ExpiryDay = model.ExpiryDay;
                    coupon.StartTime = null;
                    coupon.EndTime = null;
                }
                else
                {
                    coupon.ExpiryDay = 0;
                    coupon.StartTime = Convert.ToDateTime(model.StartTime).DayZero();
                    coupon.EndTime = Convert.ToDateTime(model.EndTime).DayEnd();
                    if (coupon.StartTime > coupon.EndTime)
                    {
                        throw new Exception("开始时间不能大于结束时间");
                    }
                }
                coupon.Describe = model.Describe;

                if (isNew)
                {
                    if (!_currencyService.Create(coupon))
                    {
                        throw new Exception("保存出错");
                    }
                }
                else
                {
                    if (!_currencyService.Update(coupon))
                    {
                        throw new Exception("保存出错");
                    }
                }
            }
            catch (Exception e)
            {
                result.ErrorMessage = e.Message;
            }

            return Json(result);
        }

        [AdminAuthorize]
        public ActionResult Delete(Guid id)
        {
            var result = new DataJsonResult();
            _currencyService.DeleteByConditon<Models.Coupon>(c => c.Id == id);
            return Json(result);
        }
    }
}