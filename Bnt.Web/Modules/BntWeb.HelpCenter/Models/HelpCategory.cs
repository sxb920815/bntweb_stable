/* Models Code Auto-Generation 
    ======================================================================== 
        File name：		Activitys
        Module:			
        Author：		Kahr.Lu（陆康康）
        Create Time：		2016/6/30 14:20:04
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BntWeb.Data;

namespace BntWeb.HelpCenter.Models
{
    [Table(KeyGenerator.TablePrefix + "Help_Categories")]
    public class HelpCategory
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 类目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public Guid ParentId { get; set; }

        /// <summary>
        /// 所有上级类别Id集合
        /// </summary>
        public string MergerId { get; set; }

        /// <summary>
        /// 所有上级类别Name集合
        /// </summary>
        public string MergerTypeName { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public int Sort { get; set; }


        /// <summary>
        /// 叶子集合
        /// </summary>
        [NotMapped]
        public virtual List<HelpCategory> Childs { get; set; }

        /// <summary>
        /// 分类图标
        /// </summary>
        [NotMapped]
        public string HelpCategoryLogo { get; set; }
    }
}