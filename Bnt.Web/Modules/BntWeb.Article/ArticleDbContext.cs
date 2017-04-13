using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using BntWeb.Article.Models;
using BntWeb.Data;

namespace BntWeb.Article
{
    public class ArticleDbContext : BaseDbContext
    {
        public DbSet<Models.Article> Article { get; set; }

        public DbSet<Models.ArticleCategories> ArticleCategories { get; set; }

     

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}