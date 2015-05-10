using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands
{
    public class NavigateToPrivateMessagePageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var args = parameter as ItemClickEventArgs;
            if (args == null)
            {
                AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", new Exception("Arguments are null"));
                return;
            }
            var thread = args.ClickedItem as PrivateMessageEntity;
            if (thread == null)
                return;
           
            var vm = Locator.ViewModels.PrivateMessagePageVm;
            vm.IsLoading = true;
            vm.PrivateMessageEntity = thread;
            try
            {
                await vm.GetPrivateMessageHtml();
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", ex);
                App.RootFrame.GoBack();
            }

        }
    }
}
