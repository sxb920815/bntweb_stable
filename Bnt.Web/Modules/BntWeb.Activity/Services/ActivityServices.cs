/* 
    ======================================================================== 
        File name：		ActivityServices
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/17 11:35:45
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using BntWeb.Activity.Models;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Logging;
using BntWeb.MemberBase;
using BntWeb.Security;
using BntWeb.Utility.Extensions;
using LinqKit;

namespace BntWeb.Activity.Services
{
    public class ActivityServices : IActivityService
    {
        private readonly ICurrencyService _currencyService;
        private readonly IStorageFileService _storageFileService;
        private readonly string _fileTypeName = "ActivityImage";
        public ILogger Logger { get; set; }
        public ActivityServices(ICurrencyService currencyService, IStorageFileService storageFileService)
        {
            _currencyService = currencyService;
            _storageFileService = storageFileService;
        }

        #region 活动
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="expression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isDesc"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<Models.Activity> GetListPaged<TKey>(int pageIndex, int pageSize, Expression<Func<Models.Activity, bool>> expression, Expression<Func<Models.Activity, TKey>> orderByExpression, bool isDesc, out int totalCount)
        {
            using (var dbContext = new ActivityDbContext())
            {
                var query = dbContext.Activity.Include(a => a.ActivityType).Where(expression);
                totalCount = query.Count();
                if (isDesc)
                    query = query.OrderByDescending(orderByExpression);
                else
                    query = query.OrderBy(orderByExpression);

                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        public Models.Activity GetActivityById(Guid id)
        {
            return _currencyService.GetSingleById<Models.Activity>(id);
        }

        public bool Delete(Models.Activity activity)
        {
            var id = activity.Id;
            id = activity.Id.ToString().ToGuid();

            //逻辑删除活动
            activity.Status = ActivityStatus.Delete;
            var result = _currencyService.Update(activity);

            //var result = _currencyService.DeleteByConditon<Models.Activity>(
            //    me => me.Id == id);
            if (result)
                Logger.Operation($"删除活动-{activity.Title}:{activity.Id}", ActivityModule.Instance, SecurityLevel.Warning);

            return result;
        }

        public Guid CreateActivity(Models.Activity activity)
        {
            activity.Id = KeyGenerator.GetGuidKey();
            activity.CreateTime = DateTime.Now;
            activity.Status = ActivityStatus.Wait;
            activity.ApplyNum = 0;
            activity.LimitNum = activity.LimitNum;
            activity.CoverImage = activity.CoverImage ?? "";
            var result = _currencyService.Create<Models.Activity>(activity);
            if (!result)
            {
                return Guid.Empty;
            }
            Logger.Operation($"创建活动-{activity.Title}:{activity.Id}", ActivityModule.Instance, SecurityLevel.Normal);
            return activity.Id;
        }
        public bool UpdateActivity(Models.Activity activity)
        {
            var result = _currencyService.Update<Models.Activity>(activity);
            Logger.Operation($"更新活动-{activity.Title}:{activity.Id}", ActivityModule.Instance, SecurityLevel.Normal);
            return result;
        }

        public List<Models.Activity> GetListPagedByType<TKey>(Guid typeId, int pageIndex, int pageSize, out int totalCount)
        {
            using (var dbContext = new ActivityDbContext())
            {
                var query = dbContext.Activity.Include(a => a.ActivityType).Where(me => me.TypeId == typeId && me.Status > 0);
                totalCount = query.Count();
                query = query.OrderByDescending(me => me.IsBest).ThenByDescending(me => me.CreateTime);

                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        public StorageFile GetActivityFile(Guid activityId)
        {
            return _storageFileService.GetFiles(activityId, ActivityModule.Key, _fileTypeName).FirstOrDefault();
        }
        /// <summary>
        /// 分页获取我参与的活动信息
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<Models.Activity> MyApplyActivitys(string memberId, int pageIndex, int pageSize, out int totalCount)
        {
            using (var dbContext = new ActivityDbContext())
            {
                var query = from aa in dbContext.ActivityApply
                            join a in dbContext.Activity on aa.ActivityId equals a.Id
                            where aa.MemberId == memberId && a.Status > 0
                            orderby aa.ApplyTime descending
                            select a;

                totalCount = query.Count();

                query = query.OrderByDescending(me => me.CreateTime);
                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        public List<Models.Activity> MyCreateActivitys(string memberId, int pageIndex, int pageSize, out int totalCount)
        {
            using (var dbContext = new ActivityDbContext())
            {
                var query = dbContext.Activity.Where(me => me.MemberId == memberId && me.Status > 0);
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
        public List<Models.Activity> Search(string keyword, int pageIndex, int pageSize, out int totalCount)
        {
            using (var dbContext = new ActivityDbContext())
            {
                var query = dbContext.Activity.Where(me => me.Status > 0);
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(me => me.Title.Contains(keyword) || me.Description.Contains(keyword));
                }
                totalCount = query.Count();

                query = query.OrderByDescending(me => me.CreateTime);
                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }
        /// <summary>
        /// 定时更改活动状态
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public int SetActivitysStatus(ActivityStatus status)
        {
            using (var dbContext = new ActivityDbContext())
            {
                IQueryable<Models.Activity> query;
                if (status == ActivityStatus.Doing)
                {
                    query =
                        dbContext.Activity.Where(me => me.StartTime <= DateTime.Now && me.Status == ActivityStatus.Wait);
                }
                else if (status == ActivityStatus.Finish)
                {
                    query =
                        dbContext.Activity.Where(me => me.EndTime <= DateTime.Now && me.Status == ActivityStatus.Doing);
                }
                else
                    return 0;

                foreach (var item in query)
                {
                    if (status == ActivityStatus.Finish)
                    {
                        item.IsBest = false;
                    }
                    item.Status = status;
                }

                var count = dbContext.SaveChanges();
                return count;
            }
        }

        #endregion

        #region 活动类型

        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <returns></returns>
        public List<Models.ActivityType> GetTypes()
        {
            using (var dbContext = new ActivityDbContext())
            {
                return dbContext.ActivityType.ToList();
            }
        }

        public ActivityType GetTypeById(Guid id)
        {
            return _currencyService.GetSingleById<Models.ActivityType>(id);
        }
        public ActivityType GetTypeByName(string typeName)
        {
            return _currencyService.GetSingleByConditon<ActivityType>(me => me.TypeName == typeName);
        }
        /// <summary>
        /// 保存类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Guid SaveType(ActivityType model)
        {
            bool result;
            string operateType = "";
            if (model.Id == Guid.Empty)
            {
                operateType = "创建";
                model.Id = KeyGenerator.GetGuidKey();
                result = _currencyService.Create(model);
            }
            else
            {
                operateType = "更新";
                result = _currencyService.Update(model);
            }
            if (!result)
                return Guid.Empty;
            Logger.Operation($"{operateType}活动类型-{model.TypeName}:{model.Id}", ActivityModule.Instance, SecurityLevel.Normal);
            return model.Id;
        }
        /// <summary>
        /// 删除活动类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteType(ActivityType model)
        {
            var result = _currencyService.DeleteByConditon<ActivityType>(
                me => me.Id == model.Id);
            if (result > 0)
                Logger.Operation($"删除活动类型-{model.TypeName}:{model.Id}", ActivityModule.Instance, SecurityLevel.Warning);

            return result > 0;
        }

        #endregion

        #region 活动报名

        public ActivityApply GetActivityApply(Guid activityId, string memberId)
        {
            using (var dbContext = new ActivityDbContext())
            {
                var query = dbContext.ActivityApply.Where(me => me.ActivityId == activityId && me.MemberId == memberId);

                return query.FirstOrDefault();
            }
        }
        /// <summary>
        /// 分页获取活动报名列表
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<ActivityApply> GetApplyListPaged(Guid activityId, int pageIndex, int pageSize, out int totalCount)
        {
            using (var dbContext = new ActivityDbContext())
            {
                var query = dbContext.ActivityApply.Where(me => me.ActivityId == activityId && me.Status > 0);
                totalCount = query.Count();

                query = query.OrderByDescending(me => me.ApplyTime);
                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }
        /// <summary>
        /// 活动报名
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool ActivityApply(ActivityApply model)
        {
            if (model.Id == Guid.Empty)
                model.Id = KeyGenerator.GetGuidKey();

            model.ApplyTime = DateTime.Now;
            model.Status = 1;
            model.Remark = model.Remark ?? "";

            var result = _currencyService.Create(model);
            return result;
        }

        public bool CancelApply(Guid activityApplyId)
        {
            return _currencyService.DeleteByConditon<ActivityApply>(me => me.Id == activityApplyId) > 0;
        }

        #endregion
    }
}