using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands
{
    public class NavigateToSettingsCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
#if WINDOWS_APP
           App.ShowAppSettingsFlyout();
#endif
#if WINDOWS_PHONE_APP
            App.RootFrame.Navigate(typeof (SettingsPage));
#endif
        }
    }
}
