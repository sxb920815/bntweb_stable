/* 
    ======================================================================== 
        File name：		ActivityDbContext
        Module:			
        Author：		Daniel.Wu（wujb）
        Create Time：		2016/6/22 10:23:23
        Modify By:        
        Modify Date:    
    ======================================================================== 
*/

using System.Data.Entity;
using BntWeb.Data;


namespace BntWeb.Merchant
{
    public class MerchantDbContext : BaseDbContext
    {
        public DbSet<Models.Merchant> Merchants { get; set; }
        public DbSet<Models.MerchantType> MerchantTypes { get; set; }
        public DbSet<Models.MerchantTypeRalation> MerchantTypeRalations { get; set; }
        public DbSet<Models.MerchantProduct> MerchantProducts { get; set; }

        public DbSet<Models.MerchantActivity> MerchantActivitys { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}