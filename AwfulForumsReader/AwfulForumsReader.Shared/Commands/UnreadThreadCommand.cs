using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;

namespace AwfulForumsReader.Commands
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
