/* 
    ======================================================================== 
        File name：		TopicController
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/22 17:50:43
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Http;
using BntWeb.Comment.Services;
using BntWeb.ContentMarkup.Models;
using BntWeb.ContentMarkup.Services;
using BntWeb.FileSystems.Media;
using BntWeb.MemberBase.Services;
using BntWeb.Topic.ApiModels;
using BntWeb.Topic.Models;
using BntWeb.Topic.Services;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.WebApi.Filters;
using BntWeb.WebApi.Models;

namespace BntWeb.Topic.ApiControllers
{
    public class TopicController : BaseApiController
    {

        private readonly ITopicService _topicService;
        private readonly IMemberService _memberService;
        private readonly IFileService _fileService;
        private readonly IStorageFileService _storageFileService;
        private readonly ICommentService _commentService;
        private readonly IMarkupService _markupService;
        private const string FileTypeName = "TopicImage";
        private const string SourceType = "Topics";

        public TopicController(ITopicService topicService,
            IMemberService memberService,
            IStorageFileService storageFileService,
            IFileService fileService,
            ICommentService commentService,
            IMarkupService markupService)
        {
            _topicService = topicService;
            _memberService = memberService;
            _fileService = fileService;
            _storageFileService = storageFileService;
            _commentService = commentService;
            _markupService = markupService;
        }


        // Api/v1/Topic
        /// <summary>
        /// 创建话题
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        [HttpPost]
        [BasicAuthentication]
        public ApiResult Create([FromBody]CreateTopicModel topic)
        {
            Argument.ThrowIfNullOrEmpty(topic.TopicContent, "内容");

            Argument.ThrowIfNull(topic.Images, "图片文件格式");
            if(topic.Images.Count > 9)
                throw new WebApiInnerException("1006", "图片数量不可以超过9张");
            foreach (var img in topic.Images)
            {
                Argument.ThrowIfNullOrEmpty(img.FileName, "图片名称");
                Argument.ThrowIfNullOrEmpty(img.Data, "图片数据");

                if (string.IsNullOrWhiteSpace(Path.GetExtension(img.FileName)))
                    throw new WebApiInnerException("1001", "图片名称没有包含扩展名");
            }


            ApiResult result = new ApiResult();
            List<Guid> imgIdList = new List<Guid>();
            foreach (var img in topic.Images)
            {
                StorageFile storageFile;
                var isSave = _fileService.SaveFile(img.Data, img.FileName, false, out storageFile, 200, 200, 100, 100, ThumbnailType.TakeCenter);
                if (isSave)
                    imgIdList.Add(storageFile.Id);
                else
                    throw new WebApiInnerException("1002", "图片上传失败");
            }

            var type = _topicService.GetTypes().FirstOrDefault();
            if (type == null)
                throw new WebApiInnerException("1003", "类型异常");

            var member = _memberService.FindMemberById(AuthorizedUser.Id);
            var model = new Models.Topic()
            {
                TopicTitle = "",
                TopicContent = topic.TopicContent,
                TypeId = type.Id,
                MemberId = member.Id,
                MemberName = member.NickName,
                CreateTime = DateTime.Now,
                Status = TopicStatus.Normal,
                IsHot = false
            };
            model.Id = _topicService.CreateTopic(model);

            if (model.Id == Guid.Empty)
                throw new WebApiInnerException("1004", "参数值有误");
            if (imgIdList.Any(imgId => !_storageFileService.AssociateFile(model.Id, TopicModule.Key, TopicModule.DisplayName, imgId, FileTypeName)))
            {
                throw new WebApiInnerException("1005", "图片关联失败");
            }

            return result;
        }

        // Api/v1/Topic/{topicId}
        /// <summary>
        /// 删除话题
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [HttpDelete]
        [BasicAuthentication]
        public ApiResult Delete(Guid topicId)
        {
            if (topicId == Guid.Empty)
                throw new WebApiInnerException("1010", "Id异常");

            var model = _topicService.GetTopicById(topicId);
            if (model == null)
                throw new WebApiInnerException("1011", "话题不存在");

            if (model.MemberId != AuthorizedUser.Id)
                throw new WebApiInnerException("1012", "您无权删除他人话题");
            if (!_topicService.Delete(model))
                throw new WebApiInnerException("1013", "话题删除失败");

            var result = new ApiResult();
            return result;
        }


        // Api/v1/Topics
        /// <summary>
        /// 获取话题列表[非强制授权]
        /// </summary>
        /// <param name="keyword">搜索关键字</param>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [BasicAuthentication(Forcible = false)]
        public ApiResult List(string keyword, int pageNo = 1, int limit = 10)
        {
            var checkKeyword = string.IsNullOrWhiteSpace(keyword);

            int totalCount;
            Expression<Func<Models.Topic, bool>> expression =
                l => (checkKeyword || l.TopicTitle.Contains(keyword)) &&
                     (l.Status > 0);


            Expression<Func<Models.Topic, object>> orderByExpression = m => new { m.CreateTime };

            var isDesc = true;
            var listTopic = _topicService.GetListPaged(pageNo, limit, expression, orderByExpression,
                isDesc, out totalCount);

            List<ListTopicModel> resList = new List<ListTopicModel>();
            foreach (var topic in listTopic)
            {
                //是否点赞
                var hasMakeUp = false;
                if (AuthorizedUser != null)
                    hasMakeUp = _markupService.MarkupExist(topic.Id, TopicModule.Key, AuthorizedUser.Id, MarkupType.Like);

                //获取图片
                var imgList = _topicService.GetTopicFiles(topic.Id);

                //获取评论前limit条
                var comments = _commentService.LoadByPage(topic.Id, TopicModule.Instance, SourceType, out totalCount, 1, limit);
                var commentList = comments.Select(comment => new ListCommentModel(comment)).ToList();

                var resModel = new ListTopicModel(topic, imgList, commentList, hasMakeUp);
                resList.Add(resModel);
            }

            var result = new ApiResult();
            var data = new
            {
                TotalCount = totalCount,
                Topics = resList
            };
            result.SetData(data);

            return result;
        }

        // Api/v1/Topic/MyCreate
        /// <summary>
        /// 我发布的话题
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [BasicAuthentication]
        public ApiResult MyCreateTopic(int pageNo = 1, int limit = 10)
        {
            int totalCount;
            Expression<Func<Models.Topic, bool>> expression =
                l => l.Status > 0 && l.MemberId == AuthorizedUser.Id;

            Expression<Func<Models.Topic, object>> orderByExpression = m => new { m.CreateTime };

            var isDesc = true;
            var listTopic = _topicService.GetListPaged(pageNo, limit, expression, orderByExpression,
                isDesc, out totalCount);

            List<ListTopicModel> resList = new List<ListTopicModel>();
            foreach (var topic in listTopic)
            {
                //是否点赞
                var hasMakeUp = false;
                if (AuthorizedUser != null)
                    hasMakeUp = _markupService.MarkupExist(topic.Id, TopicModule.Key, AuthorizedUser.Id, MarkupType.Like);

                //获取图片
                var imgList = _topicService.GetTopicFiles(topic.Id);

                //获取评论前limit条
                var comments = _commentService.LoadByPage(topic.Id, TopicModule.Instance, SourceType, out totalCount, 1, limit);
                var commentList = comments.Select(comment => new ListCommentModel(comment)).ToList();

                var resModel = new ListTopicModel(topic, imgList, commentList, hasMakeUp);
                resList.Add(resModel);
            }

            var result = new ApiResult();
            var data = new
            {
                TotalCount = totalCount,
                Topics = resList
            };
            result.SetData(data);
            return result;
        }

        // Api/v1/Topic/MyParticipate
        /// <summary>
        /// 我参与的话题
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [BasicAuthentication]
        public ApiResult MyParticipateTopic(int pageNo = 1, int limit = 10)
        {
            int totalCount;
            var listTopic = _topicService.MyParticipateTopics(AuthorizedUser.Id, pageNo, limit, out totalCount);

            List<ListTopicModel> resList = new List<ListTopicModel>();
            foreach (var topic in listTopic)
            {
                //是否点赞
                var hasMakeUp = false;
                if (AuthorizedUser != null)
                    hasMakeUp = _markupService.MarkupExist(topic.Id, TopicModule.Key, AuthorizedUser.Id, MarkupType.Like);

                //获取图片
                var imgList = _topicService.GetTopicFiles(topic.Id);

                //获取评论前limit条
                var comments = _commentService.LoadByPage(topic.Id, TopicModule.Instance, SourceType, out totalCount, 1, limit);
                var commentList = comments.Select(comment => new ListCommentModel(comment)).ToList();

                var resModel = new ListTopicModel(topic, imgList, commentList, hasMakeUp);
                resList.Add(resModel);
            }

            var result = new ApiResult();
            var data = new
            {
                TotalCount = totalCount,
                Topics = resList
            };
            result.SetData(data);

            return result;
        }
    }
}