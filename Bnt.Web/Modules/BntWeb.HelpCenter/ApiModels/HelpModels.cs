/* 
    ======================================================================== 
        File name：		HelpModels
        Module:			
        Author：		Kahr.Wu（陆康康）
        Create Time：		2016/7/6 20:45:16
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using BntWeb.Environment;
using BntWeb.HelpCenter.Models;
using BntWeb.HelpCenter.Services;

namespace BntWeb.HelpCenter.ApiModels
{
    public class ListHelpModel
    {
        /// <summary>
        /// 帮助Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 帮助标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// H5地址
        /// </summary>
        public string Url { get; set; }

        public ListHelpModel(Help model)
        {
            Id = model.Id;
            Title = model.Title;
            Url = HostConstObject.HostUrl + "HelpCenter/" + Id;
        }
    }

    public class DetaliHelpModel
    {
        /// <summary>
        /// 帮助Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 帮助标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 帮助内容
        /// </summary>
        public string Content { get; set; }

        public DetaliHelpModel(Help model)
        {
            Id = model.Id;
            Title = model.Title;
            CreateTime = model.CreateTime;
            Content = model.Content;
        }
    }
}