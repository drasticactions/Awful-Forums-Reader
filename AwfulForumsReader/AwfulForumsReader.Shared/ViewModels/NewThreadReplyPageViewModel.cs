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
        private NavigateToNewThreadPreviewCommand _navigateToPreviewThreadCommand = new NavigateToNewThreadPreviewCommand();
        private NavigateToBbCodePageCommand _navigateToBbCodePageCommand = new NavigateToBbCodePageCommand();
        private NavigateToSmiliesPageCommand _navigateToSmiliesPageCommand = new NavigateToSmiliesPageCommand();
        private PostThreadReplyCommand _postThreadReplyCommand = new PostThreadReplyCommand();
        private NavigateToLastPostPageCommand _navigateToLastPostPageCommand = new NavigateToLastPostPageCommand();
        private NavigateToEditThreadPreviewCommand _navigateToEditThreadPreviewCommand = new NavigateToEditThreadPreviewCommand();
        private ForumReplyEntity _forumReplyEntity;
        private EditThreadReplyCommand _editThreadReplyCommand = new EditThreadReplyCommand();

        public NavigateToEditThreadPreviewCommand NavigateToEditThreadPreviewCommand
        {
            get { return _navigateToEditThreadPreviewCommand; }
            set { _navigateToEditThreadPreviewCommand = value; }
        }

        public EditThreadReplyCommand EditThreadReplyCommand
        {
            get { return _editThreadReplyCommand; }
            set { _editThreadReplyCommand = value; }
        }

        public bool IsEdit { get; set; }
        public ForumReplyEntity ForumReplyEntity
        {
            get { return _forumReplyEntity; }
            set
            {
                SetProperty(ref _forumReplyEntity, value);
                OnPropertyChanged();
            }
        }



        public NavigateToLastPostPageCommand NavigateToLastPostPageCommand
        {
            get { return _navigateToLastPostPageCommand; }
            set { _navigateToLastPostPageCommand = value; }
        }

        private ForumThreadEntity _forumThreadEntity;

        private string _postBody;

        public string PostBody
        {
            get { return _postBody; }
            set
            {
                SetProperty(ref _postBody, value);
                OnPropertyChanged();
            }
        }
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
