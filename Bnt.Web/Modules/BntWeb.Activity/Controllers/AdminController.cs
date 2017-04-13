using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using BntWeb.Activity.Models;
using BntWeb.Activity.Services;
using BntWeb.Activity.ViewModels;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Security.Identity;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.Web.Extensions;
namespace BntWeb.Activity.Controllers
{
    public class AdminController : Controller
    {
        private readonly IActivityService _activityService;
        private readonly IStorageFileService _storageFileService;
        private readonly ICurrencyService _currencyService;
        private readonly IUserContainer _userContainer;

        /// <summary>
        /// 定义文件类型
        /// </summary>
        private const string FileTypeName = "ActivityImage";

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="activityService"></param>
        /// <param name="storageFileService"></param>
        /// <param name="currencyService"></param>
        /// <param name="userContainer"></param>
        public AdminController(IActivityService activityService, IStorageFileService storageFileService, ICurrencyService currencyService, IUserContainer userContainer)
        {
            _activityService = activityService;
            _storageFileService = storageFileService;
            _currencyService = currencyService;
            _userContainer = userContainer;
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewActivityKey })]
        public ActionResult List()
        {
            var types = _activityService.GetTypes();
            ViewBag.Types = types;
            return View();
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewActivityKey })]
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

            var memberName = Request.Get("extra_search[MemberName]");
            var checkMemberName = string.IsNullOrWhiteSpace(memberName);

            var createTimeBegin = Request.Get("extra_search[CreateTimeBegin]");
            var checkCreateTimeBegin = string.IsNullOrWhiteSpace(createTimeBegin);
            var createTimeBeginTime = createTimeBegin.To<DateTime>();

            var createTimeEnd = Request.Get("extra_search[CreateTimeEnd]");
            var checkCreateTimeEnd = string.IsNullOrWhiteSpace(createTimeEnd);
            var createTimeEndTime = createTimeBegin.To<DateTime>();

            Expression<Func<Models.Activity, bool>> expression =
                l => (checkTypeId || l.TypeId.ToString().Equals(typeId, StringComparison.OrdinalIgnoreCase)) &&
                     (checkTitle || l.Title.Contains(title)) &&
                     (checkMemberName || l.MemberName.Contains(memberName)) &&
                     (checkCreateTimeBegin || l.CreateTime >= createTimeBeginTime) &&
                     (checkCreateTimeEnd || l.CreateTime <= createTimeEndTime) &&
                     (checkStatus || ((int)l.Status).ToString().Equals(status)) &&
                     (l.Status > 0);



            Expression<Func<Models.Activity, object>> orderByExpression;
            //设置排序
            switch (sortColumn)
            {
                case "Title":
                    orderByExpression = act => new { act.Title };
                    break;
                case "StartTime":
                    orderByExpression = act => new { act.StartTime };
                    break;
                case "EndTime":
                    orderByExpression = act => new { act.EndTime };
                    break;
                case "CreateTime":
                    orderByExpression = act => new { act.CreateTime };
                    break;
                case "ApplyNum":
                    orderByExpression = act => new { act.ApplyNum };
                    break;
                case "Status":
                    orderByExpression = act => new { act.Status };
                    break;
                default:
                    orderByExpression = act => new { act.CreateTime };
                    break;
            }

            //分页查询
            var list = _activityService.GetListPaged(pageIndex, pageSize, expression, orderByExpression,
                isDesc, out totalCount);

            result.data = list;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.DeleteActivityKey })]
        public ActionResult Delete(Guid activityId)
        {
            var result = new DataJsonResult();
            Models.Activity activity = _currencyService.GetSingleById<Models.Activity>(activityId);

            if (activity != null)
            {

                var isDelete = _activityService.Delete(activity);

                if (!isDelete)
                {
                    result.ErrorMessage = "删除失败!";
                }
            }
            else
            {
                result.ErrorMessage = "活动不存在！";
            }

            return Json(result);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditActivityKey })]
        public ActionResult CreateActivity()
        {
            Models.Activity activity = new Models.Activity();
            ViewBag.EditMode = true;
            ViewBag.IsView = false;
            var activityTypeList = _activityService.GetTypes();
            ViewBag.ActivityType = activityTypeList;

            activity.Id = Guid.Empty;

            return View("EditActivity", activity);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditActivityKey })]
        public ActionResult EditActivity(Guid id, bool isView)
        {

            Models.Activity activity = new Models.Activity();
            bool editMode = !isView;
            ViewBag.EditMode = editMode;
            ViewBag.IsView = isView;
            var activityTypeList = _activityService.GetTypes();
            ViewBag.ActivityType = activityTypeList;

            activity = _activityService.GetActivityById(id);
            Argument.ThrowIfNull(activity, "活动信息不存在");


            return View(activity);
        }

        /// <summary>
        /// 发布认证/编辑认证活动
        /// </summary>
        /// <param name="eidtActivity"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditActivityKey })]
        public ActionResult EditOnPost(EditActivityViewModel eidtActivity)
        {
            var result = new DataJsonResult();
            Models.Activity model = new Models.Activity();

            if (!string.IsNullOrWhiteSpace(eidtActivity.Id.ToString()) && eidtActivity.Id != Guid.Empty)
                model = _activityService.GetActivityById(eidtActivity.Id);

            if (model == null)
                model = new Models.Activity();

            model.Title = eidtActivity.Title;
            model.Description = eidtActivity.Description;
            model.StartTime = eidtActivity.StartTime;
            model.EndTime = eidtActivity.EndTime;
            model.CoverImage = eidtActivity.CoverImage ?? "";
            model.TypeId = eidtActivity.TypeId;
            model.Postion = eidtActivity.Postion;

            if (model.StartTime < System.DateTime.Now || model.StartTime >= model.EndTime)
            {
                result.ErrorMessage = "活动时间选择有问题";
            }
            else
            {

                if (string.IsNullOrWhiteSpace(eidtActivity.Id.ToString()) || eidtActivity.Id == Guid.Empty)
                {
                    var currentUser = _userContainer.CurrentUser;
                    model.MemberId = currentUser.Id;
                    model.MemberName = currentUser.UserName;
                    model.Id = _activityService.CreateActivity(model);

                }
                else
                {
                    if (!_activityService.UpdateActivity(model))
                    {
                        result.ErrorMessage = "保存失败";
                        return Json(result);
                    }
                }

                if (model.Id != Guid.Empty)
                {
                    result.ErrorMessage = "";
                    //处理图片关联
                    _storageFileService.ReplaceFile(model.Id, ActivityModule.Key, ActivityModule.DisplayName,
                        model.CoverImage.ToGuid(), FileTypeName);
                }
                else
                {
                    result.ErrorMessage = "保存失败";
                }
            }

            return Json(result);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditActivityKey })]
        public ActionResult SetHome(Guid activityId)
        {
            var result = new DataJsonResult();
            Models.Activity activity = _currencyService.GetSingleById<Models.Activity>(activityId);

            if (activity != null)
            {
                activity.IsShowInFront = !activity.IsShowInFront;
                if (!_activityService.UpdateActivity(activity))
                {
                    result.ErrorMessage = "设置失败!";
                }
            }
            else
            {
                result.ErrorMessage = "活动不存在！";
            }

            return Json(result);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditActivityKey })]
        public ActionResult SetBest(Guid activityId)
        {
            var result = new DataJsonResult();
            Models.Activity activity = _currencyService.GetSingleById<Models.Activity>(activityId);

            if (activity != null)
            {
                activity.IsBest = !activity.IsBest;
                if (!_activityService.UpdateActivity(activity))
                {
                    result.ErrorMessage = "设置失败!";
                }
            }
            else
            {
                result.ErrorMessage = "活动不存在！";
            }

            return Json(result);
        }

        #region 活动类型
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewTypeKey })]
        public ActionResult TypeList()
        {
            return View();
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewTypeKey })]
        public ActionResult GetTypes()
        {
            var result = new DataTableJsonResult();

            var list = _activityService.GetTypes();

            result.data = list;
            result.recordsTotal = list.Count;
            result.recordsFiltered = list.Count;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditTypeKey })]
        public ActionResult EditType(Guid id)
        {
            ActivityType model;
            if (id == Guid.Empty)
            {
                model = new ActivityType() { Id = Guid.Empty };
            }
            else
            {
                model = _currencyService.GetSingleById<ActivityType>(id);
                Argument.ThrowIfNull(model, "类型不存在");
            }
            return View(model);
        }


        [AdminAuthorize(PermissionsArray = new[] { Permissions.EditTypeKey })]
        public ActionResult EditTypeOnPost(EditActivityTypeModel editModel)
        {
            var result = new DataJsonResult();
            ActivityType model = new ActivityType();

            var isCreate = string.IsNullOrWhiteSpace(editModel.Id.ToString()) || editModel.Id == Guid.Empty;

            if (!isCreate)
            {
                model = _currencyService.GetSingleById<ActivityType>(editModel.Id);
                Argument.ThrowIfNull(model, "类型不存在");
            }
            else
                model.Id = Guid.Empty;

            model.TypeName = editModel.TypeName;
            model.Description = editModel.Description;

            model.Id = _activityService.SaveType(model);

            result.ErrorMessage = model.Id == Guid.Empty ? "保存失败" : "";

            return Json(result);
        }

        [HttpPost]
        [AdminAuthorize(PermissionsArray = new[] { Permissions.DeleteTypeKey })]
        public ActionResult DeleteType(Guid typeId)
        {
            var result = new DataJsonResult();
            ActivityType model = _currencyService.GetSingleById<ActivityType>(typeId);

            if (model != null)
            {

                var isDelete = _activityService.DeleteType(model);

                if (!isDelete)
                {
                    result.ErrorMessage = "删除失败!";
                }
            }
            else
            {
                result.ErrorMessage = "类型不存在！";
            }

            return Json(result);
        }
        #endregion

        #region 报名人员
        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewApplyKey })]
        public ActionResult ApplyList(Guid activityId)
        {
            var activity = _activityService.GetActivityById(activityId);

            return View(activity);
        }

        [AdminAuthorize(PermissionsArray = new[] { Permissions.ViewApplyKey })]
        public ActionResult GetApplyList(Guid activityId)
        {
            var result = new DataTableJsonResult();

            //取参数值
            int draw, pageIndex, pageSize, totalCount;
            string sortColumn;
            bool isDesc;
            Request.GetDatatableParameters(out draw, out pageIndex, out pageSize, out sortColumn, out isDesc);
            result.draw = draw;



            var list = _currencyService.GetListPaged<ActivityApply>(pageIndex, pageSize, me => me.ActivityId == activityId, out totalCount, new OrderModelField() { IsDesc = isDesc, PropertyName = "ApplyTime" });

            result.data = list;
            result.recordsTotal = totalCount;
            result.recordsFiltered = totalCount;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}