using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Common;
using AwfulForumsReader.Database;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands
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
            await mfd.SetThreadNotified(thread);
        }
    }
}
