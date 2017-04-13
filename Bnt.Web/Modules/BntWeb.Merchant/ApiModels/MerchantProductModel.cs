using Autofac;
using BntWeb.FileSystems.Media;
using BntWeb.Merchant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BntWeb.Merchant.ApiModels
{
    public class MerchantProductModel
    {
        /// <summary>
		/// 
		/// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set; }

        public SimplifiedStorageFile MainImage { set; get; }

        public MerchantProductModel(Models.MerchantProduct model)
        {
            Id = model.Id;
            ProductName = model.ProductName;
            Intro = model.Intro;

            var _storageFileService = Environment.HostConstObject.Container.Resolve<IStorageFileService>();
            var image = _storageFileService.GetFiles(model.Id,MerchantModule.Key, "MainImage").FirstOrDefault();
            if(image!= null)
            {
                MainImage = image?.Simplified();
            }
        }
    }

    public class ImageModel
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
    }


    public class HomeMerchantProductModel
    {
        /// <summary>
		/// 
		/// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProductName { get; set; }

        public string MerchantName { set; get; }

        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set; }

        public SimplifiedStorageFile MainImage { set; get; }

        public HomeMerchantProductModel(Models.MerchantProduct model)
        {
            Id = model.Id;
            ProductName = model.ProductName;
            Intro = model.Intro;
            MerchantName = model.Merchant.MerchantName;

            var _storageFileService = Environment.HostConstObject.Container.Resolve<IStorageFileService>();
            var image = _storageFileService.GetFiles(model.Id, MerchantModule.Key, "MainImage").FirstOrDefault();
            if (image != null)
            {
                MainImage = image?.Simplified();
            }
        }
    }

    public class HomeMerchantModel
    {
        /// <summary>
		/// 商家Id
		/// </summary>
        public Guid Id { get; set; }
        
        public string MerchantName { set; get; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Intro { get; set; }

        public SimplifiedStorageFile MainImage { set; get; }

        public HomeMerchantModel(Models.Merchant model)
        {
            Id = model.Id;
            Intro = model.Intro;
            MerchantName = model.MerchantName;

            var _storageFileService = Environment.HostConstObject.Container.Resolve<IStorageFileService>();
            var image = _storageFileService.GetFiles(model.Id, MerchantModule.Key, "LogoImage").FirstOrDefault();
            if (image != null)
            {
                MainImage = image?.Simplified();
            }
        }
    }
}
