using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsReader.Database;
using AwfulForumsReader.Notification;

namespace AwfulForumsReader.BackgroundNotify
{
    public sealed class BackgroundNotifyStatus : IBackgroundTask
    {
        private readonly ThreadManager _threadManager = new ThreadManager();

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            try
            {
                if (NotifyStatusTile.IsInternet())
                {
                    await Update(taskInstance);
                }
            }
            catch (Exception)
            {
            }
            deferral.Complete();
        }

        private void CreateBookmarkLiveTiles(IEnumerable<ForumThreadEntity> forumThreads)
        {
            foreach (var thread in forumThreads.Where(thread => thread.RepliesSinceLastOpened > 0))
            {
                NotifyStatusTile.CreateBookmarkLiveTile(thread);
            }
        }

        private async Task Update(IBackgroundTaskInstance taskInstance)
        {
            var bookmarkManager = new MainForumsDatabase();

            var forumThreadEntities = await bookmarkManager.RefreshBookmarkedThreads();
            CreateBookmarkLiveTiles(forumThreadEntities);

            var notifyList = forumThreadEntities.Where(node => node.IsNotified);
            CreateToastNotifications(notifyList);
        }

        private void CreateToastNotifications(IEnumerable<ForumThreadEntity> forumThreads)
        {
            foreach (var thread in forumThreads.Where(thread => thread.RepliesSinceLastOpened > 0))
            {
                NotifyStatusTile.CreateToastNotification(thread);
            }
        }
    }
}
