using System;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsReader.Common;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands.PrivateMessages
{
    public class SendPrivateMessageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var vm = Locator.ViewModels.NewPrivateMessagePageVm;
            var pmManager = new PrivateMessageManager();
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

            if (string.IsNullOrEmpty(vm.PostRecipient))
            {
                AwfulDebugger.SendMessageDialogAsync("Can't post the thread yet!", new Exception("Recipient is empty"));
                return;
            }

            var pmEntity = new NewPrivateMessageEntity();
            vm.IsLoading = true;
            pmEntity.Title = vm.PostSubject;
            pmEntity.Body = vm.PostBody;
            pmEntity.Icon = vm.PostIcon;
            pmEntity.Receiver = vm.PostRecipient;

            try
            {
                bool result = await pmManager.SendPrivateMessage(pmEntity);
                if (result)
                {
                    App.RootFrame.GoBack();
                }
                else
                {
                    await AwfulDebugger.SendMessageDialogAsync("Error making the pm.", new Exception());
                }
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Error making the pm.", ex);
            }
            vm.IsLoading = false;
        }
    }
}
