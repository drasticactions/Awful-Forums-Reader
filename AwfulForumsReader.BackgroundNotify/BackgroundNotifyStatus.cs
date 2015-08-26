using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Database;
using AwfulForumsReader.Notification;

namespace AwfulForumsReader.BackgroundNotify
{
    public sealed class BackgroundNotifyStatus : IBackgroundTask
    {
        private readonly ThreadManager _threadManager = new ThreadManager();
        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            try
            {
                if ((bool) _localSettings.Values[Constants.BackgroundEnable])
                {
                    if (NotifyStatusTile.IsInternet())
                    {
                        await Update(taskInstance);
                    }
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

            if (_localSettings.Values.ContainsKey(Constants.BookmarkBackground))
            {
                if ((bool) _localSettings.Values[Constants.BookmarkBackground])
                {
                    CreateBookmarkLiveTiles(forumThreadEntities);
                }
            }

            if (_localSettings.Values.ContainsKey(Constants.BookmarkNotifications))
            {
                if ((bool)_localSettings.Values[Constants.BookmarkNotifications])
                {
                    var notifyList = forumThreadEntities.Where(node => node.IsNotified);
                    CreateToastNotifications(notifyList);
                }
            }
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
