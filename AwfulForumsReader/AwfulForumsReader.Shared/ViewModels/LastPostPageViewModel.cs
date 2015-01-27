using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class LastPostPageViewModel : NotifierBase
    {
        private bool _isLoading;
        private TextBox _replyBox;
        private readonly ThreadManager _threadManager = new ThreadManager();
        private string _html;
        private string _name;
        public TextBox ReplyBox
        {
            get { return _replyBox; }
            set
            {
                SetProperty(ref _replyBox, value);
                OnPropertyChanged();
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
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

        public string Html
        {
            get { return _html; }
            set
            {
                SetProperty(ref _html, value);
                OnPropertyChanged();
            }
        }

        public async Task Initialize(ForumReplyEntity replyEntity)
        {
            IsLoading = true;
            string htmlThread = await HtmlFormater.FormatThreadHtml(new ForumThreadEntity(), replyEntity.ForumPosts.ToList());
            Html = htmlThread;
            IsLoading = false;
        }
    }
}
