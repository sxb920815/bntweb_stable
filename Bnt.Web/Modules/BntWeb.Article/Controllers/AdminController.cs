using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using BntWeb.Article.Models;
using BntWeb.Article.Services;
using BntWeb.Article.ViewModels;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.Web.Extensions;

namespace BntWeb.Article.Controllers
{
    public class AdminController : Controller
    {
        private readonly IArticleService _articleService;
        //private readonly ICategoriesService _categoriesService;
        private readonly IStorageFileService _storageFileService;
        private readonly ICurrencyService _currencyService;
        private readonly IUserContainer _userContainer;
        private const string ArticleImages = "ArticleImages";

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="articleService"></param>
        /// <param name="storageFileService"></param>
        /// <param name="currencyService"></param>
        /// <param name="userContainer"></param>
        public AdminController(IArticleService articleService, IStorageFileService storageFileService, ICurrencyService currencyService, IUserContainer userContainer)
        {
            _articleService = articleService;
            _storageFileService = storageFileService;
            _currencyService = currencyService;
            _userContainer = userContainer;
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewArticleKey })]
        public ActionResult ListOnPage()
        {
            //return Json("", JsonRequestBehavior.AllowGet);
            var result = new DataTableJsonResult();

            //取参数值
            int draw, pageIndex, pageSize, totalCount;
            string sortColumn;
            bool isDesc;
            Request.GetDatatableParameters(out draw, out pageIndex, out pageSize, out sortColumn, out isDesc);
            result.draw = draw;

            //取查询条件
            var typeId = Request.Get("extra_search[TypeId]");
            var checkTypeId = string.IsNullOrWhiteSpace(typeId);

            var status = Request.Get("extra_search[Status]");
            var checkStatus = string.IsNullOrWhiteSpace(status);

            var title = Request.Get("extra_search[Title]");
            var checkTitle = string.IsNullOrWhiteSpace(title);

            var createName = Request.Get("extra_search[CreateName]");
            var checkCreateName = string.IsNullOrWhiteSpace(createName);

            var createTimeBegin = Request.Get("extra_search[CreateTimeBegin]");
            var checkCreateTimeBegin = string.IsNullOrWhiteSpace(createTimeBegin);
            var createTimeBeginTime = createTimeBegin.To<DateTime>();

            var createTimeEnd = Request.Get("extra_search[CreateTimeEnd]");
            var checkCreateTimeEnd = string.IsNullOrWhiteSpace(createTimeEnd);
            var createTimeEndTime = createTimeEnd.To<DateTime>();

            Expression<Func<Models.Article, bool>> expression =
                l => (checkTypeId || l.TypeId.ToString().Equals(typeId, StringComparison.OrdinalIgnoreCase)) &&
                     (checkTitle || l.Title.Contains(title)) &&
                     (checkCreateName || l.CreateName.Contains(createName)) &&
                     (checkStatus || ((int)l.Status).ToString().Equals(status)) &&
                     (checkCreateTimeBegin || l.CreateTime >= createTimeBeginTime) &&
                     (checkCreateTimeEnd || l.CreateTime <= createTimeEndTime) &&
                     (l.Status > 0);

            Expression<Func<Models.Article, object>> orderByExpression;
            //设置排序
            switch (sortColumn)
            {
                case "Title":
                    orderByExpression = act => new { act.Title };
                    break;
                case "CreateTime":
                    orderByExpression = act => new { act.CreateTime };
                    break;
                case "Status":
                    orderByExpression = act => new { act.Status };
                    break;
                default:
                    orderByExpression = act => new { act.CreateTime };
                    break;
            }

            //分页查询
            var list = _articleService.GetListPaged(pageIndex, pageSize, expression, orderByExpression,
                isDesc, out totalCount);

            result.data = list;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditArticleKey })]
        public ActionResult CreateArticle()
        {
            Models.Article article = new Models.Article();
            ViewBag.EditMode = true;
            ViewBag.IsView = false;
            var articleCategories = _articleService.GetTypes();
            ViewBag.articleCategories = articleCategories;

            article.Id = Guid.Empty;

            return View("EditArticle", article);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditArticleKey })]
        public ActionResult EditArticle(Guid id, bool isView)
        {

            Models.Article article = new Models.Article();
            bool editMode = !isView;
            ViewBag.EditMode = editMode;
            ViewBag.IsView = isView;
            var articleCategories = _articleService.GetTypes();
            ViewBag.articleCategories = articleCategories;

            article = _articleService.GetArticleById(id);
            Argument.ThrowIfNull(article, "文章信息不存在");

            return View(article);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewArticleKey })]
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
                _currencyService.GetList<ArticleCategories>(d => d.ParentId.Equals(parentId), new OrderModelField() { IsDesc = false, PropertyName = "Sort" });
            return Json(districts);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewArticleKey })]
        public ActionResult LoadChilds(Guid parentId)
        {
            Argument.ThrowIfNullOrEmpty(parentId.ToString(), "父级分类Id");

            var districts =
                _currencyService.GetList<ArticleCategories>(d => d.ParentId.Equals(parentId));
            return Json(districts, JsonRequestBehavior.AllowGet);
        }


        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewArticleKey })]
        public ActionResult List()
        {
            var types = _articleService.GetTypes();
            ViewBag.Types = types;
            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditArticleKey })]
        public ActionResult ArticleEditOnPost(EditArticleViewModel eidtArticle)
        {
            var result = new DataJsonResult();
            Models.Article model = new Models.Article();

            if (!string.IsNullOrWhiteSpace(eidtArticle.Id.ToString()) && eidtArticle.Id != Guid.Empty)
                model = _articleService.GetArticleById(eidtArticle.Id);

            if (model == null)
            {
                model = new Models.Article();
                model.CreateTime = DateTime.Now;
            }

            model.Title = eidtArticle.Title;
            model.Source = eidtArticle.Source;
            model.Content = eidtArticle.Content;
            model.LastUpdateTime = DateTime.Now;
            model.TypeId = eidtArticle.TypeId;
            model.Blurb = eidtArticle.Blurb;
            model.Author = eidtArticle.Author;


            if (string.IsNullOrWhiteSpace(eidtArticle.Id.ToString()) || eidtArticle.Id == Guid.Empty)
            {
                var currentUser = _userContainer.CurrentUser;
                model.CreateUserId = currentUser.Id;
                model.CreateName = currentUser.UserName;
                model.Id = _articleService.CreateArticle(model);

            }
            else
            {
                model.Id = _articleService.UpdateArticle(model);
            }

            if (model.Id != Guid.Empty)
            {
                //添加图片关联关系
                _storageFileService.ReplaceFile(model.Id, ArticleModule.Key, ArticleModule.DisplayName, eidtArticle.ArticleImages, ArticleImages);
            }
            else
            {
                result.ErrorMessage = "保存失败";
            }

            return Json(result);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditArticleKey })]
        public ActionResult DeleteRelation(Guid imageId, Guid merchantId)
        {
            var result = new DataJsonResult();
            if (!_storageFileService.DisassociateFile(merchantId, ArticleModule.Key, imageId, ArticleImages))
            {
                result.ErrorMessage = "图片删除失败!";
            }
            return Json(result);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.DeleteArticleKey })]
        public ActionResult ArticleDelete(Guid articleId)
        {
            var result = new DataJsonResult();
            Models.Article article = _currencyService.GetSingleById<Models.Article>(articleId);

            if (article != null)
            {

                var isDelete = _articleService.Delete(article);

                if (!isDelete)
                {
                    result.ErrorMessage = "删除失败!";
                }
            }
            else
            {
                result.ErrorMessage = "文章不存在！";
            }

            return Json(result);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewArticleKey })]
        public ActionResult CategoriesList()
        {

            return View();
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditArticleKey })]
        public ActionResult EditOnPost(EditArticleCategoriesModel editArticle)
        {
            var parent = _currencyService.GetSingleById<Models.ArticleCategories>(editArticle.ParentId);

            var categories = new Models.ArticleCategories
            {
                Id = editArticle.Id,
                ParentId = parent == null ? Guid.Empty : parent.Id,
                Name = editArticle.Name,

                Sort = editArticle.Sort,
                Level = parent == null ? 1 : parent.Level + 1,
                MergerName = (parent == null ? "全部" + "," + editArticle.MergerName : parent.MergerName + "," + editArticle.MergerName).Trim(','),
                MergerId = (parent == null ? Guid.Empty.ToString() + "," + editArticle.Id : parent.MergerId + "," + editArticle.Id).Trim(','),
                //MergerName ="",
                //MergerId = ""
            };



            return Json(_articleService.Save(categories, editArticle.EditMode == 0));
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditArticleKey })]
        public ActionResult Delete(Guid id)
        {
            var result = new DataJsonResult();
            Models.ArticleCategories model = _currencyService.GetSingleById<Models.ArticleCategories>(id);

            if (model != null)
            {

                var childs = _currencyService.GetList<ArticleCategories>(me => me.ParentId == model.Id);
                if (childs != null && childs.Count > 0)
                {
                    result.Success = false;
                    result.ErrorMessage = $"[{model.Name}] 不是末级分类，不能直接删除!";
                }
                var articleCount = _articleService.HasArticleCount(id);
                if (articleCount > 0)
                {
                    result.Success = false;
                    result.ErrorMessage = $"[{model.Name}] 分类下有文章信息，不能直接删除!";
                }
                else
                {
                    var isDelete = _articleService.Delete(model);

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
                result.ErrorMessage = "分类不存在！";
            }

            return Json(result);

            //return Json(_articleService.Delete(Id));
        }
    }
}