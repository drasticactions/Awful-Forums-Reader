using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class NewThreadReplyPageViewModel : NotifierBase
    {
        private bool _isLoading;
        private ImgurAddImageCommand _imgurAddImageCommand = new ImgurAddImageCommand();
        private NavigateToPostIconPageCommand _navigateToPostIconPageCommand = new NavigateToPostIconPageCommand();
        private NavigateToPreviewThreadCommand _navigateToPreviewThreadCommand = new NavigateToPreviewThreadCommand();
        private NavigateToBbCodePageCommand _navigateToBbCodePageCommand = new NavigateToBbCodePageCommand();
        private NavigateToSmiliesPageCommand _navigateToSmiliesPageCommand = new NavigateToSmiliesPageCommand();
        private PostThreadReplyCommand _postThreadReplyCommand = new PostThreadReplyCommand();
        private ForumThreadEntity _forumThreadEntity;

        public ForumThreadEntity ForumThreadEntity
        {
            get { return _forumThreadEntity; }
            set
            {
                SetProperty(ref _forumThreadEntity, value);
                OnPropertyChanged();
            }
        }

        public PostThreadReplyCommand PostThreadReplyCommand
        {
            get { return _postThreadReplyCommand; }
            set { _postThreadReplyCommand = value; }
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

        public NavigateToPreviewThreadCommand NavigateToPreviewThreadCommand
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
