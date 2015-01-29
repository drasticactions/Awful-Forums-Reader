using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsReader.Database.Context;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Database.Commands;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands
{
    public class NavigateToThreadListPageCommand : AlwaysExecutableCommand
    {
        public override async void Execute(object parameter)
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
                var subforumManager = new SubforumManager();
                await subforumManager.RemoveAllEntries();
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", ex);
            }
        }
    }

    public class NavigateToSubforumThreadListPageCommand : AlwaysExecutableCommand
    {
        public override async void Execute(object parameter)
        {
            var args = parameter as ItemClickEventArgs;
            if (args == null)
            {
                AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", new Exception("Arguments are null"));
                return;
            }
            try
            {
                var lastForum = Locator.ViewModels.ThreadListPageVm.ForumEntity;
                var forumEntity = (ForumEntity)args.ClickedItem;
                var threadViewModel = Locator.ViewModels.ThreadListPageVm;
                threadViewModel.Initialize(forumEntity);
                App.RootFrame.Navigate(typeof(ThreadListPage));
                var subforumManager = new SubforumManager();
                await subforumManager.AddForumToHistoryListAsync(lastForum);
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", ex);
            }
        }
    }
}
