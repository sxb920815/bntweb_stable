using Autofac;
using BntWeb.Environment;
using BntWeb.FileSystems.Media;
using BntWeb.Merchant.Models;
using BntWeb.Merchant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BntWeb.Merchant.ApiModels
{
    public class MerchantModel
    {
        /// <summary>
		/// 
		/// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string MerchantName { get; set; }

        public string BranchName { set; get; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string District { get; set; }

        public string Address { get; set; }

        public SimplifiedStorageFile Image { get; set; }

        public string OpenTime { set; get; }

        public MerchantModel(Models.Merchant model)
        {
            Id = model.Id;
            MerchantName = model.MerchantName;
            BranchName = model.BranchName;
            Intro = model.Intro;
            City = model.City;
            District = model.District;
            OpenTime = model.OpenTime;
            Address = model.Address;

            var fileService = HostConstObject.Container.Resolve<IStorageFileService>();
            var file = fileService.GetFiles(model.Id, MerchantModule.Key, "LogoImage").FirstOrDefault();
            Image = file?.Simplified();
            if (!string.IsNullOrWhiteSpace(model.PCDS))
            {
                City = model.PCDS.Split(',').Length > 2 ? model.PCDS.Split(',')[1] : "";
                District = model.PCDS.Split(',').Length > 2 ? model.PCDS.Split(',')[2] : "";
            }
        }

    }

    public class MerchantDetail
    {
        /// <summary>
		/// 
		/// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string MerchantName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 分店
        /// </summary>
        public string BranchName { set; get; }
        /// <summary>
        /// 营业时间
        /// </summary>
        public string OpenTime { set; get; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 详情
        /// </summary>
        public string Detail { get; set; }


        public SimplifiedStorageFile LogoImage { set; get; }

        public object BackgroundImages { set; get; }

        public MerchantDetail(Models.Merchant model)
        {
            Id = model.Id;
            MerchantName = model.MerchantName;
            BranchName = model.BranchName ?? "";
            PhoneNumber = model.PhoneNumber;
            OpenTime = model.OpenTime;
            Intro = model.Intro;
            Address = model.Address;//$"{model.PCDS},{model.Address}";
            Detail = model.Detail;

            var fileService = HostConstObject.Container.Resolve<IStorageFileService>();
            var files = fileService.GetFiles(model.Id, MerchantModule.Key, "BackgroundImage");
            BackgroundImages = new List<object>();
            if (files != null && files.Count > 0)
                BackgroundImages = files.Select(me => me?.Simplified()).ToList();
            var logo = fileService.GetFiles(model.Id, MerchantModule.Key, "LogoImage").FirstOrDefault();
            LogoImage = logo?.Simplified();

        }
    }

    public class SearchAllModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { set; get; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { set; get; }
        /// <summary>
        /// 位置区域
        /// </summary>
        public string Postion { set; get; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// 活动：报名人数
        /// </summary>
        public int ApplyNum { set; get; }
        /// <summary>
        /// 活动：报名限定人数
        /// </summary>
        public int LimitNum { set; get; }
        /// <summary>
        /// 活动：开始时间
        /// </summary>
        public string StartTime { set; get; }
        /// <summary>
        /// 活动截止时间
        /// </summary>
        public string EndTime { set; get; }

        /// <summary>
        /// 商家：营业时间
        /// </summary>
        public string OpenTime { set; get; }

        /// <summary>
        /// 1:活动；2商家
        /// </summary>
        public int Type { set; get; }
        /// <summary>
        /// 活动：状态
        /// </summary>
        public int Status { set; get; }

        public  bool IsBest { set; get; }

        public SimplifiedStorageFile Image { get; set; }

        public SearchAllModel(MerchantActivity model)
        {
            Id = model.Id;
            Title = model.Title;
            Postion = model.Postion;
            Description = model.Description;
            ApplyNum = model.ApplyNum;
            LimitNum = model.LimitNum;
            StartTime = model.StartTime;
            EndTime = model.EndTime;
            OpenTime = model.OpenTime;
            Type = model.Type;
            Status = model.Status;
            IsBest = model.IsBest;

            var fileService = HostConstObject.Container.Resolve<IStorageFileService>();
            StorageFile file;
            file = Type == 1 ? fileService.GetFiles(model.Id, "BntWeb-Activity", "ActivityImage").FirstOrDefault() : fileService.GetFiles(model.Id, MerchantModule.Key, "LogoImage").FirstOrDefault();
            Image = file?.Simplified();

        }
    }
}
