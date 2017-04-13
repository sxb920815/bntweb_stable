using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BntWeb.Comment.Models;
using BntWeb.Comment.Services;
using BntWeb.ContentMarkup.Services;
using BntWeb.Data;
using BntWeb.MemberBase.Services;
using BntWeb.SystemMessage.Services;
using BntWeb.Topic.ApiModels;
using BntWeb.Topic.Services;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.WebApi.Filters;
using BntWeb.WebApi.Models;

namespace BntWeb.Topic.ApiControllers
{
    public class CommentController : BaseApiController
    {
        private readonly ITopicService _topicService;
        private readonly ISystemMessageService _systemMessageService;
        private readonly ICommentService _commentService;
        private readonly IMemberService _memberService;
        private const string SourceType = "Topics";

        public CommentController(ITopicService topicService, ISystemMessageService systemMessageService, ICommentService commentService, IMemberService memberService)
        {
            _topicService = topicService;
            _systemMessageService = systemMessageService;
            _commentService = commentService;
            _memberService = memberService;
        }

        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult List(Guid sourceId, int pageNo = 1, int limit = 10)
        {
            if (sourceId.Equals(Guid.Empty))
                throw new WebApiInnerException("0001", "话题Id类型格式不正确");
            int totalCount;
            var comments = _commentService.LoadByPage(sourceId, TopicModule.Instance, SourceType, out totalCount, pageNo, limit);
            var list = comments.Select(comment => new ListCommentModel(comment)).ToList();

            var result = new ApiResult();
            var data = new
            {
                TotalCount = totalCount,
                Comments = list
            };
            result.SetData(data);
            return result;
        }

        /// <summary>
        /// 发布评论
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="postComment"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ApiResult Create(Guid sourceId, [FromBody]SubmitCommentModel postComment)
        {
            Argument.ThrowIfNullOrEmpty(postComment.Content, "评论内容");
            if (sourceId.Equals(Guid.Empty))
                throw new WebApiInnerException("0001", "话题Id类型格式不正确");

            if (postComment.Content.Length > 1000)
                throw new WebApiInnerException("0002", "评论内容太长");

            if (postComment.ParentId == Guid.Empty)
                postComment.ParentId = null;

            var member = _memberService.FindMemberById(AuthorizedUser.Id);
            var comment = new Comment.Models.Comment()
            {
                Id = KeyGenerator.GetGuidKey(),
                Content = postComment.Content,
                MemberId = AuthorizedUser.Id,
                MemberName = member.NickName,
                CreateTime = DateTime.Now,
                IsAnonymity = false,
                ModuleKey = TopicModule.Key,
                ModuleName = TopicModule.DisplayName,
                ParentId = postComment.ParentId,
                SourceId = sourceId,
                Status = CommentStatus.Normal,
                Score = 0,
                SourceType = SourceType
            };

            _commentService.SaveComment(comment);

            var topic = _topicService.GetTopicById(sourceId);
            var content = $"{member.NickName}对你的话题进行了评论";
            var pushContent = content;
            _systemMessageService.CreatePushSystemMessage("话题评论", content, pushContent, topic.MemberId, topic.Id, null, "Topic", TopicModule.Key, SystemMessage.Models.MessageCategory.Personal);

            var result = new ApiResult();
            result.SetData(new ListCommentModel(comment));
            return result;
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [BasicAuthentication]
        public ApiResult Delete(Guid sourceId, string id)
        {
            Argument.ThrowIfNullOrEmpty(id, "Id不能为空");

            var result = new ApiResult();
            var commentId = id.ToGuid();
            if (commentId.Equals(Guid.Empty))
                throw new WebApiInnerException("0001", "评论id格式不正确");

            var comment = _commentService.GetComment(commentId);
            if (comment != null && comment.MemberId.Equals(AuthorizedUser.Id, StringComparison.OrdinalIgnoreCase))
                _commentService.DeleteComment(commentId);
            else
                throw new WebApiInnerException("0002", "只可以删除自己的评论");

            return result;
        }
    }
}
