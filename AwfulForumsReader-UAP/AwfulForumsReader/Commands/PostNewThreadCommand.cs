using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Manager;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands
{
    public class PostNewThreadCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var vm = Locator.ViewModels.NewThreadVm;
            var threadManager = new ThreadManager();
            if (string.IsNullOrEmpty(vm.PostSubject))
            {
                AwfulDebugger.SendMessageDialogAsync("Can't post the thread yet!", new Exception("Submit is empty"));
                return;
            }

            if (string.IsNullOrEmpty(vm.PostBody))
            {
                AwfulDebugger.SendMessageDialogAsync("Can't post the thread yet!", new Exception("Body is empty"));
                return;
            }


            vm.IsLoading = true;
            vm.NewThreadEntity.Subject = vm.PostSubject;
            vm.NewThreadEntity.Content = vm.PostBody;
            vm.NewThreadEntity.PostIcon = vm.PostIcon;
            vm.NewThreadEntity.Forum = vm.ForumEntity;

            try
            {
                bool result = await threadManager.CreateNewThreadAsync(vm.NewThreadEntity);
                if (result)
                {
                    App.RootFrame.GoBack();
                    Locator.ViewModels.ThreadListPageVm.Refresh();
                }
                else
                {
                    await AwfulDebugger.SendMessageDialogAsync("Error making the thread.", new Exception());
                }
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Error making the thread.", ex);
            }
            vm.IsLoading = false;
        }
    }
}
