using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;
using SQLite.Net.Async;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AwfulForumsLibrary.Entity;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;

namespace AwfulForumsReader.Database
{
    public class DataSource : IDisposable
    {
        private const string DBFILENAME = "App.db";
        protected StorageFolder UserFolder { get; set; }
        protected SQLiteAsyncConnection Db { get; set; }

        public Repository<ForumCategoryEntity> ForumCategoryRepository { get; set; }

        public Repository<ForumEntity> ForumRepository { get; set; }

        public Repository<ForumThreadEntity> TabRepository { get; set; } 


        public DataSource()
        {
            UserFolder = ApplicationData.Current.LocalFolder;
#if WINDOWS_APP
            var dbPath = Path.Combine(UserFolder.Path, DBFILENAME);
#else
            var dbPath = "AppDB";
#endif
            var connectionFactory = new Func<SQLiteConnectionWithLock>(() => new SQLiteConnectionWithLock(new SQLitePlatformWinRT(), new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: false)));
            Db = new SQLiteAsyncConnection(connectionFactory);

            ForumCategoryRepository = new Repository<ForumCategoryEntity>(Db);

            ForumRepository = new Repository<ForumEntity>(Db);

            TabRepository = new Repository<ForumThreadEntity>(Db);
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
                Db.CreateTableAsync<ForumEntity>().GetAwaiter().GetResult();
            }

            if (existingTables.Any(x => x.name == "ForumCategoryEntity") != true)
            {
                Db.CreateTableAsync<ForumCategoryEntity>().GetAwaiter().GetResult();
            }

            if (existingTables.Any(x => x.name == "ForumThreadEntity") != true)
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
