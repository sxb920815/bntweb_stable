using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace BntWeb.HelpCenter.ViewModels
{
    public class EditHelpModel
    {
        /// <summary>
        /// 帮助Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 帮助标题
        /// </summary>
        [Required]
        [Display(Name="帮助标题")]
        public string Title { get; set; }

        /// <summary>
        /// 帮助内容
        /// </summary>
        [Required]
        [Display(Name ="帮助内容")]
        public string Content { get; set; }

        /// <summary>
        /// 类别Id集合（用逗号隔开）
        /// </summary>
        public string CategoryIds { get; set; }


    }
}