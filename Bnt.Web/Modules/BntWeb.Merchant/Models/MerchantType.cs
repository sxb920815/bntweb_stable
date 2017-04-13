
/* Models Code Auto-Generation 
    ======================================================================== 
        File name：		MerchantType
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/24 11:33:32
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System; 
using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using BntWeb.Data;

namespace BntWeb.Merchant.Models
{    
    /// <summary>
    /// 实体：MerchantType
    /// </summary>
    [Table(KeyGenerator.TablePrefix + "Merchant_Type")]
    public partial class MerchantType
    {    
        /// <summary>
		/// 
		/// </summary>
        public Guid Id { get; set;}
        
        /// <summary>
		/// 
		/// </summary>
        public string TypeName { get; set;}
        
        /// <summary>
		/// 备注
		/// </summary>
        public string Remark { get; set;}
        
        /// <summary>
		/// 显示顺序
		/// </summary>
        public short Sort { get; set;}
        
        /// <summary>
		/// 是否推荐显示
		/// </summary>
        public bool? IsShowInNav { get; set;}
        
        /// <summary>
		/// 
		/// </summary>
        public string Icon { get; set;}
        
        /// <summary>
		/// 父级分类Id
		/// </summary>
        public Guid ParentId { get; set;}

        /// <summary>
        /// 联合Id字符串
        /// </summary>
        public string MergerId { set; get; }

        /// <summary>
        ///  联合Name字符串
        /// </summary>
        public string MergerTypeName { set; get; }

        /// <summary>
        /// 0禁用，1正常
        /// </summary>
        public int Status { get; set;}
           
    }
}
