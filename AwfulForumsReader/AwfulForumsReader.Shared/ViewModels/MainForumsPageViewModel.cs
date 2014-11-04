using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Common;
using AwfulForumsReader.Context;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Core.Manager;
using AwfulForumsReader.Core.Tools;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class MainForumsPageViewModel : NotifierBase
    {
        private readonly ForumManager _forumManager = new ForumManager();
        private ForumCategoryEntity _favoritesEntity;
        private ObservableCollection<ForumCategoryEntity> _forumGroupList;
        private ObservableCollection<ForumCategoryEntity> _favoriteForumGroupList;
        private bool _isLoading;
        private NavigateToSettingsCommand _navigateToSettingsCommand = new NavigateToSettingsCommand();
        private LogoutCommand _logoutCommand = new LogoutCommand();
        public LogoutCommand LogoutCommand
        {
            get { return _logoutCommand; }
            set { _logoutCommand = value; }
        }

        public NavigateToSettingsCommand NavigateToSettingsCommand
        {
            get { return _navigateToSettingsCommand; }
            set { _navigateToSettingsCommand = value; }
        }

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
            List<ForumEntity> forumEntities;
            using (var db = new MainForumListContext())
            {
                forumEntities = await db.Forums.Where(node => node.IsBookmarks).ToListAsync();
            }
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
            using (var db = new MainForumListContext())
            {
                db.Forums.ToList();
                var forumCategoryEntities = db.ForumCategories.ToList();
                if (forumCategoryEntities.Any())
                {
                    foreach (var forumCategoryEntity in forumCategoryEntities)
                    {
                        var testForumList = new List<ForumEntity>();
                        foreach (var forum in forumCategoryEntity.ForumList.Where(node => node.ParentForum == null))
                        {
                            testForumList.Add(forum);
                            ForumEntity forum1 = forum;
                            testForumList.AddRange(forumCategoryEntity.ForumList.Where(node => node.ParentForum == forum1));
                        }
                        forumCategoryEntity.ForumList = testForumList;
                        ForumGroupList.Add(forumCategoryEntity);
                    }
                    return;
                }
               
                forumCategoryEntities = await _forumManager.GetForumCategoryMainPage();
                foreach (var forumCategoryEntity in forumCategoryEntities)
                {
                    ForumGroupList.Add(forumCategoryEntity);
                }
                foreach (var forumGroup in ForumGroupList)
                {
                    foreach (var forumEntity in forumGroup.ForumList)
                    {
                        db.Add(forumEntity);
                    }
                    db.Add(forumGroup);
                }

                await db.SaveChangesAsync();
            }

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
