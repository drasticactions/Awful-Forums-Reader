using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
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
    public class WebViewCollection
    {
        public WebView WebView { get; set; }
        public string PostId { get; set; }
    }

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
                    case "showPosts":
                        await webview.InvokeScriptAsync("ShowHiddenPosts", new[] {string.Empty});
                        break;
                    case "profile":
                        //Frame.Navigate(typeof(UserProfileView), command.Id);
                        break;
                    case "openPost":
                        var addIgnoredUserPostCommand = new AddIgnoredUserPostCommand();
                        var test = new WebViewCollection()
                        {
                            PostId = command.Id,
                            WebView = webview
                        };
                        try
                        {
                            addIgnoredUserPostCommand.Execute(test);
                        }
                        catch (Exception ex)
                        {
                            AwfulDebugger.SendMessageDialogAsync("Error getting post", ex);
                        }
                        break;
                    case "post_history":
                        //Frame.Navigate(typeof(UserPostHistoryPage), command.Id);
                        break;
                    case "rap_sheet":
                        //Frame.Navigate(typeof(RapSheetView), command.Id);
                        break;
                    case "quote":
                        var navigateToNewReplyViaQuoteCommand = new NavigateToNewReplyViaQuoteCommand();
                        navigateToNewReplyViaQuoteCommand.Execute(command.Id);
                        break;
                    case "edit":
                        var navigateToEditPostPageCommand = new NavigateToEditPostPageCommand();
                        navigateToEditPostPageCommand.Execute(command.Id);
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
                        Locator.ViewModels.ThreadPageVm.IsLoading = true;
                        var newThreadEntity = new ForumThreadEntity()
                        {
                            Location = command.Id,
                            ImageIconLocation = "/Assets/ThreadTags/noicon.png"
                        };
                        Locator.ViewModels.ThreadPageVm.ForumThreadEntity = newThreadEntity;

                        await Locator.ViewModels.ThreadPageVm.GetForumPostsAsync();

                        var tabManager = new TabManager();
                        await tabManager.AddThreadToTabListAsync(newThreadEntity);
                        Locator.ViewModels.ThreadPageVm.LinkedThreads.Add(newThreadEntity);
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
