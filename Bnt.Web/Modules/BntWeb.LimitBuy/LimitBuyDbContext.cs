

using System.Data.Entity;
using BntWeb.ContentMarkup.Models;
using BntWeb.Data;
using BntWeb.LimitBuy.Models;
using BntWeb.Mall.Models;
using BntWeb.OrderProcess.Models;

namespace BntWeb.LimitBuy
{
    public class LimitBuyDbContext : BaseDbContext
    {
        public DbSet<LimitSingleGoods> LimitSingleGoods { get; set; }


        public DbSet<Order> Order { get; set; }

        public DbSet<OrderGoods> OrderGoods { get; set; }

        public DbSet<Goods> Goods { get; set; }

    }
}