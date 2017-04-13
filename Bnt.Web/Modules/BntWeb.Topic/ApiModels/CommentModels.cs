/* 
    ======================================================================== 
        File name：        CommentModels
        Module:                
        Author：            罗嗣宝
        Create Time：    2016/6/17 15:11:52
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/

using System;
using System.Collections.Generic;
using BntWeb.Environment;
using BntWeb.FileSystems.Media;

namespace BntWeb.Topic.ApiModels
{
    public class SubmitCommentModel
    {
        public Guid? ParentId { get; set; }

        public string Content { get; set; }
    }

    public class MemberAvatar
    {
        /// <summary>
        /// 原图地址
        /// </summary>
        public string RelativePath { get; set; }
        /// <summary>
        /// 中等尺寸缩略图，图片类型可用
        /// 
        /// </summary>
        public string MediumThumbnail { get; set; }
        /// <summary>
        /// 小尺寸缩略图，图片类型可用
        /// 
        /// </summary>
        public string SmallThumbnail { get; set; }

        public MemberAvatar(StorageFile storageFile)
        {
            RelativePath = storageFile.RelativePath;
            MediumThumbnail = storageFile.MediumThumbnail;
            SmallThumbnail = storageFile.SmallThumbnail;
        }
    }

    public sealed class ListCommentModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 评论人编号
        /// </summary>
        public string MemberId { get; set; }

        /// <summary>
        /// 评论编号
        /// </summary>
        public string MemberName { get; set; }

        public string MemberPhone { get; set; }

        /// <summary>
        /// 父级评论Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 子评论
        /// </summary>
        public List<ListCommentModel> ChildComments { get; set; }

        public ListCommentModel(Comment.Models.Comment comment)
        {
            Id = comment.Id;
            Content = comment.Content;
            MemberId = comment.MemberId;
            MemberName = comment.MemberName;
            MemberPhone = comment.MemberPhone;
            ParentId = comment.ParentId;
            CreateTime = comment.CreateTime;
            
            ChildComments = new List<ListCommentModel>();
            if (comment.ChildComments == null || comment.ChildComments.Count <= 0) return;
            foreach (var c in comment.ChildComments)
            {
                ChildComments.Add(new ListCommentModel(c));
            }
        }
    }
}