using System.Linq;
using Windows.UI.Xaml.Controls;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Common;

namespace AwfulForumsReader.Commands.Navigation
{
    public class NavigateToSearchResultsCommand : AlwaysExecutableCommand
    {
        public override async void Execute(object parameter)
        {
            var gridView = (GridView) parameter;
            if (!gridView.SelectedItems.Any())
            {
                return;
            }
            if (string.IsNullOrEmpty(Locator.ViewModels.SearchPageVm.SearchTerms))
            {
                return;
            }
            var list = gridView.SelectedItems.Cast<ForumEntity>();
            await Locator.ViewModels.SearchPageVm.GetSearchResults(list.Select(node => node.ForumId).ToList());
        }
    }

    public class NavigateToSearchResultsFromListViewCommand : AlwaysExecutableCommand
    {
        public override async void Execute(object parameter)
        {
            var gridView = (ListView)parameter;
            if (!gridView.SelectedItems.Any())
            {
                return;
            }
            if (string.IsNullOrEmpty(Locator.ViewModels.SearchPageVm.SearchTerms))
            {
                return;
            }
            var list = gridView.SelectedItems.Cast<ForumEntity>();
            await Locator.ViewModels.SearchPageVm.GetSearchResults(list.Select(node => node.ForumId).ToList());
        }
    }
}
