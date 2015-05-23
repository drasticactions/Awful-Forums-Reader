using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Common;

namespace AwfulForumsReader.Commands.Threads
{
    public class LastPageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var thread = parameter as ForumThreadEntity;
            if (thread == null)
                return;
            thread.CurrentPage = thread.TotalPages;
            thread.RepliesSinceLastOpened = 0;
        }
    }
}
