using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Autofac;
using BntWeb.Data;
using BntWeb.Environment;
using BntWeb.FileSystems.Media;
using BntWeb.Mall;
using BntWeb.Mall.Models;
using Newtonsoft.Json;

namespace BntWeb.LimitBuy.Models
{
    [Table(KeyGenerator.TablePrefix + "Goods_Single_Limit")]
    public class LimitSingleGoods
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public Guid GoodsId { get; set; }

        /// <summary>
        /// 单品Id
        /// </summary>
        public Guid SingleGoodId { get; set; }
        /// <summary> 
        /// 单品名称  
        /// </summary>
        public string SingleGoodsName { get; set; }
        /// <summary>
        /// 原价
        /// </summary>
        public decimal OriginalPrice { get; set; }
        /// <summary>
        /// 抢购价
        /// </summary>
        public decimal LimitPrice { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        [DefaultValue(0)]
        public int Stock { get; set; }
        /// <summary>
        /// 抢购数量
        /// </summary>
        [DefaultValue(0)]
        public int Quantity { get; set; }
        /// <summary>
        /// 开始抢购时间
        /// </summary>
        public DateTime BeginTime { get; set; }
        /// <summary>
        /// 抢购结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public LimitSingleGoodsStatus Status { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 是否允许使用优惠 0不允许 1允许
        /// </summary>
        public bool UseOffers { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal Weight { get; set; }

        [ForeignKey("GoodsId")]
        [JsonIgnore]
        public virtual Goods Goods { get; set; }

    }

    public enum LimitSingleGoodsStatus
    {
        /// <summary>
        /// 已删除
        /// </summary>
        [Description("已删除")]
        Delete = -1,

        /// <summary>
        /// 未开始
        /// </summary>
        [Description("未开始")]
        NotInSale = 0,

        /// <summary>
        /// 进行中
        /// </summary>
        [Description("进行中")]
        InSale = 1,
        /// <summary>
        /// 已结束
        /// </summary>
        [Description("已结束")]
        End = -2
    }

    public class ListLimitGoodsModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 单品Id
        /// </summary>
        public Guid SingleGoodId { get; set; }
        public Guid GoodsId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// 售价，如果有多个规格的商品，取最低价格
        /// </summary>
        public decimal LimitPrice { get; set; }

        public int Stock { get; set; }

        public int Quantity { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Specification { get; set; }

        public LimitSingleGoodsStatus Status { get; set; }

        /// <summary>
        /// 主图
        /// </summary>
        public SimplifiedStorageFile MainImage { set; get; }


        public string Description { get; set; }

        public bool UseOffers { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime TimeNow { get; set; }


        public ListLimitGoodsModel(LimitSingleGoods model, string description)
        {
            Id = model.Id;
            GoodsId = model.GoodsId;
            SingleGoodId = model.SingleGoodId;
            Name = model.SingleGoodsName;
            OriginalPrice = model.OriginalPrice;
            LimitPrice = model.LimitPrice;
            Stock = model.Stock;
            Quantity = model.Quantity;
            BeginTime = model.BeginTime;
            EndTime = model.EndTime;
            Specification = model.Specification;
            Status = model.Status;
            CreateTime = model.CreateTime;
            UseOffers = model.UseOffers;
            var fileService = HostConstObject.Container.Resolve<IStorageFileService>();
            var mainImage = fileService.GetFiles(model.GoodsId, MallModule.Key, "MainImage").FirstOrDefault();
            MainImage = mainImage?.Simplified();
            Description = description;
            TimeNow = DateTime.Now;
        }
    }

    public class ViewLimitGoodsModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 单品Id
        /// </summary>
        public Guid SingleGoodId { get; set; }
        public Guid GoodsId { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// 售价，如果有多个规格的商品，取最低价格
        /// </summary>
        public decimal LimitPrice { get; set; }

        public int Stock { get; set; }

        public int Quantity { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public string Specification { get; set; }

        public LimitSingleGoodsStatus Status { get; set; }

        /// <summary>
        /// 主图
        /// </summary>
        public List<SimplifiedStorageFile> MainImage { set; get; }


        public string Description { get; set; }

        public bool UseOffers { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime TimeNow { get; set; }


        public ViewLimitGoodsModel(LimitSingleGoods model, string description)
        {
            Id = model.Id;
            GoodsId = model.GoodsId;
            SingleGoodId = model.SingleGoodId;
            Name = model.SingleGoodsName;
            OriginalPrice = model.OriginalPrice;
            LimitPrice = model.LimitPrice;
            Stock = model.Stock;
            Quantity = model.Quantity;
            BeginTime = model.BeginTime;
            EndTime = model.EndTime;
            Specification = model.Specification;
            Status = model.Status;
            CreateTime = model.CreateTime;
            UseOffers = model.UseOffers;
            var fileService = HostConstObject.Container.Resolve<IStorageFileService>();
            var mainImage = fileService.GetFiles(model.GoodsId, MallModule.Key, "MainImage").Select(x => x.Simplified()).ToList();
            MainImage = mainImage;
            Description = description;
            TimeNow = DateTime.Now;
        }
    }
}