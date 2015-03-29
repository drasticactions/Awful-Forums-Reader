using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;
using AwfulForumsReader.Database;

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
            App.RootFrame.Navigate(typeof(ThreadPage));
            var tabManager = new MainForumsDatabase();
            await tabManager.RemoveAllThreadsFromTabList();
            await tabManager.AddThreadToTabListAsync(thread);
            var tabThreads = await tabManager.GetAllTabThreads();
            Locator.ViewModels.ThreadPageVm.LinkedThreads = tabThreads.ToObservableCollection();

            Locator.ViewModels.ThreadPageVm.ForumThreadEntity = thread;
            Locator.ViewModels.ThreadPageVm.Html = null;
            await Locator.ViewModels.ThreadPageVm.GetForumPostsAsync();
        }
    }

    public class NavigateToThreadPageViaSearchResult : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var args = parameter as ItemClickEventArgs;
            if (args == null)
            {
                AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", new Exception("Arguments are null"));
                return;
            }
            var thread = args.ClickedItem as SearchEntity;
            if (thread == null)
                return;
            App.RootFrame.Navigate(typeof(ThreadPage));
            Locator.ViewModels.ThreadPageVm.IsLoading = true;
            var newThreadEntity = new ForumThreadEntity()
            {
                Location = Constants.BaseUrl + thread.ThreadLink,
                ImageIconLocation = "/Assets/ThreadTags/noicon.png"
            };
            Locator.ViewModels.ThreadPageVm.ForumThreadEntity = newThreadEntity;
            await Locator.ViewModels.ThreadPageVm.GetForumPostsAsync();
            var tabManager = new MainForumsDatabase();
            await tabManager.AddThreadToTabListAsync(newThreadEntity);
            Locator.ViewModels.ThreadPageVm.LinkedThreads.Add(newThreadEntity);
        }
    }

    public class NavigateToLastPageInThreadPageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var thread = parameter as ForumThreadEntity;
            if (thread == null)
            {
                AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", new Exception("Arguments are null"));
                return;
            }

            App.RootFrame.Navigate(typeof(ThreadPage));
            var tabManager = new MainForumsDatabase();
            await tabManager.RemoveAllThreadsFromTabList();
            await tabManager.AddThreadToTabListAsync(thread);
            var tabThreads = await tabManager.GetAllTabThreads();
            Locator.ViewModels.ThreadPageVm.LinkedThreads = tabThreads.ToObservableCollection();
            
            thread.CurrentPage = thread.TotalPages;
            thread.RepliesSinceLastOpened = 0;
            Locator.ViewModels.ThreadPageVm.ForumThreadEntity = thread;
            Locator.ViewModels.ThreadPageVm.Html = null;
            await Locator.ViewModels.ThreadPageVm.GetForumPostsAsync();
        }
    }

    public class NavigateToThreadPageViaToastCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var thread = parameter as ForumThreadEntity;
            if (thread == null)
            {
                AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", new Exception("Arguments are null"));
                return;
            }

            App.RootFrame.Navigate(typeof(ThreadPage));
            var tabManager = new MainForumsDatabase();
            await tabManager.RemoveAllThreadsFromTabList();
            await tabManager.AddThreadToTabListAsync(thread);
            var tabThreads = await tabManager.GetAllTabThreads();
            Locator.ViewModels.ThreadPageVm.LinkedThreads = tabThreads.ToObservableCollection();

            Locator.ViewModels.ThreadPageVm.ForumThreadEntity = thread;
            Locator.ViewModels.ThreadPageVm.Html = null;
            await Locator.ViewModels.ThreadPageVm.GetForumPostsAsync();
        }
    }
}
