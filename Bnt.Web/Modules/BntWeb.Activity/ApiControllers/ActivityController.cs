/* 
    ======================================================================== 
        File name：		ActivityController
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/19 16:24:31
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using BntWeb.Activity.ApiModels;
using BntWeb.Activity.Models;
using BntWeb.Activity.Services;
using BntWeb.FileSystems.Media;
using BntWeb.MemberBase;
using BntWeb.MemberBase.Services;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.WebApi.Filters;
using BntWeb.WebApi.Models;

namespace BntWeb.Activity.ApiControllers
{
    public class ActivityController : BaseApiController
    {
        private readonly IActivityService _activityService;
        private readonly IMemberService _memberService;
        private readonly IFileService _fileService;
        private readonly IStorageFileService _storageFileService;
        private const string FileTypeName = "ActivityImage";

        public ActivityController(IActivityService activityService, IMemberService memberService, IStorageFileService storageFileService, IFileService fileService)
        {
            _activityService = activityService;
            _memberService = memberService;
            _fileService = fileService;
            _storageFileService = storageFileService;
        }
        /// <summary>
        /// 发布活动
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ApiResult Create([FromBody]CreateActivityModel activity)
        {
            Argument.ThrowIfNullOrEmpty(activity.Title, "标题");
            Argument.ThrowIfNullOrEmpty(activity.TypeId, "活动类型");
            Argument.ThrowIfNullOrEmpty(activity.Postion, "地点");
            Argument.ThrowIfNullOrEmpty(activity.Description, "活动内容");
            Argument.ThrowIfNullOrEmpty(activity.StartTime.ToString(CultureInfo.InvariantCulture), "开始时间");
            Argument.ThrowIfNullOrEmpty(activity.EndTime.ToString(CultureInfo.InvariantCulture), "结束时间");
            Argument.ThrowIfNullOrEmpty(activity.CoverImage.Data, "图片数据");
            Argument.ThrowIfNullOrEmpty(activity.CoverImage.FileName, "图片文件名");

            if (activity.StartTime < DateTime.Now)
                throw new WebApiInnerException("1005", "开始时间不能小于当前时间");
            if (activity.EndTime < DateTime.Now || activity.EndTime <= activity.StartTime)
                throw new WebApiInnerException("1006", "结束时间不能小于当前时间且不能小于开始时间");

            if (string.IsNullOrWhiteSpace(Path.GetExtension(activity.CoverImage.FileName)))
                throw new WebApiInnerException("1001", "文件名称没有包含扩展名");

            StorageFile storageFile;
            ApiResult result = new ApiResult();
            if (_fileService.SaveFile(activity.CoverImage.Data, activity.CoverImage.FileName, false, out storageFile, 200, 200, 100, 100, ThumbnailType.TakeCenter))
            {
                var member = _memberService.FindMemberById(AuthorizedUser.Id);
                var model = new Models.Activity()
                {
                    Title = activity.Title,
                    Description = activity.Description,
                    StartTime = activity.StartTime,
                    EndTime = activity.EndTime,
                    TypeId = activity.TypeId.ToGuid(),
                    Postion = activity.Postion,
                    MemberId = member.Id,
                    MemberName = member.NickName,
                    CoverImage = storageFile.Id.ToString(),
                    LimitNum = activity.LimitNum < 0 ? 0 : activity.LimitNum
                };

                model.Id = _activityService.CreateActivity(model);

                if (model.Id == Guid.Empty)
                    throw new WebApiInnerException("1002", "参数值有误");

                //关联图片
                if (!_storageFileService.AssociateFile(model.Id, ActivityModule.Key, ActivityModule.DisplayName, storageFile.Id, FileTypeName))
                {
                    throw new WebApiInnerException("1003", "图片关联失败");
                }

            }
            else
            {
                throw new WebApiInnerException("1004", "图片上传失败");
            }

            return result;
        }

        /// <summary>
        /// 编辑活动
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [BasicAuthentication]
        public ApiResult Update(Guid activityId, [FromBody]UpdateActivityModel activity)
        {
            if (activityId == Guid.Empty)
                throw new WebApiInnerException("1057", "Id异常");

            ApiResult result = new ApiResult();
            Models.Activity oldActivity = _activityService.GetActivityById(activityId);
            if (oldActivity == null)
                throw new WebApiInnerException("1056", "活动不存在或已删除");
            var oldImageId = oldActivity.CoverImage;
            var member = _memberService.FindMemberById(AuthorizedUser.Id);

            if (!string.IsNullOrWhiteSpace(activity.Title))
                oldActivity.Title = activity.Title;
            if (!string.IsNullOrWhiteSpace(activity.Description))
                oldActivity.Description = activity.Description;
            if (activity.StartTime.Ticks != 0)
                oldActivity.StartTime = activity.StartTime;
            if (activity.EndTime.Ticks != 0)
                oldActivity.EndTime = activity.EndTime;
            if (!string.IsNullOrWhiteSpace(activity.TypeId))
                oldActivity.TypeId = activity.TypeId.ToGuid();
            if (!string.IsNullOrWhiteSpace(activity.Postion))
                oldActivity.Postion = activity.Postion;

            //图片不为空情况判断图片数据格式
            if (activity.CoverImage != null && !string.IsNullOrWhiteSpace(activity.CoverImage.FileName) && !string.IsNullOrWhiteSpace(activity.CoverImage.Data))
            {
                Argument.ThrowIfNullOrEmpty(activity.CoverImage.Data, "图片数据");
                Argument.ThrowIfNullOrEmpty(activity.CoverImage.FileName, "图片文件名");
                if (string.IsNullOrWhiteSpace(Path.GetExtension(activity.CoverImage.FileName)))
                    throw new WebApiInnerException("1051", "文件名称没有包含扩展名");

                StorageFile storageFile;
                if (_fileService.SaveFile(activity.CoverImage.Data, activity.CoverImage.FileName, false, out storageFile,
                    200, 200, 100, 100, ThumbnailType.TakeCenter))
                {
                    oldActivity.CoverImage = storageFile.Id.ToString();

                    //关联图片
                    //移除老的关联
                    if (!_storageFileService.DisassociateFile(oldActivity.Id, ActivityModule.Key, oldImageId.ToGuid(), FileTypeName))
                        throw new WebApiInnerException("1055", "移除老的关联失败");
                    if (!_storageFileService.AssociateFile(oldActivity.Id, ActivityModule.Key, ActivityModule.DisplayName, storageFile.Id, FileTypeName))
                    {
                        throw new WebApiInnerException("1053", "图片关联失败");
                    }
                }
                else
                {
                    throw new WebApiInnerException("1054", "图片上传失败");
                }
            }

            if (!_activityService.UpdateActivity(oldActivity))
                throw new WebApiInnerException("1052", "参数值有误");

            return result;
        }

        // Api/v1/Activitys
        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="type">1：室内，2室外，3官方认</param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult List(int type, int pageNo, int limit)
        {
            if (type == 0)
                throw new WebApiInnerException("0001", "type为必填");
            if (pageNo == 0)
                throw new WebApiInnerException("0002", "pageNo为必填");
            if (limit == 0)
                throw new WebApiInnerException("0003", "limit为必填");
            var typeName = "";
            switch (type)
            {
                case 1:
                    typeName = "室内活动";
                    break;
                case 2:
                    typeName = "户外活动";
                    break;
                case 3:
                    typeName = "官方认证";
                    break;
                default:
                    throw new WebApiInnerException("1011", "类型无效");
            }
            var typeModel = _activityService.GetTypeByName(typeName);
            if (typeModel == null)
                throw new WebApiInnerException("1012", "类型不存在");

            int totalCount;
            var list = _activityService.GetListPagedByType<Models.Activity>(typeModel.Id, pageNo, limit, out totalCount);
            var result = new ApiResult();
            var data = new
            {
                TotalCount = totalCount,
                Activitys = list.Select(item => new ListActivityModel(item)).ToList()
            };

            result.SetData(data);

            return result;
        }


        // Api/v1/Activity/New
        /// <summary>
        /// 获取最新活动列表type=1首页，type=2活动圈
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult NewestList(int type)
        {
            int totalCount;

            Expression<Func<Models.Activity, bool>> expression;
            if (type == 1)
                expression = m => (m.Status != ActivityStatus.Delete) && (m.IsShowInFront);
            else
                expression = m => (m.Status != ActivityStatus.Delete);

            Expression<Func<Models.Activity, object>> orderByExpression = m => new { m.CreateTime };
            var list = _activityService.GetListPaged(1, 3, expression, orderByExpression, true, out totalCount);

            var result = new ApiResult();
            var data = new
            {
                TotalCount = totalCount,
                Activitys = list.Select(item => new ListActivityModel(item)).ToList()
            };
            result.SetData(data);

            return result;

        }

        // Api/v1/Activity/Detail/{activityId}
        /// <summary>
        /// 获取活动详情
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpGet]
        [BasicAuthentication(Forcible = false)]
        public ApiResult ActivityDetail(Guid activityId)
        {
            if (activityId == Guid.Empty)
                throw new WebApiInnerException("3005", "Id异常");
            var model = _activityService.GetActivityById(activityId);
            if (model == null)
                throw new WebApiInnerException("1021", "活动不存在");

            var hasApply = false;
            if (AuthorizedUser != null)
            {
                var myApply = _activityService.GetActivityApply(activityId, AuthorizedUser.Id);
                if (myApply != null)
                    hasApply = true;
            }
            var resData = new DetailActivityModel(model) { HasApply = hasApply };
            var result = new ApiResult();

            result.SetData(resData);
            return result;
        }


        // Api/v1/Activitys/MyCreate
        /// <summary>
        /// 我创建的活动
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [BasicAuthentication]
        public ApiResult MyCreateActivitys(int pageNo = 1, int limit = 10)
        {
            int totalCount;
            var list = _activityService.MyCreateActivitys(AuthorizedUser.Id, pageNo, limit, out totalCount);

            var result = new ApiResult();
            var data = new
            {
                TotalCount = totalCount,
                Activitys = list.Select(item => new ListActivityModel(item)).ToList()
            };
            result.SetData(data);

            return result;
        }


        // Api/v1/Activitys/MyApply
        /// <summary>
        /// 我参与的活动
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [BasicAuthentication]
        public ApiResult MyApplyActivitys(int pageNo = 1, int limit = 10)
        {
            int totalCount;
            var list = _activityService.MyApplyActivitys(AuthorizedUser.Id, pageNo, limit, out totalCount);

            var result = new ApiResult();
            var data = new
            {
                TotalCount = totalCount,
                Activitys = list.Select(item => new ListActivityModel(item)).ToList()
            };
            result.SetData(data);

            return result;
        }

        // Api/v1/Activity/{activityId}
        /// <summary>
        /// 删除活动
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpDelete]
        [BasicAuthentication]
        public ApiResult Delete(Guid activityId)
        {
            if (activityId == Guid.Empty)
                throw new WebApiInnerException("3005", "Id异常");

            var model = _activityService.GetActivityById(activityId);
            if (model == null || model.Status == Models.ActivityStatus.Delete)
                throw new WebApiInnerException("1041", "活动不存在");

            if (model.MemberId != AuthorizedUser.Id)
                throw new WebApiInnerException("1043", "您无权删除他人活动");
            if (!_activityService.Delete(model))
                throw new WebApiInnerException("1042", "活动删除失败");

            var result = new ApiResult();
            return result;
        }

        [HttpGet]
        [BasicAuthentication]
        public ApiResult TypeListCanPublish()
        {
            var list = _activityService.GetTypes();

            var result = new ApiResult();
            var data = new
            {
                Types = list.Where(me => me.TypeName != "官方认证").Select(item => new ListTypeModel
                {
                    Id = item.Id.ToString(),
                    TypeName = item.TypeName
                }).ToList()
            };

            result.SetData(data);

            return result;
        }

        /// <summary>
        /// 活动搜索
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult Search(string keyword, int pageNo = 1, int limit = 10)
        {
            int totalCount;
            var list = _activityService.Search(keyword, pageNo, limit, out totalCount);

            var result = new ApiResult();
            var data = new
            {
                TotalCount = totalCount,
                Activitys = list.Select(item => new ListActivityModel(item)).ToList()
            };
            result.SetData(data);

            return result;
        }

    }
}