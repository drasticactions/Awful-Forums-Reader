using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Database.Commands;
using AwfulForumsReader.Notification;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;
using Newtonsoft.Json;

namespace AwfulForumsReader.Commands
{
    public class WebViewLastPostNotifyCommand
    {
        private static int _zoomSize;
        private static readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
        
        public static async void WebView_ScriptNotify(object sender, NotifyEventArgs e)
        {
            var webview = sender as WebView;
            var threadEntity = Locator.ViewModels.ThreadPageVm.ForumThreadEntity;
            if (webview == null || threadEntity == null)
            {
                return;
            }

            try
            {
                string stringJson = e.Value;
                var command = JsonConvert.DeserializeObject<ThreadCommand>(stringJson);
                switch (command.Command)
                {
                    case "showPosts":
                        await webview.InvokeScriptAsync("ShowHiddenPosts", new[] {string.Empty});
                        break;
                    case "profile":
                        //Frame.Navigate(typeof(UserProfileView), command.Id);
                        break;
                    case "openPost":
                        break;
                    case "post_history":
                        //Frame.Navigate(typeof(UserPostHistoryPage), command.Id);
                        break;
                    case "rap_sheet":
                        //Frame.Navigate(typeof(RapSheetView), command.Id);
                        break;
                    case "quote":
                        try
                        {
                            var replyManager = new ReplyManager();
                            string quoteString = await replyManager.GetQuoteString(Convert.ToInt64(command.Id));
                            quoteString = string.Concat(Environment.NewLine, quoteString);
                            string replyText = string.IsNullOrEmpty(Locator.ViewModels.LastPostPageVm.ReplyBox.Text) ? string.Empty : Locator.ViewModels.LastPostPageVm.ReplyBox.Text;
                            if (replyText != null) Locator.ViewModels.NewThreadReplyVm.PostBody = replyText.Insert(Locator.ViewModels.LastPostPageVm.ReplyBox.Text.Length, quoteString);
                            App.RootFrame.GoBack();
                        }
                        catch(Exception ex)
                        {
                            AwfulDebugger.SendMessageDialogAsync("Failed to quote post", ex);
                        }
                        break;
                    case "edit":
                        //Frame.Navigate(typeof(EditReplyPage), command.Id);
                        break;
                    case "scrollToPost":
                        try
                        {
                            if (command.Id != null)
                            {
                                await webview.InvokeScriptAsync("ScrollToDiv", new[] { string.Concat("#postId", command.Id) });
                            }
                            else if (!string.IsNullOrEmpty(threadEntity.ScrollToPostString))
                            {
                                webview.InvokeScriptAsync("ScrollToDiv", new[] { threadEntity.ScrollToPostString });
                            }
                        }
                        catch (Exception)
                        {
                            Debug.WriteLine("Could not scroll to post...");
                        }
                        break;
                    case "markAsLastRead":
                        try
                        {
                            var threadManager = new ThreadManager();
                            await threadManager.MarkPostAsLastReadAs(Locator.ViewModels.ThreadPageVm.ForumThreadEntity, Convert.ToInt32(command.Id));
                            int nextPost = Convert.ToInt32(command.Id) + 1;
                            await webview.InvokeScriptAsync("ScrollToDiv", new[] { string.Concat("#postId", nextPost.ToString()) });
                            NotifyStatusTile.CreateToastNotification("Last Read", "Post marked as last read.");
                        }
                        catch (Exception ex)
                        {
                            AwfulDebugger.SendMessageDialogAsync("Could not mark thread as last read", ex);
                        }
                        break;
                    case "setFont":

                        break;
                    case "openThread":
                       
                        break;
                    default:
                        var msgDlg = new MessageDialog("Not working yet!")
                        {
                            DefaultCommandIndex = 1
                        };
                        await msgDlg.ShowAsync();
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
