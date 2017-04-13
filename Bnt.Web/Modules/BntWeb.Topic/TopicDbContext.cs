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


namespace BntWeb.Topic
{
    public class TopicDbContext : BaseDbContext
    {
        public DbSet<Models.Topic> Topics { get; set; }
        public DbSet<Models.TopicType> TopicTypes { get; set; }
        public DbSet<Comment.Models.Comment> Comments { set; get; }
        public DbSet<ContentMarkup.Models.Markup> MarkUps { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}