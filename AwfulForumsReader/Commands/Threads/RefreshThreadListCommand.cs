using AwfulForumsReader.Common;

namespace AwfulForumsReader.Commands.Threads
{
    public class RefreshThreadListCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            Locator.ViewModels.ThreadListPageVm.Refresh();
        }
    }
}
