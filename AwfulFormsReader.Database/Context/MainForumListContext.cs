using System.Resources;
using AwfulForumsLibrary.Entity;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;


namespace AwfulForumsReader.Database.Context
{
    public class MainForumListContext : DbContext
    {


        public DbSet<ForumCategoryEntity> ForumCategories { get; set; }
        public DbSet<ForumEntity> Forums { get; set; }
        public DbSet<ForumThreadEntity> BookmarkThreads { get; set; }
        protected override void OnConfiguring(DbContextOptions builder)
        {
#if WINDOWS_PHONE_APP
            var connection = "Filename=MainForumList.db";
#else
            var dir = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            var connection = "Filename=" + System.IO.Path.Combine(dir, "MainForumList.db");
#endif
            builder.UseSQLite(connection);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ForumThreadEntity>(b =>
            {
                b.Key(thread => thread.Id);
            });
            builder.Model.GetEntityType(typeof(ForumThreadEntity)).GetProperty("Id").GenerateValueOnAdd = true;

            builder.Entity<ForumEntity>(b =>
            {
                b.Key(forum => forum.ForumId);
                b.ManyToOne(forum => forum.ParentForum).ForeignKey(forum => forum.ParentForumId);
            });

            builder.Entity<ForumCategoryEntity>(b =>
            {
                b.Key(category => category.CategoryId);
                b.OneToMany(category => category.ForumList, forum => forum.ForumCategory).ForeignKey(forum => forum.CategoryId);
            });
        }
    }
}
