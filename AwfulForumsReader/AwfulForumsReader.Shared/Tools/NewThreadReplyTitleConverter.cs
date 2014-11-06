using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Data;
using AwfulForumsReader.ViewModels;

namespace AwfulForumsReader.Tools
{
    public class NewThreadReplyTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var vm = value as NewThreadReplyPageViewModel;
            if (vm == null)
                return string.Empty;
            return vm.IsEdit ? "Edit - " + vm.ForumThreadEntity.Name : "New Reply - " + vm.ForumThreadEntity.Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
