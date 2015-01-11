using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands
{
    public class NavigateToUserProfilePageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            await Locator.ViewModels.UserPageVm.Initialize(149831);
            App.RootFrame.Navigate(typeof (UserProfilePage));
        }
    }
}
