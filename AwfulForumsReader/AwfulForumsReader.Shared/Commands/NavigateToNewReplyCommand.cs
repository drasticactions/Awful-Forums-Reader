using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;
using AwfulForumsReader.ViewModels;

namespace AwfulForumsReader.Commands
{
    public class NavigateToNewReplyCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var args = parameter as RoutedEventArgs;
            if (args == null)
            {
                await AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", new Exception("Arguments are null"));
                return;
            }

            var appButton = args.OriginalSource as AppBarButton;
            if (appButton == null)
            {
                // TODO: If this is somehow null, throw an error and tell the user.
                return;
            }
            var vm = appButton.DataContext as ThreadPageViewModel;

            if (vm == null)
                return;
            Locator.ViewModels.NewThreadReplyVm.ForumThreadEntity = vm.ForumThreadEntity;

            App.RootFrame.Navigate(typeof (NewThreadReplyPage));
        }
    }
}
