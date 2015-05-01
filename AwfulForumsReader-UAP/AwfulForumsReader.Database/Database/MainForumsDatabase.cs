using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsLibrary.Tools;

namespace AwfulForumsReader.Database
{
    public class MainForumsDatabase
    {
        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

        public async Task<List<ForumCategoryEntity>> GetMainForumsList()
        {
            var ds = new DataSource();

            var list = new List<ForumCategoryEntity>();
            var dbForumsCategories = await ds.ForumCategoryRepository.GetAllWithChildren();
            if (!dbForumsCategories.Any()) return list;
            var result = dbForumsCategories.OrderBy(node => node.Order);
            foreach (var forumCategoryEntity in result)
            {
                var testForumList = new List<ForumEntity>();
                foreach (var forum in forumCategoryEntity.ForumList.Where(node => node.ParentForum == null))
                {
                    testForumList.Add(forum);
                    ForumEntity forum1 = forum;
                    testForumList.AddRange(forumCategoryEntity.ForumList.Where(node => node.ParentForum == forum1));
                }
                forumCategoryEntity.ForumList = testForumList;
                list.Add(forumCategoryEntity);
            }
            return list;

        }

        public async Task SaveMainForumsList(List<ForumCategoryEntity> forumGroupList)
        {
            var ds = new DataSource();
            var items = await ds.ForumCategoryRepository.Items.ToListAsync();
            if (items.Any())
            {
                foreach (var item in forumGroupList)
                {
                    await ds.ForumCategoryRepository.UpdateWithChildren(item);
                }
                return;
            }

            foreach (var item in forumGroupList)
            {
                await ds.ForumCategoryRepository.CreateWithChildren(item);
            }
        }

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

            await RemoveBookmarkThreads();
            var dbs = new BookmarkDataSource();
            foreach (ForumThreadEntity t in updatedBookmarkList)
            {
                dbs.BookmarkForumRepository.CreateWithChildren(t);
            }
            _localSettings.Values["RefreshBookmarks"] = DateTime.UtcNow.ToString();

            return updatedBookmarkList;
        }

        public async Task<ForumThreadEntity> GetBookmarkThreadAsync(long threadId)
        {
            var bds = new BookmarkDataSource();
            return await bds.BookmarkForumRepository.Items.Where(node => node.ThreadId == threadId).FirstOrDefaultAsync();
        }

        public async Task<bool> IsBookmark(long threadId)
        {
            var bds = new BookmarkDataSource();
            var result = await bds.BookmarkForumRepository.Items.Where(node => node.ThreadId == threadId).ToListAsync();
            return result.Count > 0;
        }

        public async Task<List<ForumThreadEntity>> GetBookmarkedThreadsFromDb()
        {
            var bds = new BookmarkDataSource();
            return await bds.BookmarkForumRepository.Items.ToListAsync();
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

        public async Task<List<ForumEntity>> GetFavoriteForumsAsync()
        {
            var ds = new DataSource();

            var list = new List<ForumEntity>();
            var dbForumsCategories = await ds.ForumRepository.GetAllWithChildren();
            if (!dbForumsCategories.Any()) return list;
            list.AddRange(dbForumsCategories.Where(node => node.IsBookmarks));
            return list;
        }

        public async Task AddBookmarkThreads(List<ForumThreadEntity> bookmarkedThreads)
        {
            var dbs = new BookmarkDataSource();
            foreach (ForumThreadEntity t in bookmarkedThreads)
            {
                await dbs.BookmarkForumRepository.CreateWithChildren(t);
            }
        }

        public async Task AddThreadToTabListAsync(ForumThreadEntity thread)
        {
            var db = new DataSource();
            await db.TabRepository.Create(thread);
        }

        public async Task<List<ForumThreadEntity>> GetAllTabThreads()
        {
            var ds = new DataSource();
            return await ds.TabRepository.GetAllWithChildren();
        }

        public async Task RemoveAllThreadsFromTabList()
        {
            var db = new DataSource();
            await db.TabRepository.RemoveAll();
        }

        public async Task RemoveBookmarkThreads()
        {
            var dbs = new BookmarkDataSource();
            await dbs.BookmarkForumRepository.RemoveAll();
        }
    }
}
