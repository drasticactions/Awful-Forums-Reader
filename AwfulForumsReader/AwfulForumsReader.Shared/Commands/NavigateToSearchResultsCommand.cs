using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;

namespace AwfulForumsReader.Commands
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
}
