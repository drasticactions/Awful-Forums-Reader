using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Windows.UI.Xaml.Data;


namespace AwfulForumsReader.Tools
{
    public class ForumDateJoinedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var date = (DateTime)value;
            return date.Date.ToString("D");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
