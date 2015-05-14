using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Common;

namespace AwfulForumsReader.ViewModels
{
    public class RapSheetPageViewModel : NotifierBase
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

        private string _html;

        public string Html
        {
            get { return _html; }
            set
            {
                SetProperty(ref _html, value);
                OnPropertyChanged();
            }
        }

        public async Task GetRapSheet()
        {
            //var rapSheetManager = new 
        }
    }
}
