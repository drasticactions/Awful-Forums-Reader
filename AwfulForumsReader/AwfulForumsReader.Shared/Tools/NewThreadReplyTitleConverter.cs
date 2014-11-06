using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;

namespace AwfulForumsReader.Tools
{
    public class NewThreadReplyTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return "New Reply - " + (string) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
