
/* Models Code Auto-Generation 
    ======================================================================== 
        File name：		MerchantProducts
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/24 11:33:32
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System; 
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using BntWeb.Data;

namespace BntWeb.Merchant.Models
{    
    /// <summary>
    /// 实体：商家优惠信息
    /// </summary>
    [Table(KeyGenerator.TablePrefix + "Merchant_Products")]
    public partial class MerchantProduct
    {    
        /// <summary>
		/// 
		/// </summary>
        public Guid Id { get; set;}
        
        /// <summary>
		/// 
		/// </summary>
        public string ProductName { get; set;}
        
        /// <summary>
		/// 简介
		/// </summary>
        public string Intro { get; set;}
        
        /// <summary>
		/// 商品详情
		/// </summary>
        public string Detail { get; set;}
        
        /// <summary>
		/// 是否前端（首页）显示
		/// </summary>
        public bool IsShowInFront { get; set;}
        
        /// <summary>
		/// 是否推荐
		/// </summary>
        public bool IsRecommend { get; set;}
        
        /// <summary>
		/// 
		/// </summary>
        public Guid MerchantId { get; set;}
        
        /// <summary>
		/// 
		/// </summary>
        public DateTime CreateTime { get; set;}
        
        /// <summary>
		/// 
		/// </summary>
        public MerchantProductStatus Status { get; set;}

        [ForeignKey("MerchantId")]
        public virtual Merchant Merchant { get; set; }
    }

    public enum MerchantProductStatus
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1
    }
}
