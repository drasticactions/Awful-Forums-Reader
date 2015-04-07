using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using AwfulForumsLibrary;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Tools;
using HtmlAgilityPack;

namespace AwfulForumsReader.Tools
{
    public static class HtmlFormater
    {
        public static async Task<string> FormatPostHtml(ForumPostEntity forumPostEntity)
        {
            return string.Format("<div class=\"postbody\">{0}</div>", forumPostEntity.PostHtml);
        }

        public static async Task<string> FormatSaclopediaEntry(string body, PlatformIdentifier platformIdentifier)
        {
            string html = await PathIO.ReadTextAsync("ms-appx:///Assets/Website/saclopedia.html");

            var doc2 = new HtmlDocument();

            doc2.LoadHtml(html);

            HtmlNode head = doc2.DocumentNode.Descendants("head").FirstOrDefault();

            switch (platformIdentifier)
            {
                case PlatformIdentifier.WindowsPhone:
                    head.InnerHtml += "<link href=\"ms-appx-web:///Assets/Website/CSS/WindowsPhone-Default.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
            }

            HtmlNode bodyNode = doc2.DocumentNode.Descendants("body").FirstOrDefault();

            bodyNode.InnerHtml = WebUtility.HtmlDecode(body);

            return doc2.DocumentNode.OuterHtml;
        }

        public static async Task<string> FormatThreadHtml(ForumThreadEntity forumThreadEntity, List<ForumPostEntity> postEntities)
        {
            string html = await PathIO.ReadTextAsync("ms-appx:///Assets/Website/thread.html");

            var doc2 = new HtmlDocument();

            doc2.LoadHtml(html);


            HtmlNode head = doc2.DocumentNode.Descendants("head").FirstOrDefault();

            switch (forumThreadEntity.PlatformIdentifier)
            {
                case PlatformIdentifier.WindowsPhone:
                    head.InnerHtml += "<link href=\"ms-appx-web:///Assets/Website/CSS/WindowsPhone-Default.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
            }

            switch (forumThreadEntity.ForumId)
            {
                case 219:
                    head.InnerHtml += "<link href=\"ms-appx-web:///Assets/Website/CSS/219.css\" type=\"text/css\" media=\"all\" rel=\"stylesheet\">";
                    break;
                case 26:
                    break;

            }

            HtmlNode bodyNode = doc2.DocumentNode.Descendants("div").FirstOrDefault(node => node.GetAttributeValue("class", string.Empty).Contains("row clearfix"));

            if (postEntities == null) return WebUtility.HtmlDecode(WebUtility.HtmlDecode(doc2.DocumentNode.OuterHtml));

            string threadHtml = string.Empty;

            if (forumThreadEntity.ScrollToPost > 1)
            {
                threadHtml = "<div><div id=\"showPosts\">";

                var clickHandler = $"window.ForumCommand('showPosts', '{"true"}')";

                string showThreadsButton = HtmlButtonBuilder.CreateSubmitButton(
                    $"Show {forumThreadEntity.ScrollToPost} Previous Posts", clickHandler, "showHiddenPostsButton");

                threadHtml += showThreadsButton;

                threadHtml += "</div><div style=\"display: none;\" id=\"hiddenPosts\">";
                threadHtml += ParsePosts(0, forumThreadEntity.ScrollToPost, postEntities, forumThreadEntity.IsPrivateMessage);
                threadHtml += "</div>";
                threadHtml += ParsePosts(forumThreadEntity.ScrollToPost, postEntities.Count, postEntities, forumThreadEntity.IsPrivateMessage);
            }
            else
            {
                threadHtml += ParsePosts(0, postEntities.Count, postEntities, forumThreadEntity.IsPrivateMessage);
            }

            bodyNode.InnerHtml = threadHtml;
            return doc2.DocumentNode.OuterHtml;
        }

        private static string ParsePosts(int startingCount, int endCount, List<ForumPostEntity> postEntities, bool isPrivateMessage)
        {
            int seenCount = 1;
            string threadHtml = string.Empty;
            for (int index = startingCount; index < endCount; index++)
            {
                ForumPostEntity post = postEntities[index];
                if (seenCount > 2)
                    seenCount = 1;
                string hasSeen = post.HasSeen ? string.Concat("seen", seenCount) : string.Concat("postCount", seenCount);
                seenCount++;
                string userAvatar = string.Empty;
                if (!string.IsNullOrEmpty(post.User.AvatarLink))
                    userAvatar = string.Concat("<img data-user-id=\"", post.User.Id, "\" src=\"", post.User.AvatarLink,
                        "\" alt=\"\" class=\"av\" border=\"0\">");
                string username =
                    $"<p class=\"text article-title win-type-ellipsis\"><span class=\"author\">{post.User.Username}</span></p>";
                string postData =
                    $"<p class=\"text article-title text-muted win-type-caption-alt\"><span class=\"registered\">{post.PostDate}</span></p>";
                string postBody = string.Format("<div id=\"{1}\" class=\"postbody\">{0}</div>", post.PostHtml, post.PostId);
                string userInfo = $"<div class=\"userinfo\">{username}{postData}</div>";
                string postButtons = CreateButtons(post);

                string footer = string.Empty;
                if (!isPrivateMessage)
                {
                    footer = $"<tr class=\"postbar\"><td class=\"postlinks\">{postButtons}</td></tr>";
                }
                threadHtml +=
                    string.Format(
                        "<div class={6} id={4}>" +
                        "<div id={5}>" +
                        "<div class=\"row clearfix\">" +
                        "<div class=\"col-md-2\">" +
                        "<div id=\"threadView\">" +
                        "{0}{1}" +
                        "</div>" +
                        "</div>" +
                        "<div style=\"padding: 15px;\" class=\"col-md-10\">" +
                        "<div class=\"article-content\">" +
                        "{2}" +
                        "<footer>{3}</footer>" +
                        "</div>" +
                        "</div>" +
                        "</div>" +
                        "</div>" +
                        "</div>",
                        userAvatar, userInfo, postBody, footer, string.Concat("\"pti", index + 1, "\""), string.Concat("\"postId", post.PostId, "\""), string.Concat("\"", hasSeen, "\""));
            }
            return threadHtml;
        }

        private static string CreateButtons(ForumPostEntity post)
        {
            var clickHandler = $"window.ForumCommand('quote', '{post.PostId}')";

            string quoteButton = HtmlButtonBuilder.CreateSubmitButton("Quote", clickHandler, string.Empty);

            clickHandler = $"window.ForumCommand('edit', '{post.PostId}')";

            string editButton = HtmlButtonBuilder.CreateSubmitButton("Edit", clickHandler, string.Empty);

            clickHandler = $"window.ForumCommand('markAsLastRead', '{post.PostIndex}')";

            string markAsLastReadButton = HtmlButtonBuilder.CreateSubmitButton("Last Read", clickHandler, string.Empty);

            return post.User.IsCurrentUserPost
                    ? string.Concat("<ul class=\"profilelinks\">",
                        quoteButton, markAsLastReadButton, editButton, "</ul>")
                    : string.Concat("<ul class=\"profilelinks\">",
                        quoteButton, markAsLastReadButton, "</ul>");
        }
    }
}
