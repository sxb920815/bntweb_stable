using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BntWeb.Merchant.ViewModels
{
   public  class EditMerchantProductModel
    {
        /// <summary>
		/// 
		/// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// 商品详情
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 是否前端（首页）显示
        /// </summary>
        public bool IsShowInFront { get; set; }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsRecommend { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid MerchantId { get; set; }

        public string MainImage { set; get; }

        public string ProductImages { set; get; }
    }
}
