using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands.Navigation
{
    public class NavigateToPrivateMessageListPageCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            Locator.ViewModels.PrivateMessageVm.GetPrivateMessages();
            App.RootFrame.Navigate(typeof (PrivateMessageListPage));
        }
    }
}
