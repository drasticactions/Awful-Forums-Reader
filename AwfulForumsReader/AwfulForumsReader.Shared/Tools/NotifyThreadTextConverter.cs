using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Database.Commands;

namespace AwfulForumsReader.Tools
{
    public class NotifyThreadTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var thread = value as ForumThreadEntity;
            if (thread == null)
            {
                return string.Empty;
            }
            var notificationManager = new NotificationManager();
            var result = notificationManager.IsInList(thread);
            return result ? "Remove from Notification List" : "Add To Notification List";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
