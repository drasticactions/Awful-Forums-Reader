using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands
{
    public class NavigateToBookmarksCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            App.RootFrame.Navigate(typeof(BookmarksPage), parameter);
            await Locator.ViewModels.BookmarksPageVm.Initialize();
        }
    }
}
