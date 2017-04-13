using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BntWeb.Environment;
using BntWeb.FileSystems.Media;
using BntWeb.Mall.Models;
using Autofac;

namespace BntWeb.Mall.ApiModels
{
    public class ListGoodsModel
    {
        public Guid Id { get; set; }
        
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// 售价，如果有多个规格的商品，取最低价格
        /// </summary>
        public decimal ShopPrice { get; set; }
        
        /// <summary>
        /// 付款数量
        /// </summary>
        public int PaymentAmount { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        public int SalesVolume { get; set; }

        /// <summary>
        /// 主图
        /// </summary>
        public SimplifiedStorageFile MainImage { set; get; }

        public ListGoodsModel(Goods model)
        {
            Id = model.Id;
            Name = model.Name;
            OriginalPrice = model.OriginalPrice;
            ShopPrice = model.ShopPrice;
            SalesVolume = model.SalesVolume;
            PaymentAmount = model.PaymentAmount;
            var fileService = HostConstObject.Container.Resolve<IStorageFileService>();
            var mainImage = fileService.GetFiles(model.Id, MallModule.Key, "MainImage").FirstOrDefault();
            MainImage = mainImage?.Simplified();
        }
    }

    public class ListGoodsViewModel
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }
        public decimal OriginalPrice { get; set; }

        /// <summary>
        /// 售价，如果有多个规格的商品，取最低价格
        /// </summary>
        public decimal ShopPrice { get; set; }

        /// <summary>
        /// 付款数量
        /// </summary>
        public int PaymentAmount { get; set; }

        /// <summary>
        /// 销量
        /// </summary>
        public int SalesVolume { get; set; }

        /// <summary>
        /// 主图
        /// </summary>
        public SimplifiedStorageFile MainImage { set; get; }

        public ListGoodsViewModel(GoodsView model)
        {
            Id = model.Id;
            Name = model.Name;
            OriginalPrice = model.OriginalPrice;
            ShopPrice = model.ShopPrice;
            SalesVolume = model.SalesVolume;
            PaymentAmount = model.PaymentAmount;
            var fileService = HostConstObject.Container.Resolve<IStorageFileService>();
            var mainImage = fileService.GetFiles(model.Id, MallModule.Key, "MainImage").FirstOrDefault();
            MainImage = mainImage?.Simplified();
        }
    }

    /// <summary>
    /// 筛选条件
    /// </summary>
    public class FilterCriteriasModel
    {
        /// <summary>
        /// 属性ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 属性 多个用,隔开
        /// </summary>
        public string Value { get; set; }
    }

    public class GoodsPostModel
    {
        /// <summary>
        /// 商品分类ID
        /// </summary>
        public Guid CategoryId { get; set; }

        /// <summary>
        /// 关键词
        /// </summary>
        public string KeyWord { get; set; }

        /// <summary>
        /// 排序类型
        /// </summary>
        public GoodsSortType SortType { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// 每页条数
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// 品牌id
        /// </summary>
        public List<Guid> Brands { get; set; }

        /// <summary>
        /// 其他属性
        /// </summary>
        public List<AttributeModel> Others { get; set; }

        /// <summary>
        /// 最低价格
        /// </summary>
        public decimal MinPrice { get; set; }

        /// <summary>
        /// 最高价格
        /// </summary>
        public decimal MaxPrice { get; set; }
    }

    public class AttributeModel
    {
        /// <summary>
        /// 属性Id
        /// </summary>
        public Guid AttributeId { get; set; }
        /// <summary>
        /// 属性值 多个用,分隔
        /// </summary>
        public string AttributeValues { get; set; }
    }

}
