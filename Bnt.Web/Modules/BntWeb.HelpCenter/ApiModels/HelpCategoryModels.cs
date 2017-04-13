/* 
    ======================================================================== 
        File name：		HelpCategoryModels
        Module:			
        Author：		Kahr.Wu（陆康康）
        Create Time：		2016/7/6 20:46:16
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using BntWeb.FileSystems.Media;
using BntWeb.HelpCenter.Models;
using BntWeb.HelpCenter.Services;

namespace BntWeb.HelpCenter.ApiModels
{
    public class TreeHelpCategoryModel
    {
        /// <summary>
        /// 帮助类别Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string Name { get; set; }

        public SimplifiedStorageFile Logo { set; get; }

        public List<TreeHelpCategoryModel> Childs { get; set; }

        public TreeHelpCategoryModel(HelpCategory model, SimplifiedStorageFile logo)
        {
            Id = model.Id;
            Name = model.Name;

            Logo = logo;
        }
    }
}