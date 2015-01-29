using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsLibrary.Entity;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;

namespace AwfulForumsReader.Database.Context
{
    public class LinkedSubforumContext : DbContext
    {
        public DbSet<ForumEntity> LinkedForums { get; set; }

        protected override void OnConfiguring(DbContextOptions builder)
        {
#if WINDOWS_PHONE_APP
            var connection = "Filename=LinkedForum.db";
#else
            var dir = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            var connection = "Filename=" + System.IO.Path.Combine(dir, "LinkedForum.db");
#endif
            builder.UseSQLite(connection);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ForumEntity>(b => b.Key(thread => thread.ForumId));
        }
    }
}
