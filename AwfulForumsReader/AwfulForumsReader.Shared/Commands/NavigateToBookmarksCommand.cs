using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands
{
    public class NavigateToBookmarksCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var threadViewModel = Locator.ViewModels.BookmarksPageVm;
            App.RootFrame.Navigate(typeof(BookmarksPage));
            await threadViewModel.Initialize();
        }
    }
}
