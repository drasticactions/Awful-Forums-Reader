using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Commands.Bookmarks;
using AwfulForumsReader.Commands.Navigation;
using AwfulForumsReader.Commands.Threads;
using AwfulForumsReader.Common;
using AwfulForumsReader.Database;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class BookmarksPageViewModel : NotifierBase
    {
        private ObservableCollection<ForumThreadEntity> _bookmarkedThreads;
        private bool _isLoading;
        private AddOrRemoveBookmarkCommand _addOrRemoveBookmarkCommand = new AddOrRemoveBookmarkCommand();
        public AddThreadToNotificationTable AddThreadToNotificationTableCommand { get; set; } = new AddThreadToNotificationTable();

        public NavigateToBookmarkPageViaToastCommand NavigateToThreadPageViaToastCommand { get; set; } =
            new NavigateToBookmarkPageViaToastCommand();
        private UnreadThreadCommand _unreadThreadCommand = new UnreadThreadCommand();
        private LastPageCommand _lastPageCommand = new LastPageCommand();
        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

        public NavigateToLastPageInThreadPageCommand NavigateToLastPageInThreadPageCommand { get; set; } = new NavigateToLastPageInThreadPageCommand();

        public NavigateToThreadPageCommand NavigateToThreadPageCommand { get; set; } = new NavigateToThreadPageCommand();

        public RefreshBookmarkListCommand RefreshBookmarkListCommand { get; set; } = new RefreshBookmarkListCommand();

        public AddToThreadTabs AddToTabs { get; set; } = new AddToThreadTabs();

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
                AwfulDebugger.SendMessageDialogAsync("Failed to get Bookmarks", ex);
            }
            IsLoading = false;
        }

        private readonly MainForumsDatabase _bookmarkManager = new MainForumsDatabase();

        public async Task Refresh()
        {
            IsLoading = true;
            try
            {
                var test = await _bookmarkManager.RefreshBookmarkedThreads();
                BookmarkedThreads = test.ToObservableCollection();
                _localSettings.Values["RefreshBookmarks"] = DateTime.UtcNow.ToString();
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Failed to get Bookmarks", ex);
            }
            IsLoading = false;
        }

        public void UpdateThreadList()
        {
            OnPropertyChanged("BookmarkedThreads");
        }
    }
}
