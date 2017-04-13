using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BntWeb.Article.Models;
using BntWeb.FileSystems.Media;
using BntWeb.Mvc;

namespace BntWeb.Article.Services
{
    public interface IArticleService : IDependency
    {
        List<Models.Article> GetListPaged<TKey>(int pageIndex, int pageSize,
            Expression<Func<Models.Article, bool>> expression,
            Expression<Func<Models.Article, TKey>> orderByExpression, bool isDesc, out int totalCount);

        List<Models.Article> GetApiListPaged<TKey>(int pageIndex, int pageSize, 
          Expression<Func<Models.Article, TKey>> orderByExpression, bool isDesc, out int totalCount);

        Models.Article GetArticleById(Guid id);
        bool Delete(Models.Article article);

        Guid CreateArticle(Models.Article article);
        Guid UpdateArticle(Models.Article article);

        List<Models.Article> GetListPagedByType<TKey>(string typeId, int pageIndex, int pageSize,
            out int totalCount);

        List<StorageFile> GetArticleFile(Guid articleId);

      
        Models.Article GetOneArticleById(Guid id);

        int HasArticleCount(Guid typeId);

        /// <summary>
        /// 活动搜索
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Models.Article> Search(string keyword, int pageIndex, int pageSize, out int totalCount);


        #region 文章类型

        List<Models.ArticleCategories> GetTypes();
        DataJsonResult Save(ArticleCategories categories, bool isNew);
        bool Delete(ArticleCategories model);

        #endregion
    }
}
