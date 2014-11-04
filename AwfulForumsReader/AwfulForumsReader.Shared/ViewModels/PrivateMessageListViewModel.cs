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
        private NavigateToNewPrivateMessagePageCommand _navigateToNewPrivateMessagePageCommand = new NavigateToNewPrivateMessagePageCommand();
        private RefreshPrivateMessageListCommand _refreshPrivateMessageListCommand = new RefreshPrivateMessageListCommand();

        public RefreshPrivateMessageListCommand RefreshPrivateMessageListCommand
        {
            get { return _refreshPrivateMessageListCommand; }
            set { _refreshPrivateMessageListCommand = value; }
        }

        public NavigateToNewPrivateMessagePageCommand NavigateToNewPrivateMessagePageCommand
        {
            get { return _navigateToNewPrivateMessagePageCommand; }
            set { _navigateToNewPrivateMessagePageCommand = value; }
        }

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
