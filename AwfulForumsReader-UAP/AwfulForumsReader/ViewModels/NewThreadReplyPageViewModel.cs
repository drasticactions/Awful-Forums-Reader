using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Commands.Navigation;
using AwfulForumsReader.Commands.Posts;
using AwfulForumsReader.Common;

namespace AwfulForumsReader.ViewModels
{
    public class NewThreadReplyPageViewModel : NotifierBase
    {
        private bool _isLoading;
        private ForumReplyEntity _forumReplyEntity;

        public NavigateToEditThreadPreviewCommand NavigateToEditThreadPreviewCommand { get; set; } = new NavigateToEditThreadPreviewCommand();

        public EditThreadReplyCommand EditThreadReplyCommand { get; set; } = new EditThreadReplyCommand();

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



        public NavigateToLastPostPageCommand NavigateToLastPostPageCommand { get; set; } = new NavigateToLastPostPageCommand();

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

        public PostThreadReplyCommand PostThreadReplyCommand { get; set; } = new PostThreadReplyCommand();

        public NavigateToSmiliesPageCommand NavigateToSmiliesPageCommand { get; set; } = new NavigateToSmiliesPageCommand();

        public NavigateToBbCodePageCommand NavigateToBbCodePageCommand { get; set; } = new NavigateToBbCodePageCommand();

        public NavigateToNewThreadPreviewCommand NavigateToPreviewThreadCommand { get; set; } = new NavigateToNewThreadPreviewCommand();

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
