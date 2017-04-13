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
using Autofac;
using BntWeb.Environment;
using BntWeb.FileSystems.Media;
using BntWeb.MemberBase.Services;

namespace BntWeb.Activity.ApiModels
{
    public class SubmitCommentModel
    {
        public Guid? ParentId { get; set; }

        public string Content { get; set; }
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
        /// 头像
        /// </summary>
        public SimplifiedStorageFile Avatar { get; set; }

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

            var memberService = HostConstObject.Container.Resolve<IMemberService>();
            Avatar = memberService.GetAvatarFile(comment.MemberId)?.Simplified();
            
            ChildComments = new List<ListCommentModel>();
            if (comment.ChildComments == null || comment.ChildComments.Count <= 0) return;
            foreach (var c in comment.ChildComments)
            {
                ChildComments.Add(new ListCommentModel(c));
            }
        }
    }
}