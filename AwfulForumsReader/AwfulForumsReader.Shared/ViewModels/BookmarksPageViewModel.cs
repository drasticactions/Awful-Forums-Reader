using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Automation.Peers;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Common;
using AwfulForumsReader.Context;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Core.Manager;
using AwfulForumsReader.Core.Tools;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class BookmarksPageViewModel : NotifierBase
    {
        private ObservableCollection<ForumThreadEntity> _bookmarkedThreads;
        private bool _isLoading;
        private AddOrRemoveBookmarkCommand _addOrRemoveBookmarkCommand = new AddOrRemoveBookmarkCommand();
        private UnreadThreadCommand _unreadThreadCommand = new UnreadThreadCommand();
        private LastPageCommand _lastPageCommand = new LastPageCommand();
        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
        private RefreshBookmarkListCommand _refreshBookmarkListCommand = new RefreshBookmarkListCommand();
        private NavigateToMainForumPageCommand _navigateToMainForumPageCommand = new NavigateToMainForumPageCommand();

        public NavigateToMainForumPageCommand NavigateToMainForumPageCommand
        {
            get { return _navigateToMainForumPageCommand; }
            set { _navigateToMainForumPageCommand = value; }
        }

        public RefreshBookmarkListCommand RefreshBookmarkListCommand
        {
            get { return _refreshBookmarkListCommand; }
            set { _refreshBookmarkListCommand = value; }
        }

        public AddOrRemoveBookmarkCommand AddOrRemoveBookmark
        {
            get { return _addOrRemoveBookmarkCommand; }
            set
            {
                SetProperty(ref _addOrRemoveBookmarkCommand, value);
                OnPropertyChanged();
            }
        }

        public UnreadThreadCommand UnreadThread
        {
            get { return _unreadThreadCommand; }
            set
            {
                SetProperty(ref _unreadThreadCommand, value);
                OnPropertyChanged();
            }
        }

        public LastPageCommand LastPage
        {
            get { return _lastPageCommand; }
            set
            {
                SetProperty(ref _lastPageCommand, value);
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ForumThreadEntity> BookmarkedThreads
        {
            get { return _bookmarkedThreads; }
            set
            {
                SetProperty(ref _bookmarkedThreads, value);
                OnPropertyChanged();
            }
        }

        private async Task<List<ForumThreadEntity>>  GetBookmarkedThreadsAsync()
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

        public async Task Initialize()
        {
            IsLoading = true;
            BookmarkedThreads = new ObservableCollection<ForumThreadEntity>();
            DateTime refreshDate = DateTime.UtcNow;
            if (_localSettings.Values.ContainsKey("RefreshBookmarks"))
            {
                var dateString = (string)_localSettings.Values["RefreshBookmarks"];
                refreshDate = DateTime.Parse(dateString);
            }
            using (var db = new MainForumListContext())
            {
                BookmarkedThreads = db.BookmarkThreads.ToObservableCollection();
            }
            if (!BookmarkedThreads.Any() || refreshDate < (DateTime.UtcNow.AddHours(-1.00)))
            {
                await Refresh();
            }
            IsLoading = false;
        }

        public async Task Refresh()
        {
            IsLoading = true;
            List<ForumThreadEntity> updatedBookmarkList;
            try
            {
                updatedBookmarkList = await GetBookmarkedThreadsAsync();
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Could not get bookmarks", ex);
                return;
            }

            BookmarkedThreads = updatedBookmarkList.ToObservableCollection();

            using (var db = new MainForumListContext())
            {
                var all = from c in db.BookmarkThreads select c;
                db.BookmarkThreads.RemoveRange(all);
                await db.SaveChangesAsync();
            }
            foreach (ForumThreadEntity t in BookmarkedThreads)
            {
                using (var db = new MainForumListContext())
                {

                    await db.BookmarkThreads.AddAsync(t);

                    await db.SaveChangesAsync();
                }
            }
            _localSettings.Values["RefreshBookmarks"] = DateTime.UtcNow.ToString();
            IsLoading = false;
        }

        public void UpdateThreadList()
        {
            OnPropertyChanged("BookmarkedThreads");
        }
    }
}
