/* 
    ======================================================================== 
        File name：		ITopicServices
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/22 10:56:02
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using BntWeb.FileSystems.Media;
using BntWeb.Topic.Models;

namespace BntWeb.Topic.Services
{
    public interface ITopicService : IDependency
    {
        #region 话题
        List<Models.Topic> GetListPaged<TKey>(int pageIndex, int pageSize,
            Expression<Func<Models.Topic, bool>> expression,
            Expression<Func<Models.Topic, TKey>> orderByExpression, bool isDesc, out int totalCount);

        Models.Topic GetTopicById(Guid id);
        bool Delete(Models.Topic topic);

        Guid UpdateTopic(Models.Topic topic);
        Guid CreateTopic(Models.Topic topic);
        
        /// <summary>
        /// 获取活动图片
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        List<StorageFile> GetTopicFiles(Guid topicId);
        /// <summary>
        /// 我参与的话题【包括点赞和评论过的】
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Models.Topic> MyParticipateTopics(string memberId, int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 我创建的活动
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Models.Topic> MyCreateTopics(string memberId, int pageIndex, int pageSize, out int totalCount);
        /// <summary>
        /// 活动搜索
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Models.Topic> Search(string keyword, int pageIndex, int pageSize, out int totalCount);

        List<TopicType> GetTypes();
        bool SetHot(Guid topicId);

        #endregion
    }
}