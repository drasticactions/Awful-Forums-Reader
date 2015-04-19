using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands
{
    public class NavigateToSearchPageCommand : AlwaysExecutableCommand
    {
        public override async void Execute(object parameter)
        {
            App.RootFrame.Navigate(typeof (SearchPage));
            await Locator.ViewModels.SearchPageVm.Initialize();
        }
    }
}
