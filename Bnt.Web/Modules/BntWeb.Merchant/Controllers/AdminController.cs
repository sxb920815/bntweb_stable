/* 
    ======================================================================== 
        File name：		AdminController
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/24 14:22:41
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using BntWeb.Core.SystemSettings.Models;
using BntWeb.Data.Services;
using BntWeb.Merchant.Models;
using BntWeb.Merchant.Services;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.Web.Extensions;

namespace BntWeb.Merchant.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICurrencyService _currencyService;
        private readonly IMerchantTypeServices _merchantTypeService;

        public AdminController(ICurrencyService currencyService, IMerchantTypeServices merchantTypeService)
        {
            _currencyService = currencyService;
            _merchantTypeService = merchantTypeService;
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewMerchantTypeKey })]
        public ActionResult LoadByParentId()
        {
            var pid = Request.Get("pid", "");
            Guid parentId;
            if (pid == "")
            {
                parentId = Guid.Empty;
            }
            else
            {
                parentId = pid.ToGuid();
            }
            var districts =
                _currencyService.GetList<MerchantType>(d => d.ParentId.Equals(parentId),new OrderModelField() {IsDesc = true, PropertyName = "Sort"});
            return Json(districts);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewMerchantTypeKey })]
        public ActionResult TypeList(){



            return View();
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewMerchantTypeKey })]
        public ActionResult LoadChilds(Guid parentId)
        {
            Argument.ThrowIfNullOrEmpty(parentId.ToString(), "父级分类Id");

            var districts =
                _currencyService.GetList<MerchantType>(d => d.ParentId.Equals(parentId),new OrderModelField{IsDesc = true,PropertyName = "Sort"});
            return Json(districts, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewMerchantTypeKey })]
        public ActionResult Delete(Guid typeId)
        {
            var result = new DataJsonResult();
            Models.MerchantType model = _currencyService.GetSingleById<Models.MerchantType>(typeId);

            if (model != null)
            {

                var childs = _currencyService.GetList<MerchantType>(me => me.ParentId == model.Id);
                if (childs != null && childs.Count > 0)
                {
                    result.Success = false;
                    result.ErrorMessage = $"[{model.TypeName}] 不是末级分类，不能直接删除!";
                }
                var goodsCount = _merchantTypeService.HasMerchantCount(typeId);
                if (goodsCount > 0)
                {
                    result.Success = false;
                    result.ErrorMessage = $"[{model.TypeName}] 分类下有商家信息，不能直接删除!";
                }
                else
                {
                    var isDelete = _merchantTypeService.DeleteType(model);

                    if (!isDelete)
                    {
                        result.Success = false;
                        result.ErrorMessage = "删除失败!";
                    }
                }
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = "类别不存在！";
            }

            return Json(result);
        }

        /// <summary>
        /// 更新类别
        /// </summary>
        /// <param name="editModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditMerchantTypeKey })]
        public ActionResult EditOnPost(MerchantType editModel)
        {
            //var parent = _currencyService.GetSingleById<MerchantType>(editModel.ParentId);

            var model = new MerchantType
            {
                Id = editModel.Id,
                ParentId = editModel.ParentId,
                IsShowInNav = editModel.IsShowInNav??false,
                TypeName= editModel.TypeName,
                Remark = editModel.Remark,
                Sort = editModel.Sort,
                MergerId=editModel.MergerId,
                MergerTypeName=editModel.MergerTypeName
            };

            var result = new DataJsonResult();
            model.Id = _merchantTypeService.SaveType(model);
            if (model.Id == Guid.Empty)
            {
                result.ErrorMessage = "输入数据错误";
                result.Success = false;
            }
            else
            {
                result.Data = model.Id;
            }

            return Json(result);
        }
    }
}