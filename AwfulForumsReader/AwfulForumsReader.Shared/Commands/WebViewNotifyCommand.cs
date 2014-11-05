using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Core.Manager;
using AwfulForumsReader.Core.Tools;
using AwfulForumsReader.Database.Commands;
using AwfulForumsReader.Notification;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;
using Newtonsoft.Json;

namespace AwfulForumsReader.Commands
{
    public class WebViewNotifyCommand
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
                        //Frame.Navigate(typeof(ReplyView), command.Id);
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

                            //await _threadManager.MarkPostAsLastReadAs(_forumThread, Convert.ToInt32(command.Id));
                            int nextPost = Convert.ToInt32(command.Id) + 1;
                            await webview.InvokeScriptAsync("ScrollToDiv", new[] { string.Concat("#postId", nextPost.ToString()) });
                            NotifyStatusTile.CreateToastNotification("Post marked as last read! Now smash this computer and live your life!");
                        }
                        catch (Exception ex)
                        {
                            AwfulDebugger.SendMessageDialogAsync("Could not mark thread as last read", ex);
                        }
                        break;
                    case "setFont":
                        if (_localSettings.Values.ContainsKey("zoomSize"))
                        {
                            _zoomSize = Convert.ToInt32(_localSettings.Values["zoomSize"]);
                            webview.InvokeScriptAsync("ResizeWebviewFont", new[] { _zoomSize.ToString() });
                        }
                        else
                        {
                            // _zoomSize = 20;
                        }
                        break;
                    case "openThread":
                         var query = Extensions.ParseQueryString(command.Id);
                        if (query.ContainsKey("action") && query["action"].Equals("showPost"))
                        {
                            //var postManager = new PostManager();
                            //var html = await postManager.GetPost(Convert.ToInt32(query["postid"]));
                            return;
                        }
                        var threadManager = new ThreadManager();
                        var newThreadEntity = new ForumThreadEntity();
                        await threadManager.GetThreadInfo(newThreadEntity, command.Id);

                        var tabManager = new TabManager();
                        await tabManager.AddThreadToTabListAsync(newThreadEntity);
                        Locator.ViewModels.ThreadPageVm.LinkedThreads.Add(newThreadEntity);

                        Locator.ViewModels.ThreadPageVm.ForumThreadEntity = newThreadEntity;
                        Locator.ViewModels.ThreadPageVm.Html = null;
                        await Locator.ViewModels.ThreadPageVm.GetForumPostsAsync();
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
