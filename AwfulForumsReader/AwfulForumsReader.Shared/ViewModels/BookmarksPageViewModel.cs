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
using AwfulForumsReader.Database.Context;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Database.Commands;
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

        public NavigateToLastPageInThreadPageCommand NavigateToLastPageInThreadPageCommand { get; set; } = new NavigateToLastPageInThreadPageCommand();

        public NavigateToThreadPageCommand NavigateToThreadPageCommand { get; set; } = new NavigateToThreadPageCommand();


        public AddOrRemoveThreadToNotificationTableCommand AddThreadToNotificationTableCommand { get; set; } = new AddOrRemoveThreadToNotificationTableCommand();

        public NavigateToMainForumPageCommand NavigateToMainForumPageCommand { get; set; } = new NavigateToMainForumPageCommand();

        public RefreshBookmarkListCommand RefreshBookmarkListCommand { get; set; } = new RefreshBookmarkListCommand();

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
            var hasNoItems = false;
            while (!hasNoItems)
            {
                var bookmarks = await threadManager.GetBookmarksAsync(forum, pageNumber);
                bookmarkThreads.AddRange(bookmarks);
                if (bookmarks.Any())
                {
                    pageNumber++;
                }
                else
                {
                    hasNoItems = true;
                }
            }

            if (!_localSettings.Values.ContainsKey(Constants.BookmarkDefault)) return bookmarkThreads;
            var sorting = (BookmarkSorting)_localSettings.Values[Constants.BookmarkDefault];
            if (sorting != BookmarkSorting.MostUnreadOnTop) return bookmarkThreads;
            var newBookmarks =bookmarkThreads.OrderByDescending(node => node.RepliesSinceLastOpened);
            return newBookmarks.ToList();
        }

        public bool AutoRefresh { get; set; }

        public async Task Initialize()
        {
            if (BookmarkedThreads != null && BookmarkedThreads.Any())
            {
                return;
            }
            IsLoading = true;
            try
            {
                BookmarkedThreads = new ObservableCollection<ForumThreadEntity>();
                DateTime refreshDate = DateTime.UtcNow;
                if (_localSettings.Values.ContainsKey(Constants.AutoRefresh))
                {
                    AutoRefresh = (bool)_localSettings.Values[Constants.AutoRefresh];
                }
                if (_localSettings.Values.ContainsKey("RefreshBookmarks"))
                {
                    var dateString = (string)_localSettings.Values["RefreshBookmarks"];
                    refreshDate = DateTime.Parse(dateString);
                }
                var bookmarks = await _bookmarkManager.GetBookmarkedThreadsFromDb();
                if (bookmarks != null && bookmarks.Any())
                {
                    BookmarkedThreads = bookmarks.ToObservableCollection();
                }
                if ((!BookmarkedThreads.Any() || refreshDate < (DateTime.UtcNow.AddHours(-1.00))) || AutoRefresh)
                {
                    await Refresh();
                }
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Failed to get Bookmarks (Potential EF Issue!)", ex);
            }
            IsLoading = false;
        }

        private readonly BookmarkManager _bookmarkManager = new BookmarkManager();

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
            await _bookmarkManager.RemoveBookmarkThreads();
            await _bookmarkManager.AddBookmarkThreads(BookmarkedThreads.ToList());
            _localSettings.Values["RefreshBookmarks"] = DateTime.UtcNow.ToString();
            IsLoading = false;
        }

        public void UpdateThreadList()
        {
            OnPropertyChanged("BookmarkedThreads");
        }
    }
}
