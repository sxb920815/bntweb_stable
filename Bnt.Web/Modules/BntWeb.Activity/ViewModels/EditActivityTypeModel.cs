/* 
    ======================================================================== 
        File name：		EditActivityTypeModel
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/21 11:28:59
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BntWeb.Activity.ViewModels
{
    public class EditActivityTypeModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        [Required]
        [Display(Name = "类型名称")]
        public string TypeName { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        public string Description { get; set; }
    }
}