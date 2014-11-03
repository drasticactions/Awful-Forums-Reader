using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Core.Manager;
using AwfulForumsReader.Core.Tools;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class ThreadPageViewModel : NotifierBase
    {
        private ForumThreadEntity _forumThreadEntity;

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

        public async Task GetForumPostsAsync(ForumThreadEntity forumThreadEntity)
        {
IsLoading = true;
            bool isSuccess;
            string errorMessage = string.Empty;
            var postManager = new PostManager();
            var postList = new List<ForumPostEntity>();
            try
            {
                postList = await postManager.GetThreadPostsAsync(forumThreadEntity);
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
#if WINDOWS_PHONE_APP
            forumThreadEntity.PlatformIdentifier = PlatformIdentifier.WindowsPhone;
#else
            forumThreadEntity.PlatformIdentifier = PlatformIdentifier.Windows8;
#endif
            try
            {
                GetDarkModeSetting(forumThreadEntity);
                Html = await HtmlFormater.FormatThreadHtml(forumThreadEntity, postList);
                ForumThreadEntity = forumThreadEntity;
                PageNumbers = Enumerable.Range(1, forumThreadEntity.TotalPages).ToArray();
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("An error occured creating the thread HTML", ex);
            }
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
