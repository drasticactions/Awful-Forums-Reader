using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Database.Commands;
using AwfulForumsReader.Notification;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands
{
    public class AddOrRemoveThreadToNotificationTableCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var thread = parameter as ForumThreadEntity;
            if (thread == null)
                return;
            var notificationManager = new NotificationManager();
            bool isInList = notificationManager.IsInList(thread);
            bool result = false;
            try
            {
                if (isInList)
                {
                    result = await notificationManager.RemoveThreadFromNotificationListAsync(thread);
                }
                else
                {
                    result = await notificationManager.AddThreadToNotificationListAsync(thread);
                }
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Error adding thread to notification table", ex);
            }

            if (result && !isInList)
            {
                NotifyStatusTile.CreateToastNotification(string.Format("You will now be notified when \"{0}\" is updated.", thread.Name));
            }
            else if (result)
            {
                NotifyStatusTile.CreateToastNotification(string.Format("Thread status notification for \"{0}\" is now removed.", thread.Name));   
            }

            Locator.ViewModels.BookmarksPageVm.UpdateThreadList();
        }
    }
}
