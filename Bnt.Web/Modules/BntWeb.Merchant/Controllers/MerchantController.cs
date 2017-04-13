/* 
    ======================================================================== 
        File name：		MerchantController
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/25 0:07:18
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
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
    public class MerchantController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IMerchantTypeServices _merchantTypeService;
        private readonly IStorageFileService _storageFileService;
        private readonly IMerchantServices _merchantService;
        /// <summary>
        /// 定义文件类型
        /// </summary>
        private const string MerchantLogoImage = "LogoImage";
        private const string MerchantBackgroundImage = "BackgroundImage";

        public MerchantController(ICurrencyService currencyService, IStorageFileService storageFileService, IMerchantTypeServices merchantTypeService, IMerchantServices merchantService)
        {
            _currencyService = currencyService;
            _merchantTypeService = merchantTypeService;
            _merchantService = merchantService;
            _storageFileService = storageFileService;
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewMerchantKey })]
        public ActionResult List()
        {
            var types = _merchantTypeService.GetTypes();
            ViewBag.Types = types;
            return View();
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewMerchantKey })]
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
            var merchantName = Request.Get("extra_search[MerchantName]");
            var checkMerchantName = string.IsNullOrWhiteSpace(merchantName);

            var phoneNumber = Request.Get("extra_search[PhoneNumber]");
            var checkPhoneNumber = string.IsNullOrWhiteSpace(phoneNumber);

            var status = Request.Get("extra_search[Status]");
            var checkStatus = string.IsNullOrWhiteSpace(status);


            Expression<Func<Models.Merchant, bool>> expression =
                l => (checkMerchantName || l.MerchantName.ToString().Equals(merchantName, StringComparison.OrdinalIgnoreCase)) &&
                     (checkPhoneNumber || l.PhoneNumber.Contains(phoneNumber)) &&
                     (checkStatus || ((int)l.Status).ToString().Equals(status)) && (l.Status > 0);



            Expression<Func<Models.Merchant, object>> orderByExpression;
            //设置排序
            switch (sortColumn)
            {
                case "MerchantName":
                    orderByExpression = act => new { act.MerchantName };
                    break;
                case "PhoneNumber":
                    orderByExpression = act => new { act.PhoneNumber };
                    break;
                case "CreateTime":
                    orderByExpression = act => new { act.CreateTime };
                    break;
                case "IsHowInFront":
                    orderByExpression = act => new { act.IsHowInFront };
                    break;
                case "IsRecommend":
                    orderByExpression = act => new { act.IsRecommend };
                    break;
                default:
                    orderByExpression = act => new { act.CreateTime };
                    break;
            }

            //分页查询
            var list = _merchantService.GetListPaged(pageIndex, pageSize, expression, orderByExpression,
                isDesc, out totalCount);

            result.data = list;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.DeleteMerchantKey })]
        public ActionResult Delete(Guid id)
        {
            var result = new DataJsonResult();
            Models.Merchant activity = _currencyService.GetSingleById<Models.Merchant>(id);

            if (activity != null)
            {

                var isDelete = _merchantService.Delete(activity);

                if (!isDelete)
                {
                    result.ErrorMessage = "删除失败!";
                }
            }
            else
            {
                result.ErrorMessage = "商家不存在！";
            }

            return Json(result);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditMerchantKey })]
        public ActionResult Create()
        {
            var model = new Models.Merchant
            {
                Id = Guid.Empty
            };
            var parentId = Guid.Empty;
            var types = _merchantTypeService.GetTypes();
            if (types == null || types.Count == 0)
                throw new BntWebCoreException("商家分类异常");
            ViewBag.EditMode = true;
            ViewBag.IsCreate = true;
            ViewBag.TypeIds = "";
            ViewBag.TypeNames = "";
            ViewBag.TypesJson = types.Select(me => new { id = me.Id, pId = me.ParentId, name = me.TypeName }).ToList().ToJson();
            return View("Edit", model);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewMerchantKey })]
        public ActionResult View(Guid id)
        {
            Argument.ThrowIfNull(id.ToString(), "商家Id参数错误");
            var model = _merchantService.GetMerchantById(id);
            Argument.ThrowIfNull(model, "商家不存在");

            var parentId = Guid.Empty;
            var types = _merchantTypeService.GetTypes();
            if (types == null || types.Count == 0)
                throw new BntWebCoreException("商家分类异常");

            var typeRelations = _merchantTypeService.GetTypeRelations(id);

            ViewBag.EditMode = false;
            ViewBag.IsCreate = false;
            ViewBag.TypeIds = string.Join(",", typeRelations.Select(me => me.MerchantTypeId).ToList());
            ViewBag.TypeNames = string.Join(",", typeRelations.Select(me => me.MerchantType.TypeName).ToList());
            ViewBag.TypesJson = types.Select(me => new { id = me.Id, pId = me.ParentId, name = me.TypeName }).ToList().ToJson();

            //图片
            ViewBag.LogoImage =
                _storageFileService.GetFiles(id, MerchantModule.Key, MerchantLogoImage).FirstOrDefault();
            ViewBag.BackgroundImages =
                _storageFileService.GetFiles(id, MerchantModule.Key, MerchantBackgroundImage).ToList() ?? new List<StorageFile>();

            return View("Edit", model);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditMerchantKey })]
        public ActionResult Edit(Guid id)
        {
            Argument.ThrowIfNull(id.ToString(), "商家Id参数错误");
            var model = _merchantService.GetMerchantById(id);
            Argument.ThrowIfNull(model, "商家不存在");

            var parentId = Guid.Empty;
            var types = _merchantTypeService.GetTypes();
            if (types == null || types.Count == 0)
                throw new BntWebCoreException("商家分类异常");

            var typeRelations = _merchantTypeService.GetTypeRelations(id);
            var typeIds = string.Join(",", typeRelations.Select(me => me.MerchantTypeId).ToList());
            var typeNames = string.Join(",", typeRelations.Select(me => me.MerchantType.TypeName).ToList());

            ViewBag.EditMode = true;
            ViewBag.IsCreate = false;
            ViewBag.TypeIds = typeIds;
            ViewBag.TypeNames = typeNames;
            ViewBag.TypesJson = types.Select(me => new { id = me.Id, pId = me.ParentId, name = me.TypeName }).ToList().ToJson();

            return View("Edit", model);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditMerchantKey })]
        public ActionResult DeleteRelation(Guid imageId, Guid merchantId)
        {
            var result = new DataJsonResult();
            if (!_storageFileService.DisassociateFile(merchantId, MerchantModule.Key, imageId, MerchantBackgroundImage))
            {
                result.ErrorMessage = "图片删除失败!";
            }
            return Json(result);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditMerchantKey })]
        [System.Web.Mvc.ValidateInput(false)]
        public ActionResult EditOnPost(EditMerchantModel editModel)
        {
            Argument.ThrowIfNullOrEmpty(editModel.TypeIds, "分类");

            var result = new DataJsonResult();
            Models.Merchant model = new Models.Merchant();

            if (!string.IsNullOrWhiteSpace(editModel.Id.ToString()) && editModel.Id != Guid.Empty)
                model = _merchantService.GetMerchantById(editModel.Id);

            if (model == null)
                model = new Models.Merchant() { Id = Guid.Empty };
            //var oldImageId = model.CoverImage;

            model.MerchantName = editModel.MerchantName;
            model.PhoneNumber = editModel.PhoneNumber;
            //model.Province = editModel.Merchant_Province;
            //model.City = editModel.Merchant_City;
            //model.District = editModel.Merchant_District;
            //model.Street = editModel.Merchant_Street;
            model.Address = editModel.Address;
            model.Intro = editModel.Intro;
            model.IsHowInFront = editModel.IsHowInFront;
            model.IsRecommend = editModel.IsRecommend;
            model.Detail = editModel.Detail;
            model.OpenTime = editModel.OpenTime;
            model.BranchName = editModel.BranchName;

            if (model.Id == Guid.Empty)
            {
                model.Id = _merchantService.CreateMerchant(model);

            }
            else
            {
                model.Id = _merchantService.UpdateMerchant(model);
            }

            if (model.Id != Guid.Empty)
            {
                result.ErrorMessage = "";

                //关联类型
                _merchantTypeService.DeleteRelations(model.Id);
                foreach (var typeId in editModel.TypeIds.Split(','))
                {
                    var relateModel = new MerchantTypeRalation
                    {
                        MerchantId = model.Id,
                        MerchantTypeId = typeId.ToGuid()
                    };
                    _merchantTypeService.InsetTypeRelation(relateModel);
                }

                //处理Logo图片关联
                _storageFileService.ReplaceFile(model.Id, MerchantModule.Key, MerchantModule.DisplayName, editModel.LogoImage.ToGuid(), MerchantLogoImage);

                //添加背景图片关联关系
                _storageFileService.ReplaceFile(model.Id, MerchantModule.Key, MerchantModule.DisplayName, editModel.BackgroundImages, MerchantBackgroundImage);

            }
            else
            {
                result.ErrorMessage = "保存失败,数据库执行错误";
            }

            return Json(result);
        }
    }

}