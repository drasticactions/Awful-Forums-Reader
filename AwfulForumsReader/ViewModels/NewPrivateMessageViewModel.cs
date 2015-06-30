using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Commands.Navigation;
using AwfulForumsReader.Commands.Posts;
using AwfulForumsReader.Commands.PrivateMessages;

namespace AwfulForumsReader.ViewModels
{
    public class NewPrivateMessageViewModel : NotifierBase
    {
        private bool _isLoading;

        public SendPrivateMessageCommand SendPrivateMessageCommand { get; set; } = new SendPrivateMessageCommand();

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

        public NavigateToSmiliesPageCommand NavigateToSmiliesPageCommand { get; set; } = new NavigateToSmiliesPageCommand();

        public NavigateToBbCodePageCommand NavigateToBbCodePageCommand { get; set; } = new NavigateToBbCodePageCommand();

        public NavigateToPostIconPageCommand NavigateToPostIconPageCommand { get; set; } = new NavigateToPostIconPageCommand();

        public ImgurAddImageCommand ImgurAddImageCommand { get; set; } = new ImgurAddImageCommand();

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
