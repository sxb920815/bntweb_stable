using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using BntWeb.HelpCenter.Models;
using BntWeb.HelpCenter.Services;
using BntWeb.HelpCenter.ViewModels;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Security.Identity;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.Web.Extensions;

namespace BntWeb.HelpCenter.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHelpCenterService _helpCenterService;
        private readonly IStorageFileService _storageFileService;
        private readonly ICurrencyService _currencyService;
        private readonly IUserContainer _userContainer;
        private const string FileType = "HelpCategoryLogo";

        public AdminController(IHelpCenterService helpCenterService, IStorageFileService storageFileService, ICurrencyService currencyService, IUserContainer userContainer)
        {
            _helpCenterService = helpCenterService;
            _storageFileService = storageFileService;
            _currencyService = currencyService;
            _userContainer = userContainer;
        }
        #region 帮助

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewHelpKey })]
        public ViewResult List()
        {
            var categories = _helpCenterService.GetCategories();
            ViewBag.Categories = categories;
            var categoryId = Request.Params["CategoryId"];
            ViewBag.CategoryId = categoryId;
            ViewBag.CategoriesJson = categories.Select(me => new { id = me.Id, pId = me.ParentId, name = me.Name }).ToList().ToJson();
            return View();
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewHelpKey })]
        public JsonResult ListOnPage()
        {
            var result = new DataTableJsonResult();

            //取参数值
            int draw, pageIndex, pageSize, totalCount;
            string sortColumn;
            bool isDesc;
            Request.GetDatatableParameters(out draw, out pageIndex, out pageSize, out sortColumn, out isDesc);
            result.draw = draw;

            //取查询条件
            var categoryId = Request.Get("extra_search[CategoryId]");
            if (string.IsNullOrWhiteSpace(categoryId))
            {
                categoryId = Request.Params["categoryId"];
            }
            var checkCategoryId = string.IsNullOrWhiteSpace(categoryId);

            var title = Request.Get("extra_search[Title]");
            var checkTitle = string.IsNullOrWhiteSpace(title);

            var createName = Request.Get("extra_search[CreateName]");
            var checkCreateName = string.IsNullOrWhiteSpace(createName);


            Expression<Func<Help, bool>> expression =
                l => (checkCategoryId || l.CategoryId.ToString().Equals(categoryId, StringComparison.OrdinalIgnoreCase)||l.HelpCategory.MergerId.Contains(categoryId)) &&
                     (checkTitle || l.Title.Contains(title)) &&
                     (checkCreateName || l.CreateName.Contains(createName)) &&
                     (l.Status > 0);

            Expression<Func<Help, object>> orderByExpression;
            //设置排序
            switch (sortColumn)
            {
                case "Title":
                    orderByExpression = act => new { act.Title };
                    break;
                case "CategoryId":
                    orderByExpression = act => new { act.CategoryId };
                    break;
                case "CreateTime":
                    orderByExpression = act => new { act.CreateTime };
                    break;
                case "CreateName":
                    orderByExpression = act => new { act.CreateName };
                    break;
                default:
                    orderByExpression = act => new { act.CreateTime };
                    break;
            }

            //分页查询
            var list = _helpCenterService.GetListPaged(pageIndex, pageSize, expression, orderByExpression,
                isDesc, out totalCount);

            result.data = list;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.DeleteHelpKey })]
        public ActionResult Delete(Guid helpId)
        {
            var result = new DataJsonResult();
            Help help = _currencyService.GetSingleById<Help>(helpId);

            if (help != null)
            {

                var isDelete = _helpCenterService.Delete(help);

                if (!isDelete)
                {
                    result.ErrorMessage = "删除失败!";
                }
            }
            else
            {
                result.ErrorMessage = "帮助不存在！";
            }

            return Json(result);
        }

        [AdminAuthorize(PermissionsArray = new[] {Permissions.EditHelpKey})]
        public ActionResult Create()
        {
            var model = new Help
            {
                Id = Guid.Empty
            };
            var parentId = Guid.Empty;
            var categories = _helpCenterService.GetCategories();
            if (categories == null || categories.Count == 0)
                throw new BntWebCoreException("帮助类别异常");
            bool isView = Convert.ToBoolean(Request.Get("isView", "false"));
            ViewBag.IsView = isView;
            ViewBag.IsCreate = true;
            ViewBag.CategoryIds = "";
            ViewBag.CategoryNames = "";
            ViewBag.CategoriesJson =
                categories.Select(me => new {id = me.Id, pId = me.ParentId, name = me.Name}).ToList().ToJson();
            return View("Edit", model);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewHelpKey })]
        public ActionResult View(Guid id)
        {
            Argument.ThrowIfNull(id.ToString(), "帮助Id参数错误");
            var model = _helpCenterService.GetHelpById(id);
            Argument.ThrowIfNull(model, "帮助不存在");

            var parentId = Guid.Empty;
            var categories = _helpCenterService.GetCategories();
            if (categories == null || categories.Count == 0)
                throw new BntWebCoreException("帮助类别异常");

            var categoryIds = model.CategoryId;
            var categoryNames = categories.Where(x => x.Id == model.CategoryId).ToList()[0].Name;

            bool isView = Convert.ToBoolean(Request.Get("isView", "false"));
            ViewBag.IsView = isView;
            ViewBag.IsCreate = false;
            ViewBag.CategoryIds = categoryIds;
            ViewBag.CategoryNames = categoryNames;
            ViewBag.CategoriesJson = categories.Select(me => new { id = me.Id, pId = me.ParentId, name = me.Name }).ToList().ToJson();

            return View("Edit", model);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditHelpKey })]
        public ViewResult Edit(Guid id)
        {
            Argument.ThrowIfNull(id.ToString(), "帮助Id参数错误");
            var model = _helpCenterService.GetHelpById(id);
            Argument.ThrowIfNull(model, "帮助不存在");

            var parentId = Guid.Empty;
            var categories = _helpCenterService.GetCategories();
            if (categories == null || categories.Count == 0)
                throw new BntWebCoreException("帮助分类异常");


            var categoryIds = model.CategoryId;
            var categoryNames = categories.Where(x => x.Id == model.CategoryId).ToList()[0].Name;

            bool isView = Convert.ToBoolean(Request.Get("isView", "false"));
            ViewBag.IsView = isView;
            ViewBag.IsCreate = false;
            ViewBag.CategoryIds = categoryIds;
            ViewBag.CategoryNames = categoryNames;
            ViewBag.CategoriesJson = categories.Select(me => new { id = me.Id, pId = me.ParentId, name = me.Name }).ToList().ToJson();

            return View("Edit", model);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditHelpKey })]
        [ValidateInput(false)]
        public JsonResult EditOnPost(EditHelpModel editModel)
        {
            Argument.ThrowIfNullOrEmpty(editModel.CategoryIds, "分类");

            var result = new DataJsonResult();
            Help model = new Help();

            if (!string.IsNullOrWhiteSpace(editModel.Id.ToString()) && editModel.Id != Guid.Empty)
                model = _helpCenterService.GetHelpById(editModel.Id);

            if (model == null)
                model = new Help();

            model.Title = editModel.Title;
            model.Content = editModel.Content;
            model.CategoryId = editModel.CategoryIds.Split(',')[editModel.CategoryIds.Split(',').Length - 1].ToGuid();

            if (string.IsNullOrWhiteSpace(editModel.Id.ToString()) || editModel.Id == Guid.Empty)
            {
                var currentUser = _userContainer.CurrentUser;
                model.CreateUserId = currentUser.Id;
                model.CreateName = currentUser.UserName;
                model.Status = (int) HelpStatus.Normal;
                _helpCenterService.CreateHelp(model);
            }
            else
            {
                _helpCenterService.UpdateHelp(model);
            }

            return Json(result);
        }

        #endregion

        #region 帮助类别

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewHelpCategoryKey })]
        public ViewResult CategoryList()
        {
            return View();
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewHelpCategoryKey })]
        public ActionResult Load()
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
            var districts =_currencyService.GetList<HelpCategory>(d => d.ParentId.Equals(parentId), new OrderModelField() { IsDesc = false, PropertyName = "Sort" });
            foreach (var item in districts)
            {
                item.Childs = LoadChilds(item.Id);
                var logos = _storageFileService.GetFiles(item.Id, HelpCenterModule.Key).ToList();
                item.HelpCategoryLogo = logos.Count > 0 ? logos[0].SmallThumbnail : "";
            }
            return Json(districts);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewHelpCategoryKey })]
        private List<HelpCategory> LoadChilds(Guid parentId)
        {
            Argument.ThrowIfNullOrEmpty(parentId.ToString(), "父级分类Id");

            var districts =
                _currencyService.GetList<HelpCategory>(d => d.ParentId.Equals(parentId));
            foreach (var item in districts)
            {
                item.Childs = LoadChilds(item.Id);
                var logos = _storageFileService.GetFiles(item.Id, HelpCenterModule.Key).ToList();
                item.HelpCategoryLogo = logos.Count > 0 ? logos[0].SmallThumbnail : "";
            }
            return districts;
        }



        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditHelpCategoryKey })]
        public ViewResult CategoryEdit(Guid id, Guid parentId)
        {
            HelpCategory model;
            bool addRoot=false;
            if (id == Guid.Empty)
            {
                if (parentId == Guid.Empty)
                {
                    model = new HelpCategory() {Id = Guid.Empty};
                    addRoot = true;
                }
                else
                {
                    model = _currencyService.GetSingleById<HelpCategory>(parentId);
                    var tempId = model.Id;
                    model.ParentId = tempId;// 添加子分类时,Id为子分类的parentId，Id赋空使图片为空
                    model.Id = Guid.Empty;
                    Argument.ThrowIfNull(model, "类别不存在");
                }
            }
            else
            {
                model = _currencyService.GetSingleById<HelpCategory>(id);
                Argument.ThrowIfNull(model, "类别不存在");
            }
            ViewBag.ParentId = parentId;
            ViewBag.AddRoot = addRoot;
            return View(model);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditHelpCategoryKey })]
        public JsonResult EditCategoryOnPost(EditHelpCategoryModel editModel)
        {
            var model = new HelpCategory
            {
                Id = editModel.Id,
                ParentId = editModel.ParentId,
                Name = editModel.CategoryName,
                Sort = editModel.Sort,
                MergerId = editModel.MergerId,
                MergerTypeName = editModel.MergerTypeName
            };
            var result = new DataJsonResult();
            _helpCenterService.SaveCategory(model);



            if (model.Id == Guid.Empty)
            {
                result.ErrorMessage = "输入数据错误";
                result.Success = false;
            }
            else
            {
                //添加图片关联关系
                _storageFileService.ReplaceFile(model.Id, HelpCenterModule.Key, HelpCenterModule.DisplayName, editModel.HelpCategoryLogo, FileType);
                result.Data = model.Id;
            }

            return Json(result);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.DeleteHelpCategoryKey })]
        public JsonResult DeleteCategory(Guid categoryId)
        {
            var result = new DataJsonResult();
            HelpCategory model = _currencyService.GetSingleById<HelpCategory>(categoryId);

            if (model != null)
            {

                var isDelete = _helpCenterService.DeleteCategory(model);

                if (!isDelete)
                {
                    result.ErrorMessage = "删除失败!";
                }
            }
            else
            {
                result.ErrorMessage = "类别不存在！";
            }

            return Json(result);
        }

        #endregion


    }
}