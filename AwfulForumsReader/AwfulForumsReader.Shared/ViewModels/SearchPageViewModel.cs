using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Core.Manager;
using AwfulForumsReader.Database.Commands;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class SearchPageViewModel : NotifierBase
    {
        private bool _isLoading;
        private SearchManager _searchManager;
        private string _searchTerms;
        private List<SearchEntity> _searchResultsEntities; 
        private List<ForumCategoryEntity> _forumCategoryEntities;
        public NavigateToSearchResultsCommand NavigateToSearchResultsCommand { get; set; } = new NavigateToSearchResultsCommand();
        public List<ForumCategoryEntity> ForumCategoryEntities
        {
            get { return _forumCategoryEntities; }
            set
            {
                SetProperty(ref _forumCategoryEntities, value);
                OnPropertyChanged();
            }
        }
        public List<SearchEntity> SearchResultsEntities
        {
            get { return _searchResultsEntities; }
            set
            {
                SetProperty(ref _searchResultsEntities, value);
                OnPropertyChanged();
            }
        }
        public string SearchTerms
        {
            get { return _searchTerms; }
            set
            {
                SetProperty(ref _searchTerms, value);
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

        public void Initialize()
        {
            IsLoading = true;
            try
            {
                _searchManager = new SearchManager();
                var mainForumsManager = new MainForumsManager();
                ForumCategoryEntities = mainForumsManager.GetMainForumsList();
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Failed to get forums list", ex);
            }
            IsLoading = false;
        }

        public async Task GetSearchResults(List<int> forumIds)
        {
            IsLoading = true;
            try
            {
                SearchResultsEntities = await _searchManager.GetSearchQueryResults(forumIds, SearchTerms);
                if (SearchResultsEntities.Any())
                {
                    App.RootFrame.Navigate(typeof (SearchResultsPage));
                }
            }
            catch (Exception ex)
            {

                AwfulDebugger.SendMessageDialogAsync("Failed to get results", ex);
            }
            IsLoading = false;
        }

    }
}
