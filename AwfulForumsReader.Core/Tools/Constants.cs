using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwfulForumsReader.Core.Tools
{
    public class Constants
    {
        public const string BookmarkBackground = "BookmarkBackground";

        public const string BookmarkStartup = "BookmarkStartup";

        public const string BookmarkDefault = "BookmarkDefault";

        public const string DarkMode = "DarkMode";

        public const string AutoRefresh = "AutoRefresh";

        public const string BaseUrl = "http://forums.somethingawful.com/";

        public const string BookmarksUrl = BaseUrl + "bookmarkthreads.php?perage=40&sortorder=desc&sortfield=";

        public const string CookieFile = "SACookie2.txt";

        public const string GotoNewPost = "&goto=newpost";

        public const string PerPage = "&perpage=40";

        public const string UserProfile = "member.php?action=getinfo&userid={0}";

        public const string UserRapSheet = "banlist.php?userid={0}";

        public const string RapSheet = "banlist.php?";

        public const string ForumListPage = "http://forums.somethingawful.com/forumdisplay.php?forumid=48";

        public const string ForumPage = BaseUrl + "forumdisplay.php?forumid={0}";

        public const string QuoteExp = "[quote=\"{0}\" post=\"{1}\"]{2}[/quote]";

        public const string ResetSeen = "action=resetseen&threadid={0}&json=1";

        public const string ResetBase = BaseUrl + "showthread.php";

        public const string Bookmark = BaseUrl + "bookmarkthreads.php";

        public const string LastRead = BaseUrl + "showthread.php?action=setseen&index={0}&threadid={1}";

        public const string RemoveBookmark = "json=1&action=remove&threadid={0}";

        public const string AddBookmark = "json=1&action=add&threadid={0}";

        public const string NewThread = BaseUrl + "newthread.php?action=newthread&forumid={0}";

        public const string NewPrivateMessage = BaseUrl + "private.php?action=newmessage";

        public const string NewPrivateMessageBase = BaseUrl + "private.php";

        public const string NewThreadBase = BaseUrl + "newthread.php";

        public const string NewReply = BaseUrl + "newreply.php";

        public const string EditPost = BaseUrl + "editpost.php";

        public const string ReplyBase = BaseUrl + "newreply.php?action=newreply&threadid={0}";

        public const string QuoteBase = BaseUrl + "newreply.php?action=newreply&postid={0}";

        public const string EditBase = BaseUrl + "editpost.php?action=editpost&postid={0}";

        public const string UserPostHistory = BaseUrl + "search.php?action=do_search_posthistory&userid={0}";

        public const string PrivateMessages = BaseUrl + "private.php";

        public const string PageNumber = "&pagenumber={0}";

        public const string ThreadPage = BaseUrl + "showthread.php?threadid={0}";

        public const string FrontPage = "http://www.somethingawful.com";

        public const string SmileUrl = BaseUrl + "misc.php?action=showsmilies";

        public const string ShowPost = BaseUrl + "showthread.php?action=showpost&postid={0}";

        public const string UserCp = "usercp.php?";

        public const string HtmlFile = "{0}.html";

        public const string HtmlHeader =
            "<head><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no\"/><link href=\"ms-appx-web:///Assets/forum-thread.css\" media=\"all\" rel=\"stylesheet\" type=\"text/css\"><link rel=\"stylesheet\" type=\"text/css\" href=\"ms-appx-web:///Assets/bbcode.css\"><link rel=\"stylesheet\" type=\"text/css\" href=\"ms-appx-web:///Assets/forums.css\"><link rel=\"stylesheet\" type=\"text/css\" href=\"ms-appx-web:///Assets/main.css\"><link rel=\"stylesheet\" type=\"text/css\" href=\"ms-appx-web:///Assets/ui-light-new.css\"><meta name=\"MSSmartTagsPreventParsing\" content=\"TRUE\"><meta http-equiv=\"X-UA-Compatible\" content=\"chrome=1\"><script type=\"text/javascript\" src=\"ms-appx-web:///Assets/jquery.min.js\"></script><link rel=\"stylesheet\" type=\"text/css\" href=\"ms-appx-web:///Assets/jquery-ui.css\"><script type=\"text/javascript\" src=\"ms-appx-web:///Assets/jquery-ui.min.js\"></script><script type=\"text/javascript\">disable_thread_coloring = true;</script><script type=\"text/javascript\" src=\"ms-appx-web:///Assets/forums.combined.js\"></script><style type=\"text/css\"></style></head>";

        public const int DefaultTimeoutInMilliseconds = 60000;

        public const string CookieDomainUrl = "http://fake.forums.somethingawful.com";

        public const string LoginUrl = "http://forums.somethingawful.com/account.php?";

        // SA only seems to accept a limit of 30, so we'll hard code it.
        public const string SearchUrl = BaseUrl + "/f/json/usercomplete?q={0}&limit=30&timestamp=0";

        public const string Ascii2 = @"☆ *　. 　☆ 
　　☆　. ∧＿∧　∩　* ☆ 
ｷﾀ━━━( ・∀・)/ . ━━━君はバカだな！！ 
　　　. ⊂　　 ノ* ☆ 
　　☆ * (つ ノ .☆ 
　　　　 (ノ";

        public const string Ascii1 = @"Δ~~~~Δ 
ξ ･ェ･ ξ 
ξ　~　ξ 
ξ　　 ξ 
ξ　　　“~～~～〇 
ξ　　　　　　 ξ 
ξ　ξ　ξ~～~ξ　ξ 
　ξ_ξξ_ξ　ξ_ξξ_ξ 
　　ヽ(´･ω･)ﾉ 
　　　 |　 / 
　　　 UU";
        public const string Ascii3 = @"＼う～寒ぃ…/
　￣￣∨￣￣
　　　∧,,∧
　　／ミ;ﾟДﾟ彡
　//＼￣￣￣＼
／/※.＼＿＿＿＼
＼＼ ※ ※ ※ ※ ヽ
　＼ ───────────ヽ";

        public const string Ascii4 = @"     ∧_∧    「何してるの？」
　（ ´･ω･) 
　//＼￣￣旦＼ 
／/ ※ ＼＿＿＿＼ 
＼＼ 　※ 　 ∧_∧ ヽ 
　 ＼ヽ＿／( ´･ω･)_ヽ「AwfulReaderを使ってるよー」";
        public const string Ascii5 = @"　　　　　　　＿ 
　　　⋀⋀　　 /　| 
　＿/(・ω・)／●. | 
!/　.}￣￣￣ 　　/ 
i＼_}／￣|＿_／≡＝ 
　　`￣￣~❤ 
　　　　　　～❤ 
　　　　　　　　～❤ 
　　　　　　　　　　～❤ 
　　　　　　　　　　　　～❤";
    }
}
