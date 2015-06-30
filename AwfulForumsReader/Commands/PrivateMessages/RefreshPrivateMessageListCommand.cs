using AwfulForumsReader.Common;

namespace AwfulForumsReader.Commands.PrivateMessages
{
    public class RefreshPrivateMessageListCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            Locator.ViewModels.PrivateMessageVm.GetPrivateMessages();
        }
    }
}
