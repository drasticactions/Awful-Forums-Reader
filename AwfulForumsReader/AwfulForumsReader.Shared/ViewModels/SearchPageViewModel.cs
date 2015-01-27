using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
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
        private List<ForumCategoryEntity> _forumCategoryEntities;
        public NavigateToThreadPageViaSearchResult NavigateToThreadPageViaSearchResult { get; set; } = new NavigateToThreadPageViaSearchResult();
        public NavigateToSearchResultsCommand NavigateToSearchResultsCommand { get; set; } = new NavigateToSearchResultsCommand();
        public NavigateToSearchResultsFromListViewCommand NavigateToSearchResultsFromListViewCommand { get; set; } = new NavigateToSearchResultsFromListViewCommand();
        private SearchPageScrollingCollection _searchPageScrollingCollection;
        public SearchPageScrollingCollection SearchPageScrollingCollection
        {
            get { return _searchPageScrollingCollection; }
            set
            {
                SetProperty(ref _searchPageScrollingCollection, value);
                OnPropertyChanged();
            }
        }

        public List<ForumCategoryEntity> ForumCategoryEntities
        {
            get { return _forumCategoryEntities; }
            set
            {
                SetProperty(ref _forumCategoryEntities, value);
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
                SearchPageScrollingCollection = new SearchPageScrollingCollection(forumIds, SearchTerms);
                App.RootFrame.Navigate(typeof(SearchResultsPage));
            }
            catch (Exception ex)
            {

                AwfulDebugger.SendMessageDialogAsync("Failed to get results", ex);
            }
            IsLoading = false;
        }

    }
}
