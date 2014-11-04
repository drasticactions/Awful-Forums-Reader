using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Manager;
using AwfulForumsReader.Core.Tools;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands
{
    public class LogoutCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var message = string.Format("Are you sure you want to logout?{0}I mean, I could care less. It's up to you...{0}{1}", Environment.NewLine, Constants.Ascii4);
            var msgBox =
                new MessageDialog(message,
                    "Make all your wildest dreams come true...");
            var okButton = new UICommand("WHY ARE YOU ASKING YES YES YES!") { Invoked = OkButtonClick };
            var cancelButton = new UICommand("No, I like pain and misery.") { Invoked = CancelButtonClick };
            msgBox.Commands.Add(okButton);
            msgBox.Commands.Add(cancelButton);
            msgBox.ShowAsync();
        }

        private void CancelButtonClick(IUICommand command)
        {
            return;
        }

        private async void OkButtonClick(IUICommand command)
        {
            var authenticationManager = new AuthenticationManager();
            var result = await authenticationManager.Logout();
            var localSettings = ApplicationData.Current.LocalSettings;

            if (localSettings.Values.ContainsKey(Constants.BookmarkBackground))
            {
                //BackgroundTaskUtils.UnregisterBackgroundTasks(BackgroundTaskUtils.BackgroundTaskName);
                localSettings.Values[Constants.BookmarkBackground] = false;
            }

            if (result)
            {
                App.RootFrame.Navigate(typeof(LoginPage));
            }
            else
            {
                var msgBox =
                new MessageDialog("Could not log you out! You're stuck here forever! HA HA HA!",
                    "Logout error");
                msgBox.ShowAsync();
            }
        }
    }
}
