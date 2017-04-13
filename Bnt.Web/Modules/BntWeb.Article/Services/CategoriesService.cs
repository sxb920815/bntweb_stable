using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Article.Models;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.Logging;
using BntWeb.Security;

namespace BntWeb.Article.Services
{
    public class CategoriesService
    {
        #region private定义
        private readonly ICurrencyService _currencyService;

        public ILogger Logger { get; set; }
        #endregion

        #region 构造
        public CategoriesService(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }
        #endregion

        #region 商家分类

        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <returns></returns>
        public List<ArticleCategories> GetTypes()
        {
            using (var dbContext = new ArticleDbContext())
            {
                return dbContext.ArticleCategories.ToList();
            }
        }

        public ArticleCategories GetTypeById(Guid id)
        {
            return _currencyService.GetSingleById<Models.ArticleCategories>(id);
        }
        public ArticleCategories GetTypeByName(string name)
        {
            return _currencyService.GetSingleByConditon<ArticleCategories>(me => me.Name == name);
        }
        /// <summary>
        /// 保存类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Guid SaveType(ArticleCategories model)
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
            Logger.Operation($"{operateType}文章分类-{model.Name}:{model.Id}", ArticleModule.Instance, SecurityLevel.Normal);
            return model.Id;
        }
        /// <summary>
        /// 删除商家分类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool DeleteType(ArticleCategories model)
        {
            var result = _currencyService.DeleteByConditon<ArticleCategories>(
                me => me.Id == model.Id);
            if (result > 0)
                Logger.Operation($"删除文章分类-{model.Name}:{model.Id}", ArticleModule.Instance, SecurityLevel.Warning);

            return result > 0;
        }

        public bool InsetTypeRelation(ArticleCategories model)
        {
            model.Id = KeyGenerator.GetGuidKey();
            return _currencyService.Create(model);
        }

        //public bool DeleteRelations(Guid TypeId)
        //{
        //    return _currencyService.DeleteByConditon<ArticleCategories>(me => me.Id == merchantId) > 0;
        //}

        //public List<ArticleCategories> GetTypeRelations(Guid merchantId)
        //{
        //    using (var dbContext = new ArticleDbContext())
        //    {
        //        return dbContext.ArticleCategories.Include(m => m).Where(me => me.MerchantId == merchantId).ToList();
        //    }
        //}
        #endregion
    }
}