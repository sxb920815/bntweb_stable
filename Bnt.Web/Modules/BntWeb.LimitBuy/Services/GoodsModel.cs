using System;

namespace BntWeb.LimitBuy.Services
{
    public class GoodsModel
    {
        public string Id { get; set; }
        public string GoodsId { get; set; }

        public int Stock { get; set; }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}