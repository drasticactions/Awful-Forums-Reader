using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Manager;

namespace AwfulForumsReader.Commands
{
    public class AddIgnoredUserPostCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            try
            {
                var collection = (WebViewCollection)parameter;
                var postId = ParsePostId(collection.PostId);
                var postManager = new PostManager();
                var post = await postManager.GetPost(postId);
                collection.WebView.InvokeScriptAsync("AddPostToThread", new[] { postId.ToString(), post.PostHtml });
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get post", ex);
            }
        }

        private static int ParsePostId(string txt)
        {
            const string re1 = ".*?"; // Non-greedy match on filler
            const string re2 = "\\d+"; // Uninteresting: int
            const string re3 = ".*?"; // Non-greedy match on filler
            const string re4 = "(\\d+)"; // Integer Number 1

            var r = new Regex(re1 + re2 + re3 + re4, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            var m = r.Match(txt);
            if (!m.Success) return 0;
            var int1 = m.Groups[1].ToString();
            return Convert.ToInt32(int1);
        }
    }
}
