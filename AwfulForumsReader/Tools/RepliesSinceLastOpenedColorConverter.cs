using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace AwfulForumsReader.Tools
{
    public class RepliesSinceLastOpenedColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null) return new SolidColorBrush(Colors.Black);
            var count = (int)value;
            if (App.SelectedTheme == ApplicationTheme.Dark)
            {
                return new SolidColorBrush(Colors.White);
            }
            return count > 0 ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Color.FromArgb(255, 131, 131, 131));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
