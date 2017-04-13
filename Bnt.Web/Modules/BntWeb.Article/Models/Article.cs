using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using BntWeb.Data;

namespace BntWeb.Article.Models
{
    /// <summary>
    /// 实体：Articles
    /// </summary>
    [Table(KeyGenerator.TablePrefix + "Articles")]
    public class Article
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 文章类型
        /// </summary>
        public Guid TypeId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Blurb { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 读取数
        /// </summary>
        public int? ReadNum { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后一次更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public string CreateUserId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>
        public string CreateName { get; set; }

        /// <summary>
        /// 文章状态：1－正常，0－其他
        /// </summary>
        public ArticleStatus Status { get; set; }
 


        [ForeignKey("TypeId")]
        public virtual ArticleCategories ArticleCategories { get; set; }

    }

    public enum ArticleStatus
    {
        /// <summary>
        /// 待举行
        /// </summary>
        [Description("正常")]
        Ok = 1,
        /// <summary>
        /// 进行中
        /// </summary>
        [Description("其他")]
        Other = 0
    }

    /// <summary>
    /// 商家图片类型定义
    /// </summary>
    public static class ArticleImage
    {
      
        /// <summary>
        /// 图片，多图
        /// </summary>
        public static readonly string ArticleImages = "ArticleImages";
    }
}
