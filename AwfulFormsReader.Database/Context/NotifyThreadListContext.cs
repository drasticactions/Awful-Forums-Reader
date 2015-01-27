using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsLibrary.Entity;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
namespace AwfulForumsReader.Database.Context
{
    public class NotifyThreadListContext : DbContext
    {
        public DbSet<ForumThreadEntity> NotifyThreads { get; set; }

        protected override void OnConfiguring(DbContextOptions builder)
        {
#if WINDOWS_PHONE_APP
            var connection = "Filename=NotifyList.db";
#else
            var dir = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            var connection = "Filename=" + System.IO.Path.Combine(dir, "NotifyList.db");
#endif
            builder.UseSQLite(connection);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ForumThreadEntity>(b => b.Key(thread => thread.ThreadId));
        }
    }
}
