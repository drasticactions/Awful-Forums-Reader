using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class NewThreadPageViewModel : NotifierBase
    {
        private bool _isLoading;
        private ImgurAddImageCommand _imgurAddImageCommand = new ImgurAddImageCommand();
        private NavigateToPostIconPageCommand _navigateToPostIconPageCommand = new NavigateToPostIconPageCommand();
        private NavigateToNewThreadPreviewCommand _navigateToPreviewThreadCommand = new NavigateToNewThreadPreviewCommand();
        private NavigateToBbCodePageCommand _navigateToBbCodePageCommand = new NavigateToBbCodePageCommand();
        private NavigateToSmiliesPageCommand _navigateToSmiliesPageCommand = new NavigateToSmiliesPageCommand();
        private PostNewThreadCommand _postNewThreadCommand = new PostNewThreadCommand();


        private ForumEntity _forumEntity;
        private PostIconEntity _postIcon;
        private NewThreadEntity _newThreadEntity;
        private string _postBody;
        private string _postSubject;

        public NewThreadEntity NewThreadEntity
        {
            get { return _newThreadEntity; }
            set
            {
                SetProperty(ref _newThreadEntity, value);
                OnPropertyChanged();
            }
        }

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

        public PostIconEntity PostIcon
        {
            get { return _postIcon; }
            set
            {
                SetProperty(ref _postIcon, value);
                OnPropertyChanged();
            }
        }

        public ForumEntity ForumEntity
        {
            get { return _forumEntity; }
            set
            {
                SetProperty(ref _forumEntity, value);
                OnPropertyChanged();
            }
        }

        public PostNewThreadCommand PostNewThreadReplyCommand
        {
            get { return _postNewThreadCommand; }
            set { _postNewThreadCommand = value; }
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

        public NavigateToNewThreadPreviewCommand NavigateToPreviewThreadCommand
        {
            get { return _navigateToPreviewThreadCommand; }
            set { _navigateToPreviewThreadCommand = value; }
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
