
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using BntWeb.Activity.Models;
using BntWeb.Data;

namespace BntWeb.Activity
{
    
    public class ActivityDbContext : BaseDbContext
    {
        public DbSet<Models.Activity> Activity { get; set; }
        public DbSet<Models.ActivityType> ActivityType { get; set; }
        public DbSet<ActivityApply> ActivityApply { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
