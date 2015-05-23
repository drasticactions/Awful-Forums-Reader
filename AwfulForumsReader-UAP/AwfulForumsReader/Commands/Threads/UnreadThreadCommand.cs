using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsReader.Common;

namespace AwfulForumsReader.Commands.Threads
{
    public class UnreadThreadCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var thread = (ForumThreadEntity)parameter;
            var threadManager = new ThreadManager();
            await threadManager.MarkThreadUnreadAsync(thread.ThreadId);
            thread.HasBeenViewed = false;
            thread.HasSeen = false;
            thread.ReplyCount = 0;
        }
    }
}
