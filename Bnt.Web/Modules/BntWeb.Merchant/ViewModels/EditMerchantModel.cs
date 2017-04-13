/* 
    ======================================================================== 
        File name：		EditMerchantModel
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/26 21:35:38
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BntWeb.Merchant.ViewModels
{
    public class EditMerchantModel
    {
        /// <summary>
		/// 
		/// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string MerchantName { get; set; }
        /// <summary>
        /// 分店
        /// </summary>
        public string BranchName { get; set; }
        /// <summary>
        /// 营业时间
        /// </summary>
        public string OpenTime { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Merchant_Province { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Merchant_City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Merchant_District { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Merchant_Street { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 详情
        /// </summary>
        public string Detail { get; set; }
        

        /// <summary>
        /// 是否前端（首页）显示
        /// </summary>
        public bool IsHowInFront { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsRecommend { get; set; }

        /// <summary>
        /// 类型列表，英文逗号分隔
        /// </summary>
        public string TypeIds { set; get; }

        public string LogoImage { set; get; }

        public string BackgroundImages { set; get; }
    }
}