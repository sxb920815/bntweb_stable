/* 
    ======================================================================== 
        File name：		ActivityServices
        Module:			
        Author：		Kahr.Lu（陆康康）
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
using BntWeb.HelpCenter.Models;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.Logging;
using BntWeb.Security;
using BntWeb.Utility.Extensions;

namespace BntWeb.HelpCenter.Services
{
    public class HelpCenterService : IHelpCenterService
    {
        private readonly ICurrencyService _currencyService;

        public ILogger Logger { get; set; }
        public HelpCenterService(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        #region 帮助

        /// <summary>
        ///帮助分页
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="expression"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isDesc"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<Help> GetListPaged<TKey>(int pageIndex, int pageSize, Expression<Func<Help, bool>> expression,
            Expression<Func<Help, TKey>> orderByExpression, bool isDesc, out int totalCount)
        {
            using (var dbContext = new HelpCenterDbContext())
            {
                var query = dbContext.Helps.Include(a => a.HelpCategory).Where(expression);
                totalCount = query.Count();
                if (isDesc)
                    query = query.OrderByDescending(orderByExpression);
                else
                    query = query.OrderBy(orderByExpression);

                var list = query.Skip((pageIndex - 1)*pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        public Help GetHelpById(Guid id)
        {
            return _currencyService.GetSingleById<Help>(id);
        }

        public Help GetOneHelpById(Guid id)
        {
            using (var dbContext = new HelpCenterDbContext())
            {
                var query = dbContext.Helps.Include(a => a.HelpCategory).Where(me => me.Id.Equals(id));

                var list = query.ToList().FirstOrDefault();
                return list;
            }
        }

        /// <summary>
        /// 删除帮助
        /// </summary>
        /// <param name="help"></param>
        /// <returns></returns>
        public bool Delete(Help help)
        {
            var id = help.Id;

            //物理删除帮助
            var result = _currencyService.DeleteByConditon<Help>(
                me => me.Id == id);
            if (result > 0)
            {
                Logger.Operation($"删除帮助-{help.Title}:{help.Id}", HelpCenterModule.Instance, SecurityLevel.Warning);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 创建帮助
        /// </summary>
        /// <param name="help"></param>
        /// <returns></returns>
        public bool CreateHelp(Help help)
        {
            help.Id = KeyGenerator.GetGuidKey();
            help.CreateTime = DateTime.Now;
            help.LastUpdateTime = DateTime.Now;
            help.Status = 1;
            var result = _currencyService.Create(help);
            if (result)
                Logger.Operation($"创建帮助-{help.Title}:{help.Id}", HelpCenterModule.Instance);
            return result;
        }

        /// <summary>
        /// 更新帮助
        /// </summary>
        /// <param name="help"></param>
        /// <returns></returns>
        public bool UpdateHelp(Help help)
        {
            var result = _currencyService.Update(help);
            if (result)
                Logger.Operation($"更新帮助-{help.Title}:{help.Id}", HelpCenterModule.Instance);
            return result;
        }

        /// <summary>
        /// 根据类别查询帮助分页
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="categoryId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<Help> GetListPagedByCategory<TKey>(Guid categoryId, int pageIndex, int pageSize,
    out int totalCount)
        {
            using (var dbContext = new HelpCenterDbContext())
            {
                var query = dbContext.Helps.Include(a => a.HelpCategory).Where(me => me.CategoryId == categoryId && me.Status > 0);
                totalCount = query.Count();
                query = query.OrderByDescending(me => me.CreateTime);

                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        /// <summary>
        /// 帮助搜索
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<Help> Search(string keyword, int pageIndex, int pageSize, out int totalCount)
        {
            using (var dbContext = new HelpCenterDbContext())
            {
                var query = dbContext.Helps.Where(me => me.Status > 0);
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(me => me.Title.Contains(keyword));
                }
                totalCount = query.Count();

                query = query.OrderByDescending(me => me.CreateTime);
                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        #endregion

        #region 帮助类别

        /// <summary>
        /// 获取所有类别
        /// </summary>
        /// <returns></returns>
        public List<HelpCategory> GetCategories()
        {
            using (var dbContext = new HelpCenterDbContext())
            {
                return dbContext.HelpCategories.ToList();
            }
        }

        public HelpCategory GetCategoryById(Guid id)
        {
            return _currencyService.GetSingleById<HelpCategory>(id);
        }

        public HelpCategory GetCategoryByName(string name)
        {
            return _currencyService.GetSingleByConditon<HelpCategory>(me => me.Name == name);
        }

        /// <summary>
        /// 保存类别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SaveCategory(HelpCategory model)
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
            if (result)
                Logger.Operation($"{operateType}帮助类别-{model.Name}:{model.Id}", HelpCenterModule.Instance, SecurityLevel.Normal);
            return result;
        }

        /// <summary>
        /// 删除帮助类别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteCategory(HelpCategory model)
        {
            var result =
                _currencyService.DeleteByConditon<HelpCategory>(
                    x => x.Id == model.Id || x.MergerId.Contains(model.Id.ToString()));
            if (result > 0)
            {
                Logger.Operation($"删除帮助类别-{model.MergerTypeName}:{model.Id}", HelpCenterModule.Instance,
                    SecurityLevel.Warning);
                return true;
            }
            return false;
        }


        #endregion 
    }
}