using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using AwfulForumsLibrary.Tools;

namespace AwfulForumsReader.Tools
{
    public class HasSeenThreadConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (!localSettings.Values.ContainsKey(Constants.BackgroundWallpaper))
                return (bool)value ? new SolidColorBrush(Color.FromArgb(255, 212, 225, 238)) : new SolidColorBrush(Color.FromArgb(255, 241, 241, 241));
            var hasWallpaper = (bool)localSettings.Values[Constants.BackgroundWallpaper];
            if (hasWallpaper)
            {
                return (bool)value ? new SolidColorBrush(Color.FromArgb(155, 212, 225, 238)) : new SolidColorBrush(Color.FromArgb(155, 241, 241, 241));
            }
            else
            {
                return (bool)value ? new SolidColorBrush(Color.FromArgb(255, 212, 225, 238)) : new SolidColorBrush(Color.FromArgb(255, 241, 241, 241));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
