using System;
using Windows.UI.Xaml.Controls;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Common;
using AwfulForumsReader.Database;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands.Navigation
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

            Locator.ViewModels.ThreadPageVm.ForumThreadEntity = thread;
            Locator.ViewModels.ThreadPageVm.Html = null;
            await Locator.ViewModels.ThreadPageVm.GetForumPostsAsync();
        }
    }
}
