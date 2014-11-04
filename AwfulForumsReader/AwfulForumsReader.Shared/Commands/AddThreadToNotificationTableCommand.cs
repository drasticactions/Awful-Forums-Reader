using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Database.Commands;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands
{
    public class AddThreadToNotificationTableCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var thread = parameter as ForumThreadEntity;
            if (thread == null)
                return;
            var notificationManager = new NotificationManager();

            try
            {
                var result = await notificationManager.AddThreadToNotificationListAsync(thread);
                if (result)
                {
                    // TODO: Make toast notification here...
                }
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Error adding thread to notification table", ex);
            }
        }
    }
}
