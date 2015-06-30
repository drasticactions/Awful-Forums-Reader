using AwfulForumsReader.Common;

namespace AwfulForumsReader.Commands.Threads
{
    public class RefreshThreadPageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            await Locator.ViewModels.ThreadPageVm.GetForumPostsAsync();
        }
    }
}
