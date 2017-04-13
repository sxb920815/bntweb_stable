/* 
    ======================================================================== 
        File name：        Channel
        Module:                
        Author：            罗嗣宝
        Create Time：    2016/8/3 15:53:33
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using BntWeb.Data;

namespace BntWeb.PromotionChannel.Models
{
    [Table(KeyGenerator.TablePrefix + "Promotion_Channels")]
    public class Channel
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(30)]
        public string Name { get; set; }

        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 推广人数
        /// </summary>
        [NotMapped]
        public int UsersCount { get; set; }
    }
}