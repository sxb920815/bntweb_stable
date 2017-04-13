
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using BntWeb.HelpCenter.Models;
using BntWeb.Data;

namespace BntWeb.HelpCenter
{
    
    public class HelpCenterDbContext : BaseDbContext
    {
        public DbSet<HelpCategory> HelpCategories { get; set; }
        public DbSet<Help> Helps { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
