using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BntWeb.Article.ViewModels
{
    public class EditArticleCategoriesModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        [Required]
        [Display(Name = "类型名称")]
        public string Name { get; set; }

        /// <summary>
        /// 合并名称
        /// </summary>
        public string MergerName { get; set; }

        /// <summary>
        /// 合并ID
        /// </summary>
        public string MergerId { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int? Level { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }

        public int EditMode { get; set; }
    }
}