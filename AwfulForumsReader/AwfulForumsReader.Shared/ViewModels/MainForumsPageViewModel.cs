using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Database;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class MainForumsPageViewModel : NotifierBase
    {
        private readonly ForumManager _forumManager = new ForumManager();
        private MainForumsDatabase _db = new MainForumsDatabase();
        private ForumCategoryEntity _favoritesEntity;
        private ObservableCollection<ForumCategoryEntity> _forumGroupList;
        private ObservableCollection<ForumCategoryEntity> _favoriteForumGroupList;
        private bool _isLoading;
        public NavigateToSearchPageCommand NavigateToSearchPageCommand { get; set; } = new NavigateToSearchPageCommand();
        public NavigateToUserProfilePageCommand NavigateToUserProfilePageCommand { get; set; } = new NavigateToUserProfilePageCommand();
        public NavigateToPrivateMessageListPageCommand NavigateToPrivateMessageListPageCommand { get; set; } = new NavigateToPrivateMessageListPageCommand();

        public LogoutCommand LogoutCommand { get; set; } = new LogoutCommand();
        public NavigateToSaclopedia NavigateToSaclopedia { get; set; } = new NavigateToSaclopedia();
        public NavigateToSettingsCommand NavigateToSettingsCommand { get; set; } = new NavigateToSettingsCommand();

        public MainForumsPageViewModel()
        {
            if (ForumGroupList != null) return;
            ForumGroupList = new ObservableCollection<ForumCategoryEntity>();
            Initialize();
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

        public ObservableCollection<ForumCategoryEntity> ForumGroupList
        {
            get { return _forumGroupList; }
            set
            {
                SetProperty(ref _forumGroupList, value);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ForumCategoryEntity> FavoriteForumGroupList
        {
            get { return _favoriteForumGroupList; }
            set
            {
                SetProperty(ref _favoriteForumGroupList, value);
                OnPropertyChanged();
            }
        }

        private NavigateToBookmarksCommand _navigateToBookmarksCommand = new NavigateToBookmarksCommand();

        public NavigateToBookmarksCommand NavigateToBookmarks
        {
            get { return _navigateToBookmarksCommand; }
            set { SetProperty(ref _navigateToBookmarksCommand, value); }
        }

        private NavigateToThreadListPageCommand _navigateToThreadListPageCommand = new NavigateToThreadListPageCommand();

        public NavigateToThreadListPageCommand NavigateToThreadListPageCommand
        {
            get { return _navigateToThreadListPageCommand; }
            set { SetProperty(ref _navigateToThreadListPageCommand, value); }
        }

        private AddOrRemoveFavoriteCommand _addAsFavoriteCommand = new AddOrRemoveFavoriteCommand();

        public AddOrRemoveFavoriteCommand AddAsFavorite
        {
            get { return _addAsFavoriteCommand; }
            set { SetProperty(ref _addAsFavoriteCommand, value); }
        }

        public void SetFavoriteForums(ObservableCollection<ForumCategoryEntity> favoriteList)
        {
            FavoriteForumGroupList = favoriteList;
        }

        public async Task GetFavoriteForums()
        {
            var forumEntities = await _db.GetFavoriteForumsAsync();
            var favorites = ForumGroupList.FirstOrDefault(node => node.Name.Equals("Favorites"));
            if (!forumEntities.Any())
            {
                if (favorites != null)
                {
                    ForumGroupList.Remove(favorites);
                }
                OnPropertyChanged("ForumGroupList");
                return;
            }

            _favoritesEntity = new ForumCategoryEntity
            {
                Name = "Favorites",
                Location = string.Format(Constants.ForumPage, "forumid=48"),
                ForumList = forumEntities
            };

            if (favorites == null)
            {
                ForumGroupList.Insert(0, _favoritesEntity);
            }
            else
            {
                ForumGroupList.RemoveAt(0);
                ForumGroupList.Insert(0, _favoritesEntity);
            }
            OnPropertyChanged("ForumGroupList");
        }

        private async Task GetMainPageForumsAsync()
        {
            var forumCategoryEntities = await _db.GetMainForumsList();
            if (forumCategoryEntities.Any())
            {
                foreach (var forumCategoryEntity in forumCategoryEntities)
                {
                    ForumGroupList.Add(forumCategoryEntity);
                }
                return;
            }

            forumCategoryEntities = await _forumManager.GetForumCategoryMainPage();
            foreach (var forumCategoryEntity in forumCategoryEntities)
            {
                ForumGroupList.Add(forumCategoryEntity);
            }
            await _db.SaveMainForumsList(ForumGroupList.ToList());
        }

        internal async void Initialize()
        {
            IsLoading = true;
            try
            {
                await GetFavoriteForums();
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
