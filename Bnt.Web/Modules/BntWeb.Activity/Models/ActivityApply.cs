/* Models Code Auto-Generation 
    ======================================================================== 
        File name：		ActivityApply
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/17 10:04:49
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BntWeb.Data;

namespace BntWeb.Activity.Models
{
    /// <summary>
    /// 实体：ActivityApply
    /// </summary>
    [Table(KeyGenerator.TablePrefix + "Activity_Apply")]
    public class ActivityApply
    {
        /// <summary>
		/// 主键
		/// </summary>
        public Guid Id { get; set; }
        /// <summary>
		/// 活动Id
		/// </summary>
        public Guid ActivityId { get; set; }

        /// <summary>
		/// 会员Id
		/// </summary>
        public string MemberId { get; set; }

        /// <summary>
		/// 姓名
		/// </summary>
        public string RealName { get; set; }

        /// <summary>
		/// 电话
		/// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
		/// 备注
		/// </summary>
        public string Remark { get; set; }

        /// <summary>
		/// 报名时间
		/// </summary>
        public DateTime ApplyTime { get; set; }

        /// <summary>
		/// 状态
		/// </summary>
        public int Status { get; set; }
    }
}