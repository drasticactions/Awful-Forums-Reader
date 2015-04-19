using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands
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
