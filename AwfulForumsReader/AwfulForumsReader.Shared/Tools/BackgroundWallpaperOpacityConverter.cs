using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Windows.Storage;
using AwfulForumsReader.Core.Tools;
using Windows.UI.Xaml.Data;

namespace AwfulForumsReader.Tools
{
    public class BackgroundWallpaperOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            if (!localSettings.Values.ContainsKey(Constants.BackgroundWallpaper)) return 1;
            var hasWallpaper = (bool)localSettings.Values[Constants.BackgroundWallpaper];
            return hasWallpaper ? .65f : 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
