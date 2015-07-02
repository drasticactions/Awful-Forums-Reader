using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using AwfulForumsLibrary.Tools;
using ThemeManagerRt;

namespace AwfulForumsReader.Tools
{
    public class HasSeenThreadConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            
            var localSettings = ApplicationData.Current.LocalSettings;
            if (!localSettings.Values.ContainsKey(Constants.BackgroundWallpaper))
            {
                return (bool)value ? Application.Current.Resources["HasSeenThreadColor"] as SolidColorBrush : Application.Current.Resources["ThreadColor"] as SolidColorBrush;
            }
            var hasWallpaper = (bool)localSettings.Values[Constants.BackgroundWallpaper];
            if (hasWallpaper)
            {
                return (bool)value ? Application.Current.Resources["TransparentHasSeenThreadColor"] as SolidColorBrush : Application.Current.Resources["TransparentThreadColor"] as SolidColorBrush;
            }
            else
            {
                return (bool)value ? Application.Current.Resources["HasSeenThreadColor"] as SolidColorBrush : Application.Current.Resources["ThreadColor"] as SolidColorBrush;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
