/* 
    ======================================================================== 
        File name：        LikeController
        Module:                
        Author：            罗嗣宝
        Create Time：    2016/6/22 15:59:17
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using BntWeb.ContentMarkup.Models;
using BntWeb.ContentMarkup.Services;
using BntWeb.MemberBase.Services;
using BntWeb.SystemMessage.Services;
using BntWeb.Topic.Services;
using BntWeb.Utility.Extensions;
using BntWeb.WebApi.Filters;
using BntWeb.WebApi.Models;

namespace BntWeb.Topic.ApiControllers
{
    public class LikeController : BaseApiController
    {
        private readonly ITopicService _topicService;
        private readonly IMarkupService _markupService;
        private readonly IMemberService _memberService;
        private readonly ISystemMessageService _systemMessageService;

        public LikeController(ITopicService topicService, IMarkupService markupService, IMemberService memberService, ISystemMessageService systemMessageService)
        {
            _topicService = topicService;
            _markupService = markupService;
            _memberService = memberService;
            _systemMessageService = systemMessageService;
        }

        [HttpPost]
        [BasicAuthentication]
        public ApiResult Like(Guid topicId)
        {
            if (topicId.Equals(Guid.Empty))
                throw new WebApiInnerException("0001", "话题Id不合法");
            if (_markupService.MarkupExist(topicId, TopicModule.Key, AuthorizedUser.Id, MarkupType.Like))
                throw new WebApiInnerException("0001", "已经赞过了");

            if (_markupService.CreateMarkup(topicId, TopicModule.Key, AuthorizedUser.Id, MarkupType.Like))
            {
                var topic = _topicService.GetTopicById(topicId);
                var member = _memberService.FindMemberById(AuthorizedUser.Id);
                var content = $"{member.NickName}给你点赞啦~";
                var pushContent = content;
                _systemMessageService.CreatePushSystemMessage("有人给你点赞", content, pushContent, topic.MemberId, topic.Id, null, "Topic", TopicModule.Key, SystemMessage.Models.MessageCategory.Personal);
            }
            return new ApiResult();
        }

        [HttpDelete]
        [BasicAuthentication]
        public ApiResult CancelLike(Guid topicId)
        {
            if (topicId.Equals(Guid.Empty))
                throw new WebApiInnerException("0001", "话题Id不合法");
            if (!_markupService.MarkupExist(topicId, TopicModule.Key, AuthorizedUser.Id, MarkupType.Like))
                throw new WebApiInnerException("0001", "还没有点赞");

            _markupService.CancelMarkup(topicId, TopicModule.Key, AuthorizedUser.Id, MarkupType.Like);
            return new ApiResult();
        }
    }
}