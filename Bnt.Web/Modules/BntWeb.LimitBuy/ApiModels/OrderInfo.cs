using System;
using System.Linq;
using Autofac;
using BntWeb.Environment;
using BntWeb.FileSystems.Media;
using BntWeb.MemberBase;
using BntWeb.OrderProcess.Models;
using BntWeb.Utility.Extensions;

namespace BntWeb.LimitBuy.ApiModels
{
    public class OrderInfo
    {
        public Guid UserId { get; set; }
        public Guid GoodsId { get; set; }
        public bool IsBuy { get; set; }
    }


    public class LimitByOrder
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string MemberId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string MemberName { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        public SimplifiedStorageFile Avatar { get; set; }
        /// <summary>
        /// 订单生成时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public LimitByOrder(Order model)
        {
            MemberId = model.MemberId;
            MemberName = model.MemberName.Left(3) + "******" + model.MemberName.Right(2);
            CreateTime = model.CreateTime;
            var fileService = HostConstObject.Container.Resolve<IStorageFileService>();
            var mainImage =
                fileService.GetFiles(model.MemberId.ToGuid(), MemberBaseModule.Key, "Avatar").FirstOrDefault();
            Avatar = mainImage?.Simplified();
        }
    }
}