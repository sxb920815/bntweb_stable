
/* Models Code Auto-Generation 
    ======================================================================== 
        File name：		ArticleCategories
        Module:			
        Author：		WSQ
        Create Time：		2016/6/28 17:18:46
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System; 
using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using BntWeb.Data;

namespace BntWeb.Article.Models
{
    /// <summary>
    /// 实体：ArticleCategory
    /// </summary>
    [Table(KeyGenerator.TablePrefix + "Article_Categories")]
    public partial class ArticleCategory
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

    }
}