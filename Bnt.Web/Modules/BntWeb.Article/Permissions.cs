/* 
    ======================================================================== 
        File name：        Permissions
        Module:                
        Author：            WSQ
        Create Time：    2016/6/28  
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.Security.Permissions;

namespace BntWeb.Article
{
    public class Permissions : IPermissionProvider
    {
        private static readonly string CategoryKey = ArticleModule.DisplayName;

        public const string ViewArticleKey = "BntWeb-Article-ViewArticle";
        public static readonly Permission ViewArticle = new Permission { Description = "查看文章", Name = ViewArticleKey, Category = CategoryKey };


        public const string DeleteArticleKey = "BntWeb-Article-DeleteArticle";
        public static readonly Permission DeleteArticle = new Permission { Description = "删除文章", Name = DeleteArticleKey, Category = CategoryKey };

        public const string EditArticleKey = "BntWeb-Article-EditArticle";
        public static readonly Permission EditArticle = new Permission { Description = "编辑文章", Name = EditArticleKey, Category = CategoryKey };


        public const string ViewCategoriesKey = "BntWeb-Article-ViewCategories";
        public static readonly Permission ViewCategories = new Permission { Description = "查看类型", Name = ViewCategoriesKey, Category = CategoryKey };

        public const string DeleteCategoriesKey = "BntWeb-Article-DeleteCategories";
        public static readonly Permission DeleteCategories = new Permission { Description = "删除类型", Name = DeleteCategoriesKey, Category = CategoryKey };

        public const string EditCategoriesKey = "BntWeb-Article-EditCategories";
        public static readonly Permission EditCategories = new Permission { Description = "编辑类型", Name = EditCategoriesKey, Category = CategoryKey };

      

        public int Position => ArticleModule.Position;

        public string Category => CategoryKey;

        public IEnumerable<Permission> GetPermissions()
        {
            return new[]
            {
                ViewArticle,
                EditArticle,
                DeleteArticle,
             
                ViewCategories,
                EditCategories,
                DeleteCategories,
               
            };
        }
    }
}