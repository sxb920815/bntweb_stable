using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BntWeb.Activity.ViewModels
{
    public class EditActivityViewModel
    {
        /// <summary>
        /// 活动Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [Display(Name = "标题")]
        public string Title { get; set; }

        /// <summary>
        /// 活动封面图片
        /// </summary>
        [Display(Name = "活动封面图片")]
        public string CoverImage { get; set; }

        /// <summary>
        /// 活动地点
        /// </summary>
        [Required]
        [Display(Name = "活动地点")]
        public string Postion { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        [Display(Name = "开始时间")]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        [Required]
        [Display(Name = "截止时间")]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 活动类别
        /// </summary>
        [Required]
        [Display(Name = "活动类别")]
        public Guid TypeId { get; set; }

        /// <summary>
        /// 活动介绍
        /// </summary>
        [Required]
        [Display(Name = "活动介绍")]
        public string Description { get; set; }
        
    }
}