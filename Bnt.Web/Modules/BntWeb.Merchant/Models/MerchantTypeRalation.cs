
/* Models Code Auto-Generation 
    ======================================================================== 
        File name：		MerchantTypeRalations
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
    /// 实体：MerchantTypeRalations
    /// </summary>
    [Table(KeyGenerator.TablePrefix + "Merchant_Type_Ralations")]
    public partial class MerchantTypeRalation
    {    
        /// <summary>
		/// 
		/// </summary>
        public Guid Id { get; set;}
        
        /// <summary>
		/// 
		/// </summary>
        public Guid MerchantId { get; set;}
        
        /// <summary>
		/// 
		/// </summary>
        public Guid MerchantTypeId { get; set;}

        [ForeignKey("MerchantTypeId")]
        public virtual MerchantType MerchantType { set; get; }


    }
}
