using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands.Navigation
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
