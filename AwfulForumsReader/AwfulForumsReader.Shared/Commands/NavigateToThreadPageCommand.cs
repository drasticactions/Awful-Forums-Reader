using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Text;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Database.Commands;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands
{
    public class NavigateToThreadPageCommand : AlwaysExecutableCommand
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

            var tabManager = new TabManager();
            await tabManager.RemoveAllThreadsFromTabList();
            await tabManager.AddThreadToTabListAsync(thread);
            var tabThreads = await tabManager.GetAllTabThreads();
            Locator.ViewModels.ThreadPageVm.LinkedThreads = tabThreads.ToObservableCollection();

            Locator.ViewModels.ThreadPageVm.ForumThreadEntity = thread;
            Locator.ViewModels.ThreadPageVm.Html = null;
            App.RootFrame.Navigate(typeof (ThreadPage));
            await Locator.ViewModels.ThreadPageVm.GetForumPostsAsync();
        }
    }
}
