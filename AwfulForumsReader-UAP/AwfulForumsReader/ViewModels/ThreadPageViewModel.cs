using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Commands.Bookmarks;
using AwfulForumsReader.Commands.Navigation;
using AwfulForumsReader.Commands.Posts;
using AwfulForumsReader.Commands.Threads;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class ThreadPageViewModel : NotifierBase
    {
        public ThreadPageViewModel()
        {
            ThreadNotSelected = true;
        }
        private ForumThreadEntity _forumThreadEntity;
        private AddOrRemoveBookmarkCommand _addOrRemoveBookmarkCommand = new AddOrRemoveBookmarkCommand();
        public AddToThreadTabs AddToTabs { get; set; } = new AddToThreadTabs();
        private bool _threadNotSelected;

        public bool ThreadNotSelected
        {
            get { return _threadNotSelected; }
            set
            {
                SetProperty(ref _threadNotSelected, value);
                OnPropertyChanged();
            }
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

        public ChangePageThreadCommand ChangePageThreadCommand { get; set; } = new ChangePageThreadCommand();

        private string _pageSelection;

        public string PageSelection
        {
            get { return _pageSelection; }
            set
            {
                SetProperty(ref _pageSelection, value);
                OnPropertyChanged();
            }
        }

        public int QuoteId { get; set; }
        public RefreshThreadPageCommand RefreshThreadPageCommand { get; set; } = new RefreshThreadPageCommand();

        public BackThreadPageCommand BackThreadPageCommand { get; set; } = new BackThreadPageCommand();

        public void WebView_OnNavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            var command = new ThreadDomContentLoadedCommand();
            command.Execute(sender);
        }

        public ForwardThreadPageCommand ForwardThreadPageCommand { get; set; } = new ForwardThreadPageCommand();

        public ScrollToBottomThreadPageCommand ScrollToBottomThreadPageCommand { get; set; } = new ScrollToBottomThreadPageCommand();

        public NavigateToNewReplyCommand NavigateToNewReplyCommand { get; set; } = new NavigateToNewReplyCommand();

        public RemoveTabCommand RemoveTabCommand { get; set; } = new RemoveTabCommand();

        public ChangeTabsCommand ChangeTabsCommand { get; set; } = new ChangeTabsCommand();

        public ThreadDomContentLoadedCommand ThreadDomContentLoadedCommand { get; set; } = new ThreadDomContentLoadedCommand();

        private ObservableCollection<ForumThreadEntity> _linkedThreads = new ObservableCollection<ForumThreadEntity>(); 

        private bool _isLoading;

        private string _html;

        private IEnumerable<int> _pageNumbers;

        public IEnumerable<int> PageNumbers
        {
            get { return _pageNumbers; }
            set
            {
                SetProperty(ref _pageNumbers, value);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ForumThreadEntity> LinkedThreads
        {
            get { return _linkedThreads; }
            set
            {
                SetProperty(ref _linkedThreads, value);
                OnPropertyChanged();
            }
        }

        public ForumThreadEntity ForumThreadEntity
        {
            get { return _forumThreadEntity; }
            set
            {
                SetProperty(ref _forumThreadEntity, value);
                OnPropertyChanged();
            }
        }

        public string Html
        {
            get { return _html; }
            set
            {
                SetProperty(ref _html, value);
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

        public async Task GetForumPostsAsync()
        {
            if (ForumThreadEntity == null)
            {
                AwfulDebugger.SendMessageDialogAsync("Something went wrong...",
                    new Exception("ForumThreadEntity is null."));
                return;
            }
            ThreadNotSelected = false;
IsLoading = true;
            bool isSuccess;
            string errorMessage = string.Empty;
            var postManager = new PostManager();
            var postList = new List<ForumPostEntity>();
            try
            {
                postList = await postManager.GetThreadPostsAsync(ForumThreadEntity);
                isSuccess = true;
            }
            catch (Exception ex)
            {
                IsLoading = false;
                isSuccess = false;
                errorMessage = ex.Message;
            }
            if (!isSuccess)
            {
                await AwfulDebugger.SendMessageDialogAsync("Failed to get thread posts.", new Exception(errorMessage));
                return;
            }

            var count = postList.Count(node => !node.HasSeen);
            if (ForumThreadEntity.RepliesSinceLastOpened > 0)
            {
                ForumThreadEntity.RepliesSinceLastOpened -= count;
            }
#if WINDOWS_PHONE_APP
            ForumThreadEntity.PlatformIdentifier = PlatformIdentifier.WindowsPhone;
#else
            ForumThreadEntity.PlatformIdentifier = PlatformIdentifier.Windows8;
#endif
            try
            {
                GetDarkModeSetting(ForumThreadEntity);
                Html = await HtmlFormater.FormatThreadHtml(ForumThreadEntity, postList);
                ForumThreadEntity = ForumThreadEntity;
                PageNumbers = Enumerable.Range(1, ForumThreadEntity.TotalPages).ToArray();
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("An error occured creating the thread HTML", ex);
            }
            IsLoading = false;
        }

        private void GetDarkModeSetting(ForumThreadEntity forumThreadEntity)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (!localSettings.Values.ContainsKey(Constants.DarkMode)) return;
            var darkMode = (bool) localSettings.Values[Constants.DarkMode];
            switch (darkMode)
            {
                case true:
                    forumThreadEntity.PlatformIdentifier = PlatformIdentifier.WindowsPhone;
                    break;
                case false:
                    forumThreadEntity.PlatformIdentifier = PlatformIdentifier.Windows8;
                    break;
            }
        }
    }
}
