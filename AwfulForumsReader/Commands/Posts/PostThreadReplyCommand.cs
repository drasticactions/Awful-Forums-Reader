using System;
using Windows.UI.Popups;
using AwfulForumsLibrary.Manager;
using AwfulForumsReader.Common;

namespace AwfulForumsReader.Commands.Posts
{
    public class PostThreadReplyCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var replyManager = new ReplyManager();
            var vm = Locator.ViewModels.NewThreadReplyVm;
            vm.ForumReplyEntity.Message = vm.PostBody;
            vm.IsLoading = true;
            bool result = await replyManager.SendPost(vm.ForumReplyEntity);
            if (result)
            {
                Locator.ViewModels.ThreadPageVm.RefreshThreadPageCommand.Execute(string.Empty);
                App.RootFrame.GoBack();
            }
            else
            {
                vm.IsLoading = false;
                var msgDlg = new MessageDialog("Error making reply!");
                await msgDlg.ShowAsync();
            }
            vm.IsLoading = false;
        }
    }

    public class EditThreadReplyCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var replyManager = new ReplyManager();
            var vm = Locator.ViewModels.NewThreadReplyVm;
            vm.ForumReplyEntity.Message = vm.PostBody;
            vm.IsLoading = true;
            bool result = await replyManager.SendUpdatePost(vm.ForumReplyEntity);
            if (result)
            {
                Locator.ViewModels.ThreadPageVm.RefreshThreadPageCommand.Execute(string.Empty);
                App.RootFrame.GoBack();
            }
            else
            {
                vm.IsLoading = false;
                var msgDlg = new MessageDialog("Error editing reply!");
                await msgDlg.ShowAsync();
            }
            vm.IsLoading = false;
        }
    }
}
