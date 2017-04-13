/* 
    ======================================================================== 
        File name：		EditActivityTypeModel
        Module:			
        Author：		Kahr.Lu（陆康康）
        Create Time：		2016/7/4 15:41:59
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BntWeb.HelpCenter.ViewModels
{
    public class EditHelpCategoryModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        [Required]
        [Display(Name ="类别名称")]
        public string CategoryName { get; set; }

        [Required]
        [Display(Name = "父级类别")]
        public Guid ParentId { get; set; }

        /// <summary>
        /// 联合Id字符串
        /// </summary>
        public string MergerId { set; get; }

        /// <summary>
        ///  联合Name字符串
        /// </summary>
        public string MergerTypeName { set; get; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public short Sort { get; set; }

        /// <summary>
        /// 分类图标
        /// </summary>
        public string HelpCategoryLogo { get; set; }
    }
}