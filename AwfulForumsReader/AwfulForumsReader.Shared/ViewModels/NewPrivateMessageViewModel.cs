using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;

namespace AwfulForumsReader.ViewModels
{
    public class NewPrivateMessageViewModel : NotifierBase
    {
        private bool _isLoading;
        private ImgurAddImageCommand _imgurAddImageCommand = new ImgurAddImageCommand();
        private NavigateToPostIconPageCommand _navigateToPostIconPageCommand = new NavigateToPostIconPageCommand();
        private NavigateToBbCodePageCommand _navigateToBbCodePageCommand = new NavigateToBbCodePageCommand();
        private NavigateToSmiliesPageCommand _navigateToSmiliesPageCommand = new NavigateToSmiliesPageCommand();
        private SendPrivateMessageCommand _sendPrivateMessageCommand = new SendPrivateMessageCommand();

        public SendPrivateMessageCommand SendPrivateMessageCommand
        {
            get { return _sendPrivateMessageCommand; }
            set { _sendPrivateMessageCommand = value; }
        }
        private PostIconEntity _postIcon;
        private string _postBody;
        private string _postSubject;
        private string _postRecipient;

        public string PostSubject
        {
            get { return _postSubject; }
            set
            {
                SetProperty(ref _postSubject, value);
                OnPropertyChanged();
            }
        }

        public string PostBody
        {
            get { return _postBody; }
            set
            {
                SetProperty(ref _postBody, value);
                OnPropertyChanged();
            }
        }

        public string PostRecipient
        {
            get { return _postRecipient; }
            set
            {
                SetProperty(ref _postRecipient, value);
                OnPropertyChanged();
            }
        }

        public PostIconEntity PostIcon
        {
            get { return _postIcon; }
            set
            {
                SetProperty(ref _postIcon, value);
                OnPropertyChanged();
            }
        }

        public NavigateToSmiliesPageCommand NavigateToSmiliesPageCommand
        {
            get { return _navigateToSmiliesPageCommand; }
            set { _navigateToSmiliesPageCommand = value; }
        }

        public NavigateToBbCodePageCommand NavigateToBbCodePageCommand
        {
            get { return _navigateToBbCodePageCommand; }
            set { _navigateToBbCodePageCommand = value; }
        }

        public NavigateToPostIconPageCommand NavigateToPostIconPageCommand
        {
            get { return _navigateToPostIconPageCommand; }
            set { _navigateToPostIconPageCommand = value; }
        }

        public ImgurAddImageCommand ImgurAddImageCommand
        {
            get { return _imgurAddImageCommand; }
            set { _imgurAddImageCommand = value; }
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
    }
}
