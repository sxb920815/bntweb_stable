/* 
    ======================================================================== 
        File name：		Activity
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/17 16:26:51
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BntWeb.Data;

namespace BntWeb.Activity.Models
{
    [Table(KeyGenerator.TablePrefix + "Activitys")]
    public class Activity
    {
        /// <summary>
        /// 活动Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 活动封面图片
        /// </summary>
        public string CoverImage { get; set; }

        /// <summary>
        /// 活动地点
        /// </summary>
        public string Postion { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 截止时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 活动类别
        /// </summary>
        public Guid TypeId { get; set; }

        /// <summary>
        /// 活动介绍
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 活动状态：1-待举行；2-进行中；3-已结束；-1已删除
        /// </summary>
        public ActivityStatus Status { get; set; }

        /// <summary>
        /// 报名人数
        /// </summary>
        public int ApplyNum { get; set; }

        /// <summary>
        /// 限定人数：0表示不限定人数
        /// </summary>
        public int LimitNum { get; set; }

        /// <summary>
        /// 活动创始人Id,官方活动为0
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// 活动创始人名称
        /// </summary>
        public string MemberName { get; set; }
        /// <summary>
        /// 首页显示
        /// </summary>
        public bool IsShowInFront { set; get; }
        /// <summary>
        /// 精华
        /// </summary>
        public bool IsBest { set; get; }

        [ForeignKey("TypeId")]
        public virtual ActivityType ActivityType { get; set; }
    }

    public enum ActivityStatus
    {
        /// <summary>
        /// 待举行
        /// </summary>
        [Description("待举行")]
        Wait = 1,
        /// <summary>
        /// 进行中
        /// </summary>
        [Description("进行中")]
        Doing = 2,
        /// <summary>
        /// 已结束
        /// </summary>
        [Description("已结束")]
        Finish = 3,
        /// <summary>
        /// 进行中
        /// </summary>
        [Description("已删除")]
        Delete = -1
    }
}