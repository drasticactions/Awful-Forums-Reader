using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Notifications;
using AwfulForumsLibrary.Tools;

namespace AwfulForumsReader.BackgroundNotify
{
    public sealed class ToastNotificationBackgroundTask : IBackgroundTask
    {
        private readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

            var details = taskInstance.TriggerDetails as ToastNotificationActionTriggerDetail;
            var arguments = details?.Argument;
            if (arguments == "sleep")
            {
                _localSettings.Values[Constants.BookmarkNotifications] = false;
            }
            deferral.Complete();
        }
    }
}
