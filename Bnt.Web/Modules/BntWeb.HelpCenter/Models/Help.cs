/* Models Code Auto-Generation 
    ======================================================================== 
        File name：		Activitys
        Module:			
        Author：		Kahr.Lu（陆康康）
        Create Time：		2016/6/30 14:23:04
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
namespace BntWeb.HelpCenter.Models
{
    [Table(KeyGenerator.TablePrefix + "Helps")]
    public class Help
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 类目Id
        /// </summary>
        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual HelpCategory HelpCategory { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreateName { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }

    public enum HelpStatus
    {
        [Description("前台显示")]
        Normal=1
    }
}