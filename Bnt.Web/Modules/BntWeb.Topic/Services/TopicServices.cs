/* 
    ======================================================================== 
        File name：		TopicServices
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/22 10:57:26
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Web;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Logging;
using BntWeb.Security;
using BntWeb.Topic.Models;
using BntWeb.Comment;
using BntWeb.Comment.Models;
using BntWeb.Comment.Services;
using BntWeb.ContentMarkup;
using BntWeb.ContentMarkup.Models;
using BntWeb.ContentMarkup.Services;
using BntWeb.Topic.ApiModels;
using BntWeb.Utility.Extensions;

namespace BntWeb.Topic.Services
{
    public class TopicServices:ITopicService
    {
        private readonly ICurrencyService _currencyService;
        private readonly IStorageFileService _storageFileService;
        private readonly ICommentService _commentService;
        private readonly IMarkupService _markupService;
        private readonly string _fileTypeName = "TopicImage";
        public ILogger Logger { get; set; }
        public TopicServices(ICurrencyService currencyService, IStorageFileService storageFileService,ICommentService commentService, IMarkupService markupService)
        {
            _currencyService = currencyService;
            _storageFileService = storageFileService;
            _commentService = commentService;
            _markupService = markupService;
        }

        /// <summary>
        /// 话题信息列表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="expression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isDesc"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<Models.Topic> GetListPaged<TKey>(int pageIndex, int pageSize, Expression<Func<Models.Topic, bool>> expression, Expression<Func<Models.Topic, TKey>> orderByExpression, bool isDesc,
            out int totalCount)
        {
            using (var dbContext = new TopicDbContext())
            {
                var query = dbContext.Topics.Include(a => a.TopicType).Where(expression);
                totalCount = query.Count();
                if (isDesc)
                    query = query.OrderByDescending(orderByExpression);
                else
                    query = query.OrderBy(orderByExpression);

                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        public Models.Topic GetTopicById(Guid id)
        {
            using (var dbContext = new TopicDbContext())
            {
                var model = dbContext.Topics.Include(a=>a.TopicType).FirstOrDefault(me => me.Id == id);
                return model;
            }
        }

        public bool Delete(Models.Topic topic)
        {
            var result = _currencyService.DeleteByConditon<Models.Topic>(
                me => me.Id == topic.Id);
            //删除图片关联关系
            _storageFileService.DisassociateFile(topic.Id, TopicModule.Key, _fileTypeName);
            //删除评论
            _commentService.DeleteCommentBySourceId(topic.Id);
            //取消点赞
            

            if (result > 0)
                Logger.Operation($"删除话题-{topic.TopicTitle}:{topic.Id}:{topic.TopicContent}", TopicModule.Instance, SecurityLevel.Warning);

            return result > 0;
        }

        public Guid UpdateTopic(Models.Topic topic)
        {
            var result = _currencyService.Update<Models.Topic>(topic);
            if (!result)
                return Guid.Empty;
            Logger.Operation($"更新活动-{topic.TopicTitle}:{topic.Id}:{topic.TopicContent}", TopicModule.Instance, SecurityLevel.Normal);
            return topic.Id;
        }

        public Guid CreateTopic(Models.Topic topic)
        {
            topic.Id = KeyGenerator.GetGuidKey();
            topic.CreateTime = DateTime.Now;
            topic.Status = TopicStatus.Normal;
            topic.ReadCount = 0;
            topic.TransmitCount = 0;
            var result = _currencyService.Create<Models.Topic>(topic);
            if (!result)
            {
                return Guid.Empty;
            }
            Logger.Operation($"创建活动-{topic.TopicTitle}:{topic.Id}:{topic.TopicContent}", TopicModule.Instance, SecurityLevel.Normal);
            return topic.Id;
        }
        /// <summary>
        /// 获取话题图片
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        public List<StorageFile> GetTopicFiles(Guid topicId)
        {
            return _storageFileService.GetFiles(topicId, TopicModule.Key, _fileTypeName).ToList();
        }

        /// <summary>
        /// 获取我参与过的话题
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<Models.Topic> MyParticipateTopics(string memberId, int pageIndex, int pageSize, out int totalCount)
        {
            using (var dbContext = new TopicDbContext())
            {
                var query = from t in dbContext.Topics
                    where
                        (from c in dbContext.Comments where c.ModuleKey== TopicModule.Key && c.SourceType=="Topics" && c.MemberId==memberId select c.SourceId).
                            Union
                            (from m in dbContext.MarkUps where m.ModuleKey == TopicModule.Key && m.SourceType == "Topics" && m.MemberId == memberId
                             select m.SourceId).Contains(t.Id)
                    select t;


                totalCount = query.Count();

                query = query.OrderByDescending(me => me.CreateTime);
                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        /// <summary>
        /// 我的活动
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<Models.Topic> MyCreateTopics(string memberId, int pageIndex, int pageSize, out int totalCount)
        {
            using (var dbContext = new TopicDbContext())
            {
                var query = dbContext.Topics.Where(me => me.MemberId == memberId && me.Status>0);
                totalCount = query.Count();

                query = query.OrderByDescending(me => me.CreateTime);
                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        /// <summary>
        /// 活动搜索
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<Models.Topic> Search(string keyword, int pageIndex, int pageSize, out int totalCount)
        {
            using (var dbContext = new TopicDbContext())
            {
                var query = dbContext.Topics.Where(me => me.Status > 0);
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(me => me.TopicTitle.Contains(keyword) || me.TopicContent.Contains(keyword));
                }
                totalCount = query.Count();

                query = query.OrderByDescending(me => me.CreateTime);
                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        public List<TopicType> GetTypes()
        {
            using (var dbContext = new TopicDbContext())
            {
                return dbContext.TopicTypes.ToList();
            }
        }

        public bool SetHot(Guid topicId)
        {
            var model = _currencyService.GetSingleById<Models.Topic>(topicId);
            if (model == null)
                return false;
            model.IsHot = !model.IsHot;
            return _currencyService.Update(model);
        }
    }
}