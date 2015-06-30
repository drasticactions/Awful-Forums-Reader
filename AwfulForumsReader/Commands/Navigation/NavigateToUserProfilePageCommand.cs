using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands.Navigation
{
    public class NavigateToUserProfilePageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var value = (long) parameter;
            Locator.ViewModels.UserPageVm.ForumUserEntity = null;
            App.RootFrame.Navigate(typeof (UserProfilePage));
            await Locator.ViewModels.UserPageVm.Initialize(value);
        }
    }
}
