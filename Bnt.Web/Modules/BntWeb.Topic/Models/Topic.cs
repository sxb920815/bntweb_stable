
/* Models Code Auto-Generation 
    ======================================================================== 
        File name：		ActivityApply
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/22 10:51:04
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

namespace BntWeb.Topic.Models
{
    /// <summary>
    /// 实体：topics
    /// </summary>
    [Table(KeyGenerator.TablePrefix + "Topics")]
    public class Topic
    {
        /// <summary>
        /// 话题Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        public Guid TypeId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string TopicTitle { get; set; }

        /// <summary>
        /// 话题内容
        /// </summary>
        public string TopicContent { get; set; }

        /// <summary>
        /// 发布人Id
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// 发布人名称
        /// </summary>
        public string MemberName { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public TopicStatus Status { get; set; }

        /// <summary>
        /// 是否热门
        /// </summary>
        public bool IsHot { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ReadCount { get; set; }

        /// <summary>
        /// 转发次数
        /// </summary>
        public int TransmitCount { get; set; }

        [ForeignKey("TypeId")]
        public virtual TopicType TopicType{get;set;}
    }

    public enum TopicStatus
    {

        /// <summary>
        /// 未审核
        /// </summary>
        [Description("未审核")]
        NotAudited = 0,
        /// <summary>
        /// 正常
        /// </summary>
        [Description("正常")]
        Normal = 1
    }
}
