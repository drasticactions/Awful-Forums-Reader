using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands
{
    public class ChangeTabsCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var args = parameter as ItemClickEventArgs;
            if (args == null)
            {
                AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", new Exception("Arguments are null"));
                return;
            }

            var thread = args.ClickedItem as ForumThreadEntity;
            if (thread == null)
                return;
            Locator.ViewModels.ThreadPageVm.IsLoading = true;
            Locator.ViewModels.ThreadPageVm.ForumThreadEntity = thread;
            await Locator.ViewModels.ThreadPageVm.GetForumPostsAsync();
        }
    }
}
