using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BntWeb.Article.Services;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.Security;
using BntWeb.Validation;

namespace BntWeb.Article.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private const string ArticleImages = "ArticleImages";
        private readonly IStorageFileService _storageFileService;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="articleService"></param>
        /// <param name="storageFileService"></param>
        /// <param name="currencyService"></param>
        /// <param name="userContainer"></param>
        public ArticleController(IArticleService articleService, IStorageFileService storageFileService, ICurrencyService currencyService, IUserContainer userContainer)
        {
            _articleService = articleService;
            _storageFileService = storageFileService;
        }

        // GET: H5
        public ActionResult NewsInfo(Guid id)
        {
            //网址/Article/d7702424-aa07-466d-8414-ea37cddcdd9b
            Argument.ThrowIfNull(id.ToString(), "Id");

            Models.Article article  = _articleService.GetOneArticleById(id);
            Argument.ThrowIfNull(article, "信息不存在");
          
            ViewBag.articleImage =
            _storageFileService.GetFiles(id, ArticleModule.Key, ArticleImages);


            return View(article);
        }
    }
}