using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BntWeb.LimitBuy.ViewModels
{
    public class ViewSingleGoods
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 货号
        /// </summary>
        public string SingleGoodsNo { get; set; }

        /// <summary>
        /// 所属商品Id
        /// </summary>
        public Guid GoodsId { get; set; }

        /// <summary>
        /// 售价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int Stock { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Specification
        {
            get { return string.Join(" ; ", lists); }

        }



        public List<string> lists
        {
            get;
            set;
        }
    }
}