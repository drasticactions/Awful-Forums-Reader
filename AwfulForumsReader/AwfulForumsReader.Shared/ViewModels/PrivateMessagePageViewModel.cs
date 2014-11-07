using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Commands;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Core.Manager;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class PrivateMessagePageViewModel : NotifierBase
    {
        private bool _isLoading;
        private string _html;
        private PrivateMessageEntity _privateMessageEntity;
        private NavigateToReplyPrivateMessagePageCommand _navigateToReplyPrivateMessagePageCommand = new NavigateToReplyPrivateMessagePageCommand();

        public NavigateToReplyPrivateMessagePageCommand NavigateToReplyPrivateMessagePageCommand
        {
            get { return _navigateToReplyPrivateMessagePageCommand; }
            set { _navigateToReplyPrivateMessagePageCommand = value; }
        }

        public PrivateMessageEntity PrivateMessageEntity
        {
            get { return _privateMessageEntity; }
            set
            {
                SetProperty(ref _privateMessageEntity, value);
                OnPropertyChanged();
            }
        }

        public string Html
        {
            get { return _html; }
            set
            {
                SetProperty(ref _html, value);
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

        public async Task GetPrivateMessageHtml()
        {
            var privateMessageManager = new PrivateMessageManager();
            var postEntity = await privateMessageManager.GetPrivateMessageAsync(PrivateMessageEntity.MessageUrl);
            await FormatPmHtml(postEntity);
        }

        private async Task FormatPmHtml(ForumPostEntity postEntity)
        {
            var list = new List<ForumPostEntity> {postEntity};
            Html = await HtmlFormater.FormatThreadHtml(new ForumThreadEntity(){IsPrivateMessage = true}, list);
        }
    }
}
