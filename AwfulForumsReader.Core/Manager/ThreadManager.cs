using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Core.Exceptions;
using AwfulForumsReader.Core.Interface;
using AwfulForumsReader.Core.Tools;
using HtmlAgilityPack;

namespace AwfulForumsReader.Core.Manager
{
    public class ThreadManager
    {
        private readonly IWebManager _webManager;

        public ThreadManager(IWebManager webManager)
        {
            _webManager = webManager;
        }

        public ThreadManager() : this(new WebManager())
        {
        }

        public async Task<bool> AddBookmarkAsync(long threadId)
        {
            await _webManager.PostData(
                    Constants.Bookmark, string.Format(
                        Constants.AddBookmark, threadId
                        ));
            return true;
        }

        public async Task<bool> RemoveBookmarkAsync(long threadId)
        {
            await _webManager.PostData(
                    Constants.Bookmark, string.Format(
                        Constants.RemoveBookmark, threadId
                        ));
            return true;
        }

        public async Task<ObservableCollection<ForumThreadEntity>> GetBookmarksAsync(ForumEntity forumCategory, int page)
        {
            var forumThreadList = new ObservableCollection<ForumThreadEntity>();
            String url = Constants.BookmarksUrl;
            if (forumCategory.CurrentPage >= 0)
            {
                url = Constants.BookmarksUrl + string.Format(Constants.PageNumber, page);
            }

            HtmlDocument doc = (await _webManager.GetData(url)).Document;

            HtmlNode forumNode =
                doc.DocumentNode.Descendants()
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("threadlist"));


            foreach (
                HtmlNode threadNode in
                    forumNode.Descendants("tr")
                        .Where(node => node.GetAttributeValue("class", string.Empty).StartsWith("thread")))
            {
                var threadEntity = new ForumThreadEntity { ForumId = forumCategory.ForumId, ForumEntity = forumCategory, IsBookmark = true};
                ParseThreadHtml(threadEntity, threadNode);
                forumThreadList.Add(threadEntity);
            }
            return forumThreadList;
        }

        public async Task<ObservableCollection<ForumThreadEntity>> GetForumThreadsAsync(ForumEntity forumCategory, int page)
        {
            string url = forumCategory.Location + string.Format(Constants.PageNumber, page);

            HtmlDocument doc = (await _webManager.GetData(url)).Document;

            HtmlNode forumNode =
                doc.DocumentNode.Descendants()
                    .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("threadlist"));
            var forumThreadList = new ObservableCollection<ForumThreadEntity>();
            foreach (
                HtmlNode threadNode in
                    forumNode.Descendants("tr")
                        .Where(node => node.GetAttributeValue("class", string.Empty).StartsWith("thread")))
            {
                var threadEntity = new ForumThreadEntity {ForumId = forumCategory.ForumId, ForumEntity = forumCategory};
                ParseThreadHtml(threadEntity, threadNode);
                forumThreadList.Add(threadEntity);
            }
            return forumThreadList;
        }

        public async Task<NewThreadEntity> GetThreadCookiesAsync(long forumId)
        {
            try
            {
                string url = string.Format(Constants.NewThread, forumId);
                WebManager.Result result = await _webManager.GetData(url);
                HtmlDocument doc = result.Document;

                HtmlNode[] formNodes = doc.DocumentNode.Descendants("input").ToArray();

                HtmlNode formKeyNode =
                    formNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("formkey"));

                HtmlNode formCookieNode =
                    formNodes.FirstOrDefault(node => node.GetAttributeValue("name", "").Equals("form_cookie"));

                var newForumEntity = new NewThreadEntity();
                try
                {
                    string formKey = formKeyNode.GetAttributeValue("value", "");
                    string formCookie = formCookieNode.GetAttributeValue("value", "");
                    newForumEntity.FormKey = formKey;
                    newForumEntity.FormCookie = formCookie;
                    return newForumEntity;
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException(string.Format("Could not parse new thread form data. {0}", exception));
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void ParseHasSeenThread(ForumThreadEntity threadEntity, HtmlNode threadNode)
        {
            threadEntity.HasSeen = threadNode.GetAttributeValue("class", string.Empty).Contains("seen");
        }

        private void ParseThreadTitleAnnouncement(ForumThreadEntity threadEntity, HtmlNode threadNode)
        {
            var titleNode = threadNode.Descendants("a")
               .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("thread_title")) ??
                           threadNode.Descendants("a")
                   .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("announcement"));

            threadEntity.IsAnnouncement = titleNode != null &&
                titleNode.GetAttributeValue("class", string.Empty).Equals("announcement");

            threadEntity.Name =
                titleNode != null ? WebUtility.HtmlDecode(titleNode.InnerText) : "BLANK TITLE?!?";
        }

        private void ParseThreadKilledBy(ForumThreadEntity threadEntity, HtmlNode threadNode)
        {
            threadEntity.KilledBy =
                threadNode.Descendants("a")
                    .LastOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("author")) != null ? threadNode.Descendants("a")
                    .LastOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("author")).InnerText : string.Empty;
        }

        private void ParseThreadIsSticky(ForumThreadEntity threadEntity, HtmlNode threadNode)
        {
            threadEntity.IsSticky =
                threadNode.Descendants("td")
                    .Any(node => node.GetAttributeValue("class", string.Empty).Contains("title_sticky"));
        }

        private void ParseThreadIsLocked(ForumThreadEntity threadEntity, HtmlNode threadNode)
        {
            threadEntity.IsLocked = threadNode.GetAttributeValue("class", string.Empty).Contains("closed");
        }

        private void ParseThreadCanMarkAsUnread(ForumThreadEntity threadEntity, HtmlNode threadNode)
        {
            threadEntity.CanMarkAsUnread =
                threadNode.Descendants("a").Any(node => node.GetAttributeValue("class", string.Empty).Equals("x"));
        }

        private void ParseThreadAuthor(ForumThreadEntity threadEntity, HtmlNode threadNode)
        {
            threadEntity.Author =
               threadNode.Descendants("td")
                   .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("author"))
                   .InnerText;
        }

        private void ParseThreadRepliesSinceLastOpened(ForumThreadEntity threadEntity, HtmlNode threadNode)
        {
            if (threadNode.Descendants("a").Any(node => node.GetAttributeValue("class", string.Empty).Equals("count")))
            {
                threadEntity.RepliesSinceLastOpened =
                    Convert.ToInt32(
                        threadNode.Descendants("a")
                            .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("count"))
                            .InnerText);
            }
        }

        private void ParseThreadReplyCount(ForumThreadEntity threadEntity, HtmlNode threadNode)
        {
            try
            {
                threadEntity.ReplyCount =
                threadNode.Descendants("td")
                    .Any(node => node.GetAttributeValue("class", string.Empty).Contains("replies"))
                    ? Convert.ToInt32(
                        threadNode.Descendants("td")
                            .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("replies"))
                            .InnerText)
                    : 1;
            }
            catch (Exception)
            {
                threadEntity.ReplyCount = 0;
            }
        }

        private void ParseThreadViewCount(ForumThreadEntity threadEntity, HtmlNode threadNode)
        {
            try
            {
                threadEntity.ViewCount =
               threadNode.Descendants("td")
                   .Any(node => node.GetAttributeValue("class", string.Empty).Contains("views"))
                   ? Convert.ToInt32(
                       threadNode.Descendants("td")
                           .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("views"))
                           .InnerText)
                   : 1;
            }
            catch (Exception)
            {
                threadEntity.ViewCount = 0;
            }
        }

        private void ParseThreadRating(ForumThreadEntity threadEntity, HtmlNode threadNode)
        {
            threadEntity.RatingImage =
    threadNode.Descendants("td")
        .Any(node => node.GetAttributeValue("class", string.Empty).Contains("rating")) &&
    threadNode.Descendants("td")
        .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("rating"))
        .Descendants("img")
        .Any()
        ? threadNode.Descendants("td")
            .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("rating"))
            .Descendants("img")
            .FirstOrDefault()
            .GetAttributeValue("src", string.Empty)
        : null;
        }

        private void ParseThreadTotalPages(ForumThreadEntity threadEntity)
        {
            threadEntity.TotalPages = (threadEntity.ReplyCount / 40) + 1;
        }

        private void ParseThreadId(ForumThreadEntity threadEntity, HtmlNode threadNode)
        {
            var titleNode = threadNode.Descendants("a")
              .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("thread_title")) ??
                          threadNode.Descendants("a")
                  .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("announcement"));

            if (titleNode == null) return;

            threadEntity.Location = Constants.BaseUrl +
                                    titleNode.GetAttributeValue("href", string.Empty) + Constants.PerPage;

            threadEntity.ThreadId =
                Convert.ToInt64(
                    titleNode
                        .GetAttributeValue("href", string.Empty)
                        .Split('=')[1]);
        }

        private void ParseThreadIcon(ForumThreadEntity threadEntity, HtmlNode threadNode)
        {
            HtmlNode first =
               threadNode.Descendants("td")
                   .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("icon"));
            if (first != null)
            {
                var testImageString = first.Descendants("img").FirstOrDefault().GetAttributeValue("src", string.Empty); ;
                if (!string.IsNullOrEmpty(testImageString))
                {
                    threadEntity.ImageIconLocation = string.Format("/Assets/ThreadTags/{0}.png", Path.GetFileNameWithoutExtension(testImageString));
                }
            }
        }

        private void ParseStoreThreadIcon(ForumThreadEntity threadEntity, HtmlNode threadNode)
        {
            HtmlNode second =
    threadNode.Descendants("td")
        .FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Equals("icon2"));
            if (second == null) return;
            try
            {
                var testImageString = second.Descendants("img").FirstOrDefault().GetAttributeValue("src", string.Empty);
                if (!string.IsNullOrEmpty(testImageString))
                {
                    threadEntity.StoreImageIconLocation = string.Format("/Assets/ThreadTags/{0}.png", Path.GetFileNameWithoutExtension(testImageString));
                }
            }
            catch (Exception)
            {
                threadEntity.StoreImageIconLocation = null;
            }
        }

        private void ParseThreadHtml(ForumThreadEntity threadEntity, HtmlNode threadNode)
        {
            try
            {
                ParseHasSeenThread(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new ForumListParsingFailedException(string.Format("Failed to parse 'Has Seen' element {0}", exception));
            }

            try
            {
                ParseThreadTitleAnnouncement(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new ForumListParsingFailedException(string.Format("Failed to parse 'Thread/Announcement' element {0}", exception));
            }

            try
            {
                ParseThreadKilledBy(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new ForumListParsingFailedException(string.Format("Failed to parse 'Killed By' element {0}", exception));
            }

            try
            {
                ParseThreadIsSticky(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new ForumListParsingFailedException(string.Format("Failed to parse 'Is Thread Sticky' element {0}", exception));
            }

            try
            {
                ParseThreadIsLocked(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new ForumListParsingFailedException(string.Format("Failed to parse 'Thread Locked' element {0}", exception));
            }

            try
            {
                ParseThreadCanMarkAsUnread(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new ForumListParsingFailedException(string.Format("Failed to parse 'Can mark as thread as unread' element {0}", exception));
            }

            try
            {
                threadEntity.HasBeenViewed = threadEntity.CanMarkAsUnread;
            }
            catch (Exception exception)
            {
                throw new ForumListParsingFailedException(string.Format("Failed to parse 'Has Been Viewed' element {0}", exception));
            }

            try
            {
                ParseThreadAuthor(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new ForumListParsingFailedException(string.Format("Failed to parse 'Thread Author' element {0}", exception));
            }

            try
            {
                ParseThreadRepliesSinceLastOpened(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new ForumListParsingFailedException(string.Format("Failed to parse 'Replies since last opened' element {0}", exception));
            }

            try
            {
                ParseThreadReplyCount(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new ForumListParsingFailedException(string.Format("Failed to parse 'Reply count' element {0}", exception));
            }

            try
            {
                ParseThreadViewCount(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new ForumListParsingFailedException(string.Format("Failed to parse 'View Count' element {0}", exception));
            }

            try
            {
                ParseThreadRating(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new ForumListParsingFailedException(string.Format("Failed to parse 'Thread Rating' element {0}", exception));
            }

            try
            {
                ParseThreadTotalPages(threadEntity);
            }
            catch (Exception exception)
            {
                throw new ForumListParsingFailedException(string.Format("Failed to parse 'Total Pages' element {0}", exception));
            }

            try
            {
                ParseThreadId(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new ForumListParsingFailedException(string.Format("Failed to parse 'Thread Id' element {0}", exception));
            }

            try
            {
                ParseThreadIcon(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new ForumListParsingFailedException(string.Format("Failed to parse 'Thread Icon' element {0}", exception));
            }

            try
            {
                ParseStoreThreadIcon(threadEntity, threadNode);
            }
            catch (Exception exception)
            {
                throw new ForumListParsingFailedException(string.Format("Failed to parse 'Store thread icon' element {0}", exception));
            }
        }

        public async Task<bool> MarkThreadUnreadAsync(long threadId)
        {
            await _webManager.PostData(
                    Constants.ResetBase, string.Format(
                        Constants.ResetSeen, threadId
                        ));
            return true;
        }
    }
}
