using System;

namespace BntWeb.LimitBuy.Models
{
    public class GoodsModel
    {
        public string Id { get; set; }

        public int Stock { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}