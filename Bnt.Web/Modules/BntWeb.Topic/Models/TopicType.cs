
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
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using BntWeb.Data;

namespace BntWeb.Topic.Models
{
    /// <summary>
    /// 实体：topic_type
    /// </summary>
    [Table(KeyGenerator.TablePrefix + "Topic_Type")]
    public  class TopicType
    {
        /// <summary>
        /// 类型Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 话题类型
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 状态:0禁用，1启用
        /// </summary>
        public int Status { get; set; }

    }
}
