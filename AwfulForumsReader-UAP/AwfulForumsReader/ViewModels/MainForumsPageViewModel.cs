using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Common;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class MainForumsPageViewModel : NotifierBase
    {
        public MainForumsPageViewModel()
        {
            if (ForumGroupList != null) return;
            ForumGroupList = new ObservableCollection<ForumCategoryEntity>();
            Initialize();
        }
        public NavigateToThreadListPageCommand NavigateToThreadListPageCommand { get; set; } = new NavigateToThreadListPageCommand();
        private readonly ForumManager _forumManager = new ForumManager();
        private ObservableCollection<ForumCategoryEntity> _forumGroupList;
        public ObservableCollection<ForumCategoryEntity> ForumGroupList
        {
            get { return _forumGroupList; }
            set
            {
                SetProperty(ref _forumGroupList, value);
                OnPropertyChanged();
            }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);
                OnPropertyChanged();
            }
        }

        private async Task GetMainPageForumsAsync()
        {
            var forumCategoryEntities = await _forumManager.GetForumCategoryMainPage();
            foreach (var forumCategoryEntity in forumCategoryEntities)
            {
                ForumGroupList.Add(forumCategoryEntity);
            }
        }

        public async void Initialize()
        {
            IsLoading = true;
            try
            {
                //await GetFavoriteForums();
                await GetMainPageForumsAsync();
                OnPropertyChanged("ForumGroupList");
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Error getting the main forums dialog", ex);
            }
            IsLoading = false;
        }
    }
}
