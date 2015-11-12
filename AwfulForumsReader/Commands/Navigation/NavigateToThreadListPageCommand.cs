using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands.Navigation
{
    public class NavigateToThreadListPageCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var args = parameter as ItemClickEventArgs;
            if (args == null)
            {
                AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", new Exception("Arguments are null"));
                return;
            }
            try
            {
                var forumEntity = (ForumEntity)args.ClickedItem;
                var threadViewModel = Locator.ViewModels.ThreadListPageVm;
                threadViewModel.Initialize(forumEntity);
                App.RootFrame.Navigate(typeof(ThreadListPage));
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", ex);
            }
        }
    }

    public class NavigateToThreadListPageCommandViaJumplist : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var forumEntityId = (long)parameter;
            if (forumEntityId == 0)
            {
                AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", new Exception("Arguments are null"));
                return;
            }
            try
            {
                var forumList = Locator.ViewModels.MainForumsPageVm.ForumGroupList.SelectMany(node => node.ForumList);
                var forum = forumList.First(node => node.Id == forumEntityId);
                var threadViewModel = Locator.ViewModels.ThreadListPageVm;
                threadViewModel.Initialize(forum);
                App.RootFrame.Navigate(typeof(ThreadListPage));
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", ex);
            }
        }
    }

    public class NavigateToThreadListPageCommandViaTile : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var forumEntity = parameter as ForumEntity;
            if (forumEntity == null)
            {
                AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", new Exception("Arguments are null"));
                return;
            }
            try
            {
                var threadViewModel = Locator.ViewModels.ThreadListPageVm;
                threadViewModel.Initialize(forumEntity);
                App.RootFrame.Navigate(typeof(ThreadListPage));
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", ex);
            }
        }
    }


}
