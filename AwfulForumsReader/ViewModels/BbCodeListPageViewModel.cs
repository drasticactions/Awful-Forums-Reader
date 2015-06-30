using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsReader.Commands.Posts;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class BbCodeListPageViewModel : NotifierBase
    {
        private bool _isLoading;
        private TextBox _replyBox;
        private ObservableCollection<BbCodeCategoryEntity> _bbCodeList = new ObservableCollection<BbCodeCategoryEntity>();
        private AddBbCodeToTextboxCommand _addBbCodeToTextboxCommand = new AddBbCodeToTextboxCommand();
        public ReplyBoxLocation ReplyBoxLocation { get; set; }
        public AddBbCodeToTextboxCommand AddBbCodeToTextboxCommand
        {
            get { return _addBbCodeToTextboxCommand; }
            set { _addBbCodeToTextboxCommand = value; }
        }

        public ObservableCollection<BbCodeCategoryEntity> BbCodeList
        {
            get { return _bbCodeList; }
            set
            {
                SetProperty(ref _bbCodeList, value);
                OnPropertyChanged();
            }
        }

        public TextBox ReplyBox
        {
            get { return _replyBox; }
            set
            {
                SetProperty(ref _replyBox, value);
                OnPropertyChanged();
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                SetProperty(ref _isLoading, value);
                OnPropertyChanged();
            }
        }

        public void Initialize()
        {
            if (BbCodeList == null || !BbCodeList.Any())
            {
                BbCodeList = new ObservableCollection<BbCodeCategoryEntity>();
                var list = BbCodeManager.BBCodes;
                foreach (var item in list)
                {
                    BbCodeList.Add(item);
                }
            }
            OnPropertyChanged("BbCodes");
        }
    }
}
