using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.ViewModels
{
    public class PreviewThreadPageViewModel : NotifierBase
    {
        private bool _isLoading;
        private readonly ThreadManager _threadManager = new ThreadManager();
        private string _html;
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

        public NewThreadEntity NewThreadEntity { get; set; }

        public async Task CreateThreadPreview(NewThreadEntity newThreadEntity)
        {
            IsLoading = true;
            NewThreadEntity = newThreadEntity;
            try
            {
                string result = await _threadManager.CreateNewThreadPreview(NewThreadEntity);
                if (string.IsNullOrEmpty(result))
                {
                    string messageText =
                    string.Format(
                        "No text?! What good is showing you a preview then! Type something in and try again!{0}{1}",
                        Environment.NewLine, Constants.Ascii2);
                    await AwfulDebugger.SendMessageDialogAsync(messageText, new Exception("No text in reply box"));
                    return;
                }

                Html = result;

            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Failed to get the preview html", ex);
            }
            IsLoading = false;
        }

        public async Task CreateReplyPreview(ForumReplyEntity forumReplyEntity, bool isEdit)
        {
            IsLoading = true;
            var replyManager = new ReplyManager();
            string result;
            if (isEdit)
            {
                result = await replyManager.CreatePreviewEditPost(forumReplyEntity);
            }
            else
            {
                result = await replyManager.CreatePreviewPost(forumReplyEntity);
            }
            try
            {
                if (!string.IsNullOrEmpty(result))
                {
                    Html = result;
                }
                else
                {
                    string messageText =
                        string.Format(
                            "No text?! What good is showing you a preview then! Type something in and try again!{0}{1}",
                            Environment.NewLine, Constants.Ascii2);
                    var msgDlg = new MessageDialog(messageText);
                    await msgDlg.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                AwfulDebugger.SendMessageDialogAsync("Failed to get the preview html", ex);
            }
            IsLoading = false;
        }
    }
}
