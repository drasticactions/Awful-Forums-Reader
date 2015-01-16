using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Core.Manager;
using AwfulForumsReader.Core.Tools;
using AwfulForumsReader.Database.Context;
using Remotion.Linq.Parsing;

namespace AwfulForumsReader.Database.Commands
{
    public class BookmarkManager
    {
        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

        public async Task<List<ForumThreadEntity>> RefreshBookmarkedThreads()
        {
            List<ForumThreadEntity> updatedBookmarkList;
            try
            {
                updatedBookmarkList = await GetBookmarkedThreadsAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not get bookmarks", ex);
            }

            using (var db = new MainForumListContext())
            {
                var all = from c in db.BookmarkThreads select c;
                db.BookmarkThreads.RemoveRange(all);
                await db.SaveChangesAsync();
            }
            foreach (ForumThreadEntity t in updatedBookmarkList)
            {
                using (var db = new MainForumListContext())
                {

                    await db.BookmarkThreads.AddAsync(t);

                    await db.SaveChangesAsync();
                }
            }
            _localSettings.Values["RefreshBookmarks"] = DateTime.UtcNow.ToString();

            return updatedBookmarkList;
        }

        public async Task<ForumThreadEntity> GetBookmarkThreadAsync(long threadId)
        {
            using (var db = new MainForumListContext())
            {
               return await db.BookmarkThreads.FirstOrDefaultAsync(node => node.ThreadId == threadId);
            }
        }

        public bool IsBookmark(long threadId)
        {
            using (var db = new MainForumListContext())
            {
                return db.BookmarkThreads.Any(node => node.ThreadId == threadId);
            }
        }

        public Task<List<ForumThreadEntity>> GetBookmarkedThreadsFromDb()
        {
            using (var db = new MainForumListContext())
            {
                return db.BookmarkThreads.ToListAsync();
            }
        }
        private async Task<List<ForumThreadEntity>> GetBookmarkedThreadsAsync()
        {
            var bookmarkThreads = new List<ForumThreadEntity>();
            var threadManager = new ThreadManager();
            var forum = new ForumEntity()
            {
                Name = "Bookmarks",
                IsBookmarks = true,
                IsSubforum = false,
                Location = Constants.UserCp
            };
            var pageNumber = 1;
            var hasItems = false;
            while (!hasItems)
            {
                var bookmarks = await threadManager.GetBookmarksAsync(forum, pageNumber);
                bookmarkThreads.AddRange(bookmarks);
                if (bookmarks.Any())
                {
                    hasItems = true;
                }
                else
                {
                    pageNumber++;
                }
            }
            return bookmarkThreads;
        }

        public async Task AddBookmarkThreads(List<ForumThreadEntity> bookmarkedThreads)
        {
            foreach (ForumThreadEntity t in bookmarkedThreads)
            {
                using (var db = new MainForumListContext())
                {

                    await db.BookmarkThreads.AddAsync(t);

                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task RemoveBookmarkThreads()
        {
            using (var db = new MainForumListContext())
            {
                var all = from c in db.BookmarkThreads select c;
                db.BookmarkThreads.RemoveRange(all);
                await db.SaveChangesAsync();
            }
        }
    }
}
