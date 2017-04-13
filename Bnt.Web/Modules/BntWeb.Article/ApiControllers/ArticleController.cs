using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using BntWeb.Article.ApiModels;
using BntWeb.Article.Services;

using BntWeb.WebApi.Models;

namespace BntWeb.Article.ApiControllers
{
    public class ArticleController : BaseApiController
    {
        private readonly IArticleService _articleService;
       

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        // Api/v1/Article
        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        public ApiResult ArticleList( int pageNo = 1, int limit = 10)
        {
            
            int totalCount;
           

            Expression<Func<Models.Article, object>> orderByExpression = m => new { m.CreateTime };

            var isDesc = true;
            var listArticle = _articleService.GetApiListPaged(pageNo, limit, orderByExpression, isDesc, out totalCount);

            List<ArticleModel> resList = new List<ArticleModel>();
            foreach (var article in listArticle)
            {
                 
                //获取图片
                var imgList = _articleService.GetArticleFile(article.Id);

                var resModel = new ArticleModel(article, imgList);

                resList.Add(resModel);
            }

            var result = new ApiResult();
            var data = new
            {
                TotalCount = totalCount,
                ArticleList = resList
            };
            result.SetData(data);

            return result;
        }
    }
}