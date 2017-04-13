/* 
    ======================================================================== 
        File name：		IActivityService
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/17 11:18:45
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using BntWeb.Activity.Models;
using BntWeb.FileSystems.Media;

namespace BntWeb.Activity.Services
{
    public interface IActivityService : IDependency
    {
        #region 活动
        List<Models.Activity> GetListPaged<TKey>(int pageIndex, int pageSize,
            Expression<Func<Models.Activity, bool>> expression,
            Expression<Func<Models.Activity, TKey>> orderByExpression, bool isDesc, out int totalCount);

        Models.Activity GetActivityById(Guid id);
        bool Delete(Models.Activity activity);

        bool UpdateActivity(Models.Activity activity);
        Guid CreateActivity(Models.Activity activity);

        List<Models.Activity> GetListPagedByType<TKey>(Guid typeId,int pageIndex, int pageSize, out int totalCount);
        /// <summary>
        /// 获取活动图片
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        StorageFile GetActivityFile(Guid activityId);
        /// <summary>
        /// 我参与的活动
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Models.Activity> MyApplyActivitys(string memberId, int pageIndex, int pageSize, out int totalCount);

        /// <summary>
        /// 我创建的活动
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Models.Activity> MyCreateActivitys(string memberId, int pageIndex, int pageSize, out int totalCount);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Models.Activity> Search(string keyword, int pageIndex, int pageSize, out int totalCount);
        /// <summary>
        /// 定时更新活动状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        int SetActivitysStatus(ActivityStatus status);
        #endregion

        #region 活动类型
        List<Models.ActivityType> GetTypes();
        ActivityType GetTypeById(Guid id);
        ActivityType GetTypeByName(string typeName);
        Guid SaveType(ActivityType model);
        bool DeleteType(ActivityType model);
        #endregion

        #region 活动报名
        /// <summary>
        /// 获取报名信息
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        ActivityApply GetActivityApply(Guid activityId, string memberId);
        /// <summary>
        /// 活动报名列表
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<ActivityApply> GetApplyListPaged(Guid activityId, int pageIndex, int pageSize,out int totalCount);
        /// <summary>
        /// 报名
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool ActivityApply(ActivityApply model);

        bool CancelApply(Guid activityApplyId);

        #endregion
    }
}