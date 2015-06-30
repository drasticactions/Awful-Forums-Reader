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
    public class SmiliesPageViewModel : NotifierBase
    {
        private bool _isLoading;
        private ObservableCollection<SmileCategoryEntity> _smileCategoryList = new ObservableCollection<SmileCategoryEntity>();
        public ReplyBoxLocation ReplyBoxLocation { get; set; }
        private TextBox _replyBox;
        private SmiliesFilterOnItemClick _smiliesFilterOnItemClick = new SmiliesFilterOnItemClick();

        public SmiliesFilterOnItemClick SmiliesFilterOnItemClick
        {
            get { return _smiliesFilterOnItemClick; }
            set { _smiliesFilterOnItemClick = value; }
        }

        private SmiliesFilterOnSuggestedQuery _smiliesFilterOnSuggestedQuery = new SmiliesFilterOnSuggestedQuery();
        private SmiliesFilterOnChangedQuery _smiliesFilterOnChangedQuery = new SmiliesFilterOnChangedQuery();
        private SmiliesFilterOnSubmittedQuery _smiliesFilterOnSubmittedQuery = new SmiliesFilterOnSubmittedQuery();
       

        public SmiliesFilterOnSubmittedQuery SmiliesFilterOnSubmittedQuery
        {
            get { return _smiliesFilterOnSubmittedQuery; }
            set { _smiliesFilterOnSubmittedQuery = value; }
        }

        public SmiliesFilterOnChangedQuery SmiliesFilterOnChangedQuery
        {
            get { return _smiliesFilterOnChangedQuery; }
            set { _smiliesFilterOnChangedQuery = value; }
        }

        public SmiliesFilterOnSuggestedQuery SmiliesFilterOnSuggestedQuery
        {
            get { return _smiliesFilterOnSuggestedQuery; }
            set { _smiliesFilterOnSuggestedQuery = value; }
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

        private readonly SmileManager _smileManager = new SmileManager();
        public ObservableCollection<SmileCategoryEntity> SmileCategoryList
        {
            get { return _smileCategoryList; }
            set
            {
                SetProperty(ref _smileCategoryList, value);
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

        public ObservableCollection<SmileCategoryEntity> FullSmileCategoryEntities { get; set; }

        public async Task Initialize()
        {
            if (!SmileCategoryList.Any())
            {
                IsLoading = true;
                var list = await _smileManager.GetSmileList();
                FullSmileCategoryEntities = list.ToObservableCollection();
                foreach (var item in list)
                {
                    SmileCategoryList.Add(item);
                }
                IsLoading = false;
            }
            OnPropertyChanged("SmileCategoryList");
        }
    }
}
