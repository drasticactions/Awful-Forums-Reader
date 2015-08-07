using System;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Common;
using AwfulForumsReader.Database;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands.Threads
{
    public class AddThreadToNotificationTable : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var thread = (ForumThreadEntity)parameter;
            if (!thread.IsBookmark)
            {
                await
                    AwfulDebugger.SendMessageDialogAsync(
                        "In order to be notified of thread updates, the thread must be a bookmark.",
                        new Exception("Not a bookmark"));
                return;
            }
            var mfd = new MainForumsDatabase();
            try
            {
                await mfd.SetThreadNotified(thread);
            }
            catch (Exception ex)
            {
                await
                   AwfulDebugger.SendMessageDialogAsync(
                       "Failed to save thread to notifications table",
                       ex);
            }
        }
    }
}
