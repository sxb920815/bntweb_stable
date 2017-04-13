/* 
    ======================================================================== 
        File name：		TopicModels
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/22 17:53:55
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BntWeb.FileSystems.Media;
using BntWeb.Environment;
using BntWeb.MemberBase.Services;
using Autofac;

namespace BntWeb.Topic.ApiModels
{
    public class CreateTopicModel
    {
        /// <summary>
        /// 话题内容
        /// </summary>
        public string TopicContent { get; set; }

        public List<Base64Image> Images { set; get; }
    }

    public class ListTopicModel
    {
        /// <summary>
        /// 话题Id
        /// </summary>
        public Guid Id { get; set; }

        public string MemberName { set; get; }

        /// <summary>
        /// 话题内容
        /// </summary>
        public string TopicContent { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否热门
        /// </summary>
        public bool IsHot { get; set; }
        /// <summary>
        /// 判断是否标记点赞
        /// </summary>
        public bool HasLiked { set; get; }

        public string MemberId { set; get; }

        public SimplifiedStorageFile Avatar { set; get; }

        public List<SimplifiedStorageFile> Images { set; get; }

        public List<ListCommentModel> Comments { set; get; }

        public ListTopicModel(Models.Topic model, List<StorageFile> imgList, List<ListCommentModel> commentList,bool hasMakeUp)
        {
            Id = model.Id;
            TopicContent = model.TopicContent;
            CreateTime = model.CreateTime;
            IsHot = model.IsHot;
            HasLiked = hasMakeUp;
            MemberId = model.MemberId;
            Images = imgList.Select(me=>me?.Simplified()).ToList();
            Comments = commentList;

            var memberService = HostConstObject.Container.Resolve<IMemberService>();
            Avatar = memberService.GetAvatarFile(model.MemberId)?.Simplified();
            MemberName = model.MemberName;
        }
    }
}