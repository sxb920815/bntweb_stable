using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BntWeb.Article.Models;
using BntWeb.Data;
using BntWeb.Security;

namespace BntWeb.Article.Services
{
    interface ICategoriesService : IDependency
    {
        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <returns></returns>
        List<ArticleCategories> GetTypes();

        ArticleCategories GetTypeById(Guid id);

        ArticleCategories GetTypeByName(string name);

        /// <summary>
        /// 保存类型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Guid SaveType(ArticleCategories model);

        /// <summary>
        /// 删除商家分类
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool DeleteType(ArticleCategories model);

        bool InsetTypeRelation(ArticleCategories model);
    }
}
