using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace AwfulForumsReader.Tools
{
    public class PageNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var currentPage =(int)value;
            return $"{currentPage} / {Locator.ViewModels.ThreadPageVm.ForumThreadEntity.TotalPages}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
