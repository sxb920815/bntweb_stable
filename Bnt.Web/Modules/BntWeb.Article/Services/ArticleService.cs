using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using BntWeb.Article.Models;
using BntWeb.Data;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Logging;
using BntWeb.Mvc;
using BntWeb.Security;
using BntWeb.Utility;
using BntWeb.Utility.Extensions;

namespace BntWeb.Article.Services
{
    public class ArticleService: IArticleService
    {
        private readonly ICurrencyService _currencyService;
        private readonly IStorageFileService _storageFileService;
        private readonly string _fileTypeName = "ArticleImages";
        public ILogger Logger { get; set; }
        public ArticleService(ICurrencyService currencyService, IStorageFileService storageFileService)
        {
            _currencyService = currencyService;
            _storageFileService = storageFileService;
        }


        //#region 活动
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
        public List<Models.Article> GetListPaged<TKey>(int pageIndex, int pageSize, Expression<Func<Models.Article, bool>> expression, Expression<Func<Models.Article, TKey>> orderByExpression, bool isDesc, out int totalCount)
        {
            using (var dbContext = new ArticleDbContext())
            {
                var query = dbContext.Article.Include(a => a.ArticleCategories).Where(expression);
                totalCount = query.Count();
                if (isDesc)
                    query = query.OrderByDescending(orderByExpression);
                else
                    query = query.OrderBy(orderByExpression);

                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="orderByExpression"></param>
        /// <param name="isDesc"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<Models.Article> GetApiListPaged<TKey>(int pageIndex, int pageSize, Expression<Func<Models.Article, TKey>> orderByExpression, bool isDesc, out int totalCount)
        {
            using (var dbContext = new ArticleDbContext())
            {
                var query = dbContext.Article.Include(a => a.ArticleCategories);
                totalCount = query.Count();
                if (isDesc)
                    query = query.OrderByDescending(orderByExpression);
                else
                    query = query.OrderBy(orderByExpression);

                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        public Models.Article GetArticleById(Guid id)
        {
            return _currencyService.GetSingleById<Models.Article>(id);
        }

 

        public Models.Article GetOneArticleById(Guid id)
        {
            using (var dbContext = new ArticleDbContext())
            {
                var query = dbContext.Article.Include(a => a.ArticleCategories).Where(me => me.Id.Equals(id));

                var list = query.ToList().FirstOrDefault();
                return list;
            }
        }


        public int HasArticleCount(Guid typeId)
        {
            using (var dbContext = new ArticleDbContext())
            {
                return dbContext.Article.Count(me => me.TypeId == typeId);
            }
        }

        public bool Delete(Models.Article article)
        {
            var id = article.Id.ToString().ToGuid();

            //逻辑删除活动
            //article.Status = ArticleStatus.Other;
            //var result = _currencyService.Update(article);

            var result = _currencyService.DeleteByConditon<Models.Article>(
                me => me.Id == id);
            if (result > 0)
                Logger.Operation($"删除活动-{article.Title}:{article.Id}", ArticleModule.Instance, SecurityLevel.Warning);

            return result > 0;
        }

        public Guid CreateArticle(Models.Article article)
        {
            article.Id = KeyGenerator.GetGuidKey();
            article.CreateTime = DateTime.Now;
            article.Status = ArticleStatus.Ok;
            article.ReadNum=0;

            var result = _currencyService.Create(article);
            
           
            if (!result)
            {
                return Guid.Empty;
            }

            Logger.Operation($"创建文章-{article.Title}:{article.Id}", ArticleModule.Instance);
            return article.Id;
        }
        public Guid UpdateArticle(Models.Article article)
        {
            var result = _currencyService.Update(article);
            if (!result)
                return Guid.Empty;
            Logger.Operation($"更新文章-{article.Title}:{article.Id}", ArticleModule.Instance);
            return article.Id;
        }

        public List<Models.Article> GetListPagedByType<TKey>(string typeId, int pageIndex, int pageSize,
            out int totalCount)
        {
            using (var dbContext = new ArticleDbContext())
            {
                var query = dbContext.Article.Include(a => a.TypeId).Where(me => me.TypeId.Equals(typeId) && me.Status > 0);
                totalCount = query.Count();
                query = query.OrderByDescending(me => me.CreateTime);

                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        public List<StorageFile> GetArticleFile(Guid articleId)
        {
            return _storageFileService.GetFiles(articleId, ArticleModule.Key, _fileTypeName).ToList();
        }
        

        /// <summary>
        /// 文章搜索
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<Models.Article> Search(string keyword, int pageIndex, int pageSize, out int totalCount)
        {
            using (var dbContext = new ArticleDbContext())
            {
                var query = dbContext.Article.Where(me => me.Status > 0);
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(me => me.Title.Contains(keyword));
                    //修改过
                }
                totalCount = query.Count();

                query = query.OrderByDescending(me => me.CreateTime);
                var list = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return list;
            }
        }

        //#endregion


        //#region 文章类型 

        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <returns></returns>
        public List<Models.ArticleCategories> GetTypes()
        {
            using (var dbContext = new ArticleDbContext())
            {
                return dbContext.ArticleCategories.ToList();
            }
        }

        public DataJsonResult Save(ArticleCategories categories, bool isNew)
        {
            var result = new DataJsonResult();
            try
            {

                var nameChanged = true;
                if (!isNew)
                {
                    var oldCategories = _currencyService.GetSingleById<ArticleCategories>(categories.Id);
                    if (oldCategories != null)
                    {
                        //判断名称是否变更，如果变更，则需要递归更新子节点
                        if (oldCategories.Name.Equals(categories.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            nameChanged = false;
                        }
                    }
                    else
                    {
                        //转为新增
                        isNew = true;
                    }

                } 

                if (!isNew)
                {
                    result.Success = _currencyService.Update(categories);
                    if (nameChanged)
                    {
                        //更新所有子节点
                        UpdateChildsInfo(categories);
                    }
                }
                else
                {
                    categories.Id= KeyGenerator.GetGuidKey();
                    result.Success = _currencyService.Create(categories);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "出现异常，类型失败";

                Logger.Error(ex, "保存类型失败");
            }

            return result;
        }

        private void UpdateChildsInfo(ArticleCategories parentCategories)
        {
            var childs =
                _currencyService.GetList<ArticleCategories>(
                    d => d.ParentId.Equals(parentCategories.Id));

            foreach (var categories in childs)
            {
                //更新结点
                categories.MergerName = (parentCategories.MergerName + "," + categories.Name).Trim(',');
                _currencyService.Update(categories);

                UpdateChildsInfo(categories);
            }
        }

        public bool Delete(ArticleCategories model)
        {
            var result = _currencyService.DeleteByConditon<ArticleCategories>(
              me => me.Id == model.Id);
            if (result > 0)
                Logger.Operation($"删除文章分类-{model.Name}:{model.Id}", ArticleModule.Instance, SecurityLevel.Warning);

            return result > 0;
        }
    }
}
 