using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;

namespace AwfulForumsReader.ViewModels
{
    public class SearchPageViewModel : NotifierBase
    {
        private bool _isLoading;

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);
                OnPropertyChanged();
            }
        }

    }
}
