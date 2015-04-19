using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Common;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class PrivateMessageListViewModel : NotifierBase
    {
        private PrivateMessageScrollingCollection _privateMessageScrollingCollection;

        public NavigateToPrivateMessagePageCommand NavigateToPrivateMessagePageCommand { get; set; } = new NavigateToPrivateMessagePageCommand();

        public RefreshPrivateMessageListCommand RefreshPrivateMessageListCommand { get; set; } = new RefreshPrivateMessageListCommand();

        public NavigateToNewPrivateMessagePageCommand NavigateToNewPrivateMessagePageCommand { get; set; } = new NavigateToNewPrivateMessagePageCommand();

        public PrivateMessageScrollingCollection PrivateMessageScrollingCollection
        {
            get { return _privateMessageScrollingCollection; }
            set
            {
                SetProperty(ref _privateMessageScrollingCollection, value);
                OnPropertyChanged();
            }
        }

        public void GetPrivateMessages()
        {
            PrivateMessageScrollingCollection = new PrivateMessageScrollingCollection();
        }
    }
}
