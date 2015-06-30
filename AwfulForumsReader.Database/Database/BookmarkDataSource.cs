using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Windows.Storage;
using AwfulForumsLibrary.Entity;
using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.WinRT;

namespace AwfulForumsReader.Database
{
    public class BookmarkDataSource : IDisposable
    {
        private const string DBFILENAME = "Bookmark.db";
        protected StorageFolder UserFolder { get; set; }
        protected SQLiteAsyncConnection Db { get; set; }

        public Repository<ForumThreadEntity> BookmarkForumRepository { get; set; }


        public BookmarkDataSource()
        {
            UserFolder = ApplicationData.Current.LocalFolder;
            var dbPath = Path.Combine(UserFolder.Path, DBFILENAME);
            var connectionFactory = new Func<SQLiteConnectionWithLock>(() => new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: false)));
            Db = new SQLiteAsyncConnection(connectionFactory);

            BookmarkForumRepository = new Repository<ForumThreadEntity>(Db);
        }

        public void InitDatabase()
        {
            //Check to ensure db file exists
            try
            {
                //Try to read the database file
                UserFolder.GetFileAsync(DBFILENAME).GetAwaiter().GetResult();
            }
            catch
            {
                //Will throw an exception if not found
                UserFolder.CreateFileAsync(DBFILENAME).GetAwaiter().GetResult();
            }
        }

        public void CreateDatabase()
        {
            var existingTables =
                Db.QueryAsync<sqlite_master>("SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;")
                  .GetAwaiter()
                  .GetResult();

            if (existingTables.Any(x => x.name == "ForumEntity") != true)
            {
                Db.CreateTableAsync<ForumThreadEntity>().GetAwaiter().GetResult();
            }
        }

        public void Dispose()
        {
            this.Db = null;
        }
    }
}
