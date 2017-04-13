/* 
    ======================================================================== 
        File name：		HelpCategory
        Module:			
        Author：		Kahr.Lu（陆康康）
        Create Time：		2016/7/6 20:39:21
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Configuration;
using System.Web.Http;
using BntWeb.HelpCenter.Services;
using BntWeb.Data.Services;
using BntWeb.FileSystems.Media;
using BntWeb.HelpCenter.ApiModels;
using BntWeb.Utility.Extensions;
using BntWeb.Validation;
using BntWeb.WebApi.Filters;
using BntWeb.HelpCenter.Models;
using BntWeb.WebApi.Models;

namespace BntWeb.HelpCenter.ApiControllers
{
    public class HelpCategoryController : BaseApiController
    {
        private readonly IHelpCenterService _helpCenterService;

        private readonly IStorageFileService _storageFileService;

        public HelpCategoryController(IHelpCenterService helpCenterService, IStorageFileService sIStorageFileService)
        {
            _helpCenterService = helpCenterService;
            _storageFileService = sIStorageFileService;
        }

        /// <summary>
        /// 递归帮助类别树结构
        /// </summary>
        /// <param name="toCategory"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        private List<TreeHelpCategoryModel> setCategoryTree(TreeHelpCategoryModel toCategory,
            List<HelpCategory> categories)
        {
            List<HelpCategory> childCategories = categories.Where(x => x.ParentId == toCategory.Id).OrderByDescending(x => x.Sort).ToList();
            if (childCategories.Count > 0)
            {
                toCategory.Childs = new List<TreeHelpCategoryModel>();
                foreach (var child in childCategories)
                {
                    //获取图标
                    var logo = _storageFileService.GetFiles(child.Id, HelpCenterModule.Key)
                        .Select(o => o.Simplified())
                        .FirstOrDefault();
                    var category = new TreeHelpCategoryModel(child, logo);
                    category.Childs = setCategoryTree(category, categories) ?? new List<TreeHelpCategoryModel>();
                    toCategory.Childs.Add(category);
                }
                
            }
            return toCategory.Childs;
        }

        /// <summary>
        /// 获取帮助类别树结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ApiResult GetHelpCategoriesTree()
        {
            var list = _helpCenterService.GetCategories();

            List<TreeHelpCategoryModel> categoryList = new List<TreeHelpCategoryModel>();

            foreach (var item in list.Where(me => me.ParentId == Guid.Empty).OrderByDescending(me => me.Sort))
            {
                //获取图标
                var logo =
                    _storageFileService.GetFiles(item.Id, HelpCenterModule.Key)
                        .Select(o => o.Simplified())
                        .FirstOrDefault();
                var category = new TreeHelpCategoryModel(item, logo);
                category.Childs = setCategoryTree(category, list) ?? new List<TreeHelpCategoryModel>();
                categoryList.Add(category);
            }

            var result = new ApiResult();
            var data = new
            {
                HelpCategories = categoryList
            };

            result.SetData(data);

            return result;
        }
    }
}
