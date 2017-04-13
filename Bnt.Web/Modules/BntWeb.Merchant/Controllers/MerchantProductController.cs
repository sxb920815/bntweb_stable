using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Merchant.Services;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Utility.Extensions;
using BntWeb.Web.Extensions;
using BntWeb.Merchant.Models;
using BntWeb.Validation;
using BntWeb.Merchant.ViewModels;


namespace BntWeb.Merchant.Controllers
{
    public class MerchantProductController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IMerchantTypeServices _merchantTypeService;
        private readonly IStorageFileService _storageFileService;
        private readonly IMerchantProductServices _merchantService;
        /// <summary>
        /// 定义文件类型
        /// </summary>
        private const string MainImage = "MainImage";
        private const string ProductImages = "ProductImages";

        public MerchantProductController(ICurrencyService currencyService, IStorageFileService storageFileService, IMerchantTypeServices merchantTypeService, IMerchantProductServices merchantService)
        {
            _currencyService = currencyService;
            _merchantTypeService = merchantTypeService;
            _merchantService = merchantService;
            _storageFileService = storageFileService;
        }


        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewMerchantProductKey })]
        public ActionResult List(string merchantId)
        {
            Argument.ThrowIfNullOrEmpty(merchantId, "商家Id");
            if (merchantId.ToGuid() == Guid.Empty)
                Argument.ThrowIfNullOrEmpty(null, "商家Id");

            ViewBag.MerchantId = merchantId;
            return View();
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewMerchantProductKey })]
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
            Argument.ThrowIfNullOrEmpty(Request.Get("extra_search[MerchantId]"), "参数Id");
            var productName = Request.Get("extra_search[ProductName]") ?? "";
            var checkProductName = string.IsNullOrWhiteSpace(productName);

            Guid merchantId = Request.Get("extra_search[MerchantId]").ToGuid();

            Expression<Func<Models.MerchantProduct, bool>> expression =
                m => (m.MerchantId == merchantId) &&
                     (checkProductName || m.ProductName.Contains(productName));

            Expression<Func<Models.MerchantProduct, object>> orderByExpression;
            //设置排序
            switch (sortColumn)
            {
                case "ProductName":
                    orderByExpression = act => new { act.ProductName };
                    break;
                case "CreateTime":
                    orderByExpression = act => new { act.CreateTime };
                    break;
                case "IsShowInFront":
                    orderByExpression = act => new { act.IsShowInFront };
                    break;
                case "IsRecommend":
                    orderByExpression = act => new { act.IsRecommend };
                    break;
                default:
                    orderByExpression = act => new { act.CreateTime };
                    break;
            }

            //分页查询
            var list = _merchantService.GetListPaged(pageIndex, pageSize, expression, orderByExpression, isDesc, out totalCount);

            result.data = list;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.DeleteMerchantProductKey })]
        public ActionResult Delete(Guid id)
        {
            var result = new DataJsonResult();
            Models.MerchantProduct model = _currencyService.GetSingleById<Models.MerchantProduct>(id);

            if (model != null)
            {

                var isDelete = _merchantService.Delete(model);

                if (!isDelete)
                {
                    result.ErrorMessage = "删除失败!";
                }
            }
            else
            {
                result.ErrorMessage = "商家优惠不存在！";
            }

            return Json(result);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditMerchantProductKey })]
        public ActionResult Create(Guid merchantId)
        {
            var model = new MerchantProduct
            {
                Id = Guid.Empty,
                MerchantId = merchantId
            };
            var merchantList = _merchantService.GetMerchantSelectList();
            Argument.ThrowIfNull(merchantList, "商家不存在");
            ViewBag.MerchantList = merchantList.FindAll(me => me.MerchantId == merchantId);

            ViewBag.MerchantId = merchantId;
            ViewBag.EditMode = true;
            ViewBag.IsCreate = true;
            return View("Edit", model);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditMerchantProductKey })]
        public ActionResult Edit(Guid id, bool isEdit = true)
        {
            Argument.ThrowIfNull(id.ToString(), "商家优惠Id参数错误");
            var model = _merchantService.GetMerchantProductById(id);
            Argument.ThrowIfNull(model, "商家优惠不存在");

            var merchantList = _merchantService.GetMerchantSelectList();
            Argument.ThrowIfNull(merchantList, "商家不存在");
            ViewBag.MerchantList = merchantList.FindAll(me => me.MerchantId == model.MerchantId);

            ViewBag.MerchantId = model.MerchantId;
            ViewBag.EditMode = isEdit;
            ViewBag.IsCreate = false;

            return View("Edit", model);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditMerchantProductKey })]
        [System.Web.Mvc.ValidateInput(false)]
        public ActionResult EditOnPost(EditMerchantProductModel editModel)
        {
            var result = new DataJsonResult();

            if (editModel.MerchantId == Guid.Empty)
            {
                result.ErrorCode = "0000";
                result.ErrorMessage = "商家必填";
                return Json(result);
            }

            Models.MerchantProduct model = new Models.MerchantProduct();

            if (!string.IsNullOrWhiteSpace(editModel.Id.ToString()) && editModel.Id != Guid.Empty)
                model = _merchantService.GetMerchantProductById(editModel.Id);

            if (model == null)
                model = new Models.MerchantProduct() { Id = Guid.Empty };
            //var oldImageId = model.CoverImage;

            model.ProductName = editModel.ProductName;
            model.Intro = editModel.Intro;
            model.Detail = editModel.Detail;
            model.IsShowInFront = editModel.IsShowInFront;
            model.IsRecommend = editModel.IsRecommend;
            model.MerchantId = editModel.MerchantId;

            if (model.Id == Guid.Empty)
            {
                model.Id = _merchantService.Create(model);

            }
            else
            {
                model.Id = _merchantService.Update(model);
            }

            if (model.Id != Guid.Empty)
            {
                result.ErrorMessage = "";
                _storageFileService.ReplaceFile(model.Id, MerchantModule.Key, MerchantModule.DisplayName, editModel.MainImage.ToGuid(), MainImage);
                _storageFileService.ReplaceFile(model.Id, MerchantModule.Key, MerchantModule.DisplayName, editModel.ProductImages, ProductImages);
            }
            else
            {
                result.ErrorMessage = "保存失败,数据库执行错误";
            }

            return Json(result);
        }


        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditMerchantProductKey })]
        public ActionResult DeleteRelation(Guid imageId, Guid companyId)
        {
            var result = new DataJsonResult();
            if (!_storageFileService.DisassociateFile(companyId, MerchantModule.Key, imageId, ProductImages))
            {
                result.ErrorMessage = "图片删除失败!";
            }
            return Json(result);
        }
    }
}
