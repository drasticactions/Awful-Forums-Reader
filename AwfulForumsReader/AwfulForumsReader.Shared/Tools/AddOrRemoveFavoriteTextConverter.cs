using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using AwfulForumsReader.Core.Entity;

namespace AwfulForumsReader.Tools
{
    public class AddOrRemoveFavoriteTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var forum = (ForumEntity) value;
            return forum.IsBookmarks ? "Remove Favorite" : "Add as Favorite";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
