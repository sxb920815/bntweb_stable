
/* Models Code Auto-Generation 
    ======================================================================== 
        File name：		Merchants
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
    /// 实体：Merchants
    /// </summary>
    [Table(KeyGenerator.TablePrefix + "Merchants")]
    public partial class Merchant
    {    
        /// <summary>
		/// 
		/// </summary>
        public Guid Id { get; set;}
        
        /// <summary>
		/// 名称
		/// </summary>
        public string MerchantName { get; set;}
        
        /// <summary>
		/// 联系电话
		/// </summary>
        public string PhoneNumber { get; set;}
        /// <summary>
        /// 分店
        /// </summary>
        public string BranchName { set; get; }
        /// <summary>
        /// 营业时间
        /// </summary>
        public string OpenTime { set; get; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set;}
        
        /// <summary>
		/// 
		/// </summary>
        public string Province { get; set;}
        
        /// <summary>
		/// 
		/// </summary>
        public string City { get; set;}
        
        /// <summary>
		/// 
		/// </summary>
        public string District { get; set;}
        
        /// <summary>
		/// 
		/// </summary>
        public string Street { get; set; }
        /// <summary>
        /// 省市区县
        /// </summary>
        public string PCDS { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set;}
        
        /// <summary>
		/// 详情
		/// </summary>
        public string Detail { get; set;}
        
        /// <summary>
		/// 0禁用，1正常
		/// </summary>
        public MerchantStatus Status { get; set;}
        
        /// <summary>
		/// 
		/// </summary>
        public DateTime CreateTime { get; set;}
        
        /// <summary>
		/// 是否前端（首页）显示
		/// </summary>
        public bool IsHowInFront { get; set;}
        
        /// <summary>
		/// 是否推荐
		/// </summary>
        public bool IsRecommend { get; set;}

    }
    public enum MerchantStatus
    {
        /// <summary>
        /// 已删除
        /// </summary>
        [Description("已删除")]
        Delete = -1,
        /// <summary>
        /// 禁用
        /// </summary>
        [Description("禁用")]
        Disable = 0,
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1
    }

    /// <summary>
    /// 商家图片类型定义
    /// </summary>
    public static class MerchantImageType
    {
        /// <summary>
        /// Log图片
        /// </summary>
        public static readonly string MerchantLog = "MerchantLog";
        /// <summary>
        /// 商家详情背景图片，多图
        /// </summary>
        public static readonly string MerchantBackGround = "MerchantBackGround";
    }
}
