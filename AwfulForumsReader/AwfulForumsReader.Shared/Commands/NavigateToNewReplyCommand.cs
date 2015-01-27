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

            Locator.ViewModels.NewThreadReplyVm.PostBody = string.Empty;
            Locator.ViewModels.NewThreadReplyVm.ForumThreadEntity = vm.ForumThreadEntity;
            App.RootFrame.Navigate(typeof(NewThreadReplyPage));
            Locator.ViewModels.NewThreadReplyVm.IsLoading = true;
            try
            {
                var replyManager = new ReplyManager();
                Locator.ViewModels.NewThreadReplyVm.ForumReplyEntity =
                    await replyManager.GetReplyCookies(Locator.ViewModels.NewThreadReplyVm.ForumThreadEntity);
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Can't reply in this thread.", ex);
                App.RootFrame.GoBack();
            }


            Locator.ViewModels.NewThreadReplyVm.IsLoading = false;
        }
    }

    public class NavigateToEditPostPageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            long id = Int64.Parse((string)parameter);
            var vm = Locator.ViewModels.ThreadPageVm;
            
            if (vm == null)
                return;
            Locator.ViewModels.NewThreadReplyVm.PostBody = string.Empty;
            Locator.ViewModels.NewThreadReplyVm.IsEdit = true;
            Locator.ViewModels.NewThreadReplyVm.ForumThreadEntity = vm.ForumThreadEntity;

            App.RootFrame.Navigate(typeof(EditPostPage));
            Locator.ViewModels.NewThreadReplyVm.IsLoading = true;
            try
            {
                var replyManager = new ReplyManager();
                Locator.ViewModels.NewThreadReplyVm.ForumReplyEntity =
                    await replyManager.GetReplyCookiesForEdit(id);
                Locator.ViewModels.NewThreadReplyVm.PostBody =
                    Locator.ViewModels.NewThreadReplyVm.ForumReplyEntity.Quote;
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Can't reply in this thread.", ex);
                App.RootFrame.GoBack();
            }
            Locator.ViewModels.NewThreadReplyVm.IsLoading = false;
        }
    }

    public class NavigateToNewReplyViaQuoteCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            long id = Int64.Parse((string)parameter);
            var vm = Locator.ViewModels.ThreadPageVm;

            if (vm == null)
                return;
            Locator.ViewModels.NewThreadReplyVm.ForumThreadEntity = vm.ForumThreadEntity;

            App.RootFrame.Navigate(typeof(NewThreadReplyPage));
            Locator.ViewModels.NewThreadReplyVm.IsLoading = true;
            try
            {
                var replyManager = new ReplyManager();
                Locator.ViewModels.NewThreadReplyVm.ForumReplyEntity =
                    await replyManager.GetReplyCookies(id);
                Locator.ViewModels.NewThreadReplyVm.PostBody =
                    Locator.ViewModels.NewThreadReplyVm.ForumReplyEntity.Quote;
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Can't reply in this thread.", ex);
                App.RootFrame.GoBack();
            }
            Locator.ViewModels.NewThreadReplyVm.IsLoading = false;
        }
    }
}
