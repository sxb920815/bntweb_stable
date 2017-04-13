using System;
using System.Web.Mvc;
using BntWeb.Merchant.Services;
using BntWeb.Validation;

namespace BntWeb.Merchant.Controllers
{
    public class H5Controller : Controller
    {
        private readonly IMerchantServices _merchantService;
        private readonly IMerchantProductServices _merchantProductService;

        /// <summary>
        /// 构造
        /// </summary>
        public H5Controller(IMerchantServices merchantService, IMerchantProductServices merchantProductService)
        {
            _merchantService = merchantService;
            _merchantProductService = merchantProductService;
        }

        /// <summary>
        /// 商家详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult MerchantDetail(Guid id)
        {
            Argument.ThrowIfNull(id.ToString(), "Id");
            var model = _merchantService.GetMerchantById(id);
            Argument.ThrowIfNull(model, "商家信息不存在");
            
            return View(model);
        }

        /// <summary>
        /// 优惠详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ProductDetail(Guid id)
        {
            var model = _merchantProductService.GetMerchantProductById(id);
            if (model == null)
            {
                Response.Write("商家优惠信息不存在");
                Response.End();
            }

            var merchantInfo = _merchantService.GetMerchantById(model.MerchantId);
            var files = _merchantProductService.GetMerchantProductFile(id, "ProductImages");

            ViewBag.ProductImages = files;
            ViewBag.Merchant = merchantInfo;
            return View(model);
        }
    }
}