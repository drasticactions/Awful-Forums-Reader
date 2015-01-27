using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Manager;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands
{
    public class NavigateToNewThreadPreviewCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var vm = Locator.ViewModels.NewThreadVm;
            var newVm = Locator.ViewModels.PreviewThreadPageVm;
            vm.NewThreadEntity.Subject = vm.PostSubject;
            vm.NewThreadEntity.Content = vm.PostBody;
            vm.NewThreadEntity.PostIcon = vm.PostIcon;
            vm.NewThreadEntity.Forum = vm.ForumEntity;
            App.RootFrame.Navigate(typeof (PreviewThreadPage));
            newVm.IsLoading = true;
            await newVm.CreateThreadPreview(vm.NewThreadEntity);
            newVm.IsLoading = false;
        }
    }

    public class NavigateToEditThreadPreviewCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var vm = Locator.ViewModels.NewThreadReplyVm;
            var newVm = Locator.ViewModels.PreviewThreadPageVm;
            vm.ForumReplyEntity.Message = vm.PostBody;
            App.RootFrame.Navigate(typeof(PreviewThreadPage));
            await newVm.CreateReplyPreview(vm.ForumReplyEntity, vm.IsEdit);

        }
    }
}
