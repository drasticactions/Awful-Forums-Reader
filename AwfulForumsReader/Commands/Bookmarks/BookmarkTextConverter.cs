using System;
using Windows.UI.Xaml.Data;

namespace AwfulForumsReader.Commands.Bookmarks
{
    public class BookmarkTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? "Remove Bookmark" : "Add as Bookmark";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
