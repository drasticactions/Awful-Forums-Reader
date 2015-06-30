using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Commands.Forums;
using AwfulForumsReader.Commands.Navigation;
using AwfulForumsReader.Commands.Threads;
using AwfulForumsReader.Common;
using AwfulForumsReader.Database;
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
        private MainForumsDatabase _db = new MainForumsDatabase();
        private ForumCategoryEntity _favoritesEntity;
        private ObservableCollection<ForumCategoryEntity> _favoriteForumGroupList;
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

        public ObservableCollection<ForumCategoryEntity> FavoriteForumGroupList
        {
            get { return _favoriteForumGroupList; }
            set
            {
                SetProperty(ref _favoriteForumGroupList, value);
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

        public CreateForumTileCommand CreateForumTile { get; set; } = new CreateForumTileCommand();

        public AddOrRemoveFavoriteCommand AddOrRemoveFavorite { get; set; } = new AddOrRemoveFavoriteCommand();

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
                // TODO: There is an "error" saving the main forums page the first time, but they do get saved correctly.
                // And the exception is not helpful at all... Figure out what's going on and fix it. :\
                //AwfulDebugger.SendMessageDialogAsync("Error getting the main forums dialog", ex);
            }
            IsLoading = false;
        }
    }
}
