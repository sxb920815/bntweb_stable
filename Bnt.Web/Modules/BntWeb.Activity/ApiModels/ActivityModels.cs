/* 
    ======================================================================== 
        File name：		ActivityModels
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/21 16:29:16
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using BntWeb.Activity.Models;
using BntWeb.Activity.Services;
using BntWeb.Environment;
using BntWeb.FileSystems.Media;
using BntWeb.MemberBase.Services;

namespace BntWeb.Activity.ApiModels
{
    public class CreateActivityModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 活动封面图片,base64
        /// </summary>
        public Base64Image CoverImage { get; set; }

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
        public string TypeId { get; set; }

        /// <summary>
        /// 活动介绍
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 限定人数：0表示不限定人数
        /// </summary>
        public int LimitNum { get; set; }

    }

    public class UpdateActivityModel
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 活动封面图片,base64
        /// </summary>
        public Base64Image CoverImage { get; set; }

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
        public string TypeId { get; set; }

        /// <summary>
        /// 活动介绍
        /// </summary>
        public string Description { get; set; }

        ///// <summary>
        ///// 限定人数：0表示不限定人数
        ///// </summary>
        //public int LimitNum { get; set; }
    }

    public class ListActivityModel
    {
        public Guid Id { set; get; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 活动封面图片,base64
        /// </summary>
        public SimplifiedStorageFile CoverImage { get; set; }

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
        public string TypeName { get; set; }

        /// <summary>
        /// 活动介绍
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 限定人数：0表示不限定人数
        /// </summary>
        public int LimitNum { get; set; }
        /// <summary>
        /// 限定人数：0表示不限定人数
        /// </summary>
        public int ApplyNum { get; set; }

        public int Status { set; get; }

        public bool IsBest { get; set; }

        public DateTime CreateTime { set; get; }
        

        public ListActivityModel(Models.Activity model)
        {
            Id = model.Id;
            Title = model.Title;
            Description = model.Description;
            StartTime = model.StartTime;
            EndTime = model.EndTime;
            CreateTime = model.CreateTime;
            ApplyNum = model.ApplyNum;
            LimitNum = model.LimitNum;
            Postion = model.Postion;
            Status = (int)model.Status;
            IsBest = model.IsBest;

            var activityService = HostConstObject.Container.Resolve<IActivityService>();
            var file = activityService.GetActivityFile(model.Id);
            CoverImage = file?.Simplified();

            var type = activityService.GetTypeById(model.TypeId) ?? new ActivityType();
            TypeName = type.TypeName;

        }
    }


    public class DetailActivityModel
    {
        public Guid Id { set; get; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 活动封面图片,base64
        /// </summary>
        public SimplifiedStorageFile CoverImage { get; set; }

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
        public string TypeName { get; set; }

        /// <summary>
        /// 活动介绍
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 限定人数：0表示不限定人数
        /// </summary>
        public int LimitNum { get; set; }
        /// <summary>
        /// 限定人数：0表示不限定人数
        /// </summary>
        public int ApplyNum { get; set; }

        public int Status { set; get; }
        public DateTime CreateTime { set; get; }

        public bool HasApply { set; get; }

        public string MemberId { set; get; }

        /// <summary>
        /// 头像
        /// </summary>
        public SimplifiedStorageFile Avatar { get; set; }

        public DetailActivityModel(Models.Activity model)
        {
            Id = model.Id;
            Title = model.Title;
            Description = model.Description;
            StartTime = model.StartTime;
            EndTime = model.EndTime;
            CreateTime = model.CreateTime;
            ApplyNum = model.ApplyNum;
            LimitNum = model.LimitNum;
            Postion = model.Postion;
            Status = (int)model.Status;
            MemberId = model.MemberId;

            var activityService = HostConstObject.Container.Resolve<IActivityService>();
            var file = activityService.GetActivityFile(model.Id);
            CoverImage = file?.Simplified();


            var memberService = HostConstObject.Container.Resolve<IMemberService>();
            Avatar = memberService.GetAvatarFile(model.MemberId)?.Simplified();

            var type = activityService.GetTypeById(model.TypeId) ?? new ActivityType();
            TypeName = type.TypeName;
        }
    }

    public class AppyActivityModel
    {

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
    }

    public class ListApplyMemberModel
    {
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

        public SimplifiedStorageFile Avatar { set; get; }

        public ListApplyMemberModel(ActivityApply model)
        {
            RealName = model.RealName;
            PhoneNumber = model.PhoneNumber;
            Remark = model.Remark;
            var memberService = HostConstObject.Container.Resolve<IMemberService>();

            var file = memberService.GetAvatarFile(model.MemberId);
            Avatar = file?.Simplified();
        }
    }

    public class ListTypeModel
    {
        public string Id { get; set; }

        public string TypeName { set; get; }
    }
}