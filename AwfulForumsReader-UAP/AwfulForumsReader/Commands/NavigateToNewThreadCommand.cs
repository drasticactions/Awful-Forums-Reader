using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;
using AwfulForumsReader.ViewModels;

namespace AwfulForumsReader.Commands
{
    public class NavigateToNewThreadCommand : AlwaysExecutableCommand
    {
        private readonly ThreadManager _threadManager = new ThreadManager();

        public async override void Execute(object parameter)
        {
            var args = parameter as RoutedEventArgs;
            if (args == null)
            {
                await AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", new Exception("Arguments are null"));
                return;
            }

            //var appButton = args.OriginalSource as AppBarButton;
            //if (appButton == null)
            //{
            //    // TODO: If this is somehow null, throw an error and tell the user.
            //    return;
            //}
            //var vm = appButton.DataContext as ThreadListPageViewModel;

            //if (vm == null)
            //    return;
            //App.RootFrame.Navigate(typeof(NewThreadPage));
            //Locator.ViewModels.NewThreadVm.IsLoading = true;
            //try
            //{
            //    Locator.ViewModels.NewThreadVm.NewThreadEntity = await _threadManager.GetThreadCookiesAsync(vm.ForumEntity.ForumId);
            //    if (Locator.ViewModels.NewThreadVm.NewThreadEntity == null)
            //    {
            //        await AwfulDebugger.SendMessageDialogAsync("You can't post in this forum!",
            //            new Exception("Forum locked for new posts"));
            //        App.RootFrame.GoBack();
            //        return;
            //    }
            //}
            //catch (Exception exception)
            //{
            //    AwfulDebugger.SendMessageDialogAsync("Error getting thread cookies for this forum.", exception);
            //    App.RootFrame.GoBack();
            //    return;
            //}

            //Locator.ViewModels.NewThreadVm.ForumEntity = vm.ForumEntity;
            //Locator.ViewModels.NewThreadVm.PostBody = string.Empty;
            //Locator.ViewModels.NewThreadVm.PostSubject = string.Empty;
            //Locator.ViewModels.NewThreadVm.PostIcon = new PostIconEntity()
            //{
            //    Id = 0,
            //    ImageUrl = "/Assets/ThreadTags/shitpost.png",
            //    Title = "Shit Post"
            //};
            //Locator.ViewModels.NewThreadVm.IsLoading = false;
        }
    }
}
