using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.ApplicationModel.Background;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsReader.Database.Commands;
using AwfulForumsReader.Notification;

namespace AwfulForumsReader.BackgroundStatus
{
    public sealed class BackgroundNotifyStatus : IBackgroundTask
    {
        private readonly ThreadManager _threadManager = new ThreadManager();

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            //
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            if (NotifyStatusTile.IsInternet())
            {
                await Update(taskInstance);
            }
            deferral.Complete();
        }

        private async Task Update(IBackgroundTaskInstance taskInstance)
        {
            var bookmarkManager = new BookmarkManager();

            var forumThreadEntities = await bookmarkManager.RefreshBookmarkedThreads();
            CreateBookmarkLiveTiles(forumThreadEntities);

            var notificationManager = new NotificationManager();
            var notifyList = await notificationManager.GetNotifyThreadListAsync();

            List<ForumThreadEntity> list = forumThreadEntities.Where(foo => notifyList.Any(node => node.ThreadId == foo.ThreadId)).ToList();
            CreateToastNotifications(list);
        }

        private void CreateBookmarkLiveTiles(IEnumerable<ForumThreadEntity> forumThreads)
        {
            foreach (ForumThreadEntity thread in forumThreads.Where(thread => thread.RepliesSinceLastOpened > 0))
            {
                NotifyStatusTile.CreateBookmarkLiveTile(thread);
            }
        }

        private void CreateToastNotifications(IEnumerable<ForumThreadEntity> forumThreads)
        {
            foreach (ForumThreadEntity thread in forumThreads.Where(thread => thread.RepliesSinceLastOpened > 0))
            {
                NotifyStatusTile.CreateToastNotification(thread);
            }
        }
    }
}
