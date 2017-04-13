/* 
    ======================================================================== 
        File name：		ActivityApply
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/21 17:39:21
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Web.Http;
using BntWeb.Activity.ApiModels;
using BntWeb.Activity.Models;
using BntWeb.Activity.Services;
using BntWeb.FileSystems.Media;
using BntWeb.MemberBase;
using BntWeb.MemberBase.Services;
using BntWeb.SystemMessage.Services;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.WebApi.Filters;
using BntWeb.WebApi.Models;

namespace BntWeb.Activity.ApiControllers
{
    public class ActivityApplyController : BaseApiController
    {
        private readonly IActivityService _activityService;
        private readonly IMemberService _memberService;
        private readonly ISystemMessageService _systemMessageService;

        public ActivityApplyController(IActivityService activityService, IMemberService memberService, ISystemMessageService systemMessageService)
        {
            _activityService = activityService;
            _memberService = memberService;
            _systemMessageService = systemMessageService;
        }

        /// <summary>
        /// 活动报名
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="applyModel"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ApiResult ApplyActivity(Guid activityId, [FromBody]AppyActivityModel applyModel)
        {
            if (activityId == Guid.Empty)
                throw new WebApiInnerException("3005", "Id异常");

            Argument.ThrowIfNullOrEmpty(applyModel.PhoneNumber, "手机号码");
            Argument.ThrowIfNullOrEmpty(applyModel.RealName, "姓名");
            Argument.ThrowIfNullOrEmpty(applyModel.Remark, "备注");

            var member = _memberService.FindMemberById(AuthorizedUser.Id);
            var model = new Models.ActivityApply()
            {
                ActivityId = activityId,
                MemberId = member.Id,
                PhoneNumber = applyModel.PhoneNumber,
                RealName = applyModel.RealName,
                Remark = applyModel.Remark,
                Status = 1,
                ApplyTime = DateTime.Now
            };
            var activity = _activityService.GetActivityById(activityId);
            if (activity == null)
                throw new WebApiInnerException("3003", "活动不存在");
            if (activity.Status != ActivityStatus.Wait)
                throw new WebApiInnerException("3004", "活动不在报名状态，无法报名");

            var myApply = _activityService.GetActivityApply(model.ActivityId, model.MemberId);
            if (myApply != null)
                throw new WebApiInnerException("3001", "您已经报名过了");

            if (!_activityService.ActivityApply(model))
                throw new WebApiInnerException("3002", "报名失败");

            activity.ApplyNum = activity.ApplyNum + 1;
            if (_activityService.UpdateActivity(activity))
            {
                var content = "有人报名了您发布的活动";
                var pushContent = content;
                _systemMessageService.CreatePushSystemMessage("有人报名您的活动", content, pushContent, activity.MemberId, activity.Id, null, "Activity", ActivityModule.Key, SystemMessage.Models.MessageCategory.Activity);
            }

            var result = new ApiResult();
            return result;
        }

        /// <summary>
        /// 取消报名
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpDelete]
        [BasicAuthentication]
        public ApiResult CancelApplyActivity(Guid activityId)
        {
            if (activityId == Guid.Empty)
                throw new WebApiInnerException("3005", "Id异常");

            var activity = _activityService.GetActivityById(activityId);
            if (activity == null)
                throw new WebApiInnerException("3013", "活动不存在");
            if (activity.Status != ActivityStatus.Wait)
                throw new WebApiInnerException("3004", "活动不在报名状态，无法取消");

            var myApply = _activityService.GetActivityApply(activityId, AuthorizedUser.Id);
            if (myApply == null)
                throw new WebApiInnerException("3011", "您未报名");

            if (!_activityService.CancelApply(myApply.Id))
                throw new WebApiInnerException("3012", "取消失败");

            activity.ApplyNum = activity.ApplyNum - 1;
            _activityService.UpdateActivity(activity);

            var result = new ApiResult();
            return result;
        }

        /// <summary>
        /// 报名人员查看
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [BasicAuthentication]
        public ApiResult Applylist(Guid activityId, int pageNo = 1, int limit = 10)
        {
            if (activityId == Guid.Empty)
                throw new WebApiInnerException("3005", "Id异常");
            int totalCount;
            var list = _activityService.GetApplyListPaged(activityId, pageNo, limit, out totalCount);

            var result = new ApiResult();
            var data = new
            {
                TotalCount = totalCount,
                Members = list.Select(item => new ListApplyMemberModel(item)).ToList()
            };
            result.SetData(data);
            return result;
        }
    }
}