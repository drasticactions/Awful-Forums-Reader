using System;
using Windows.UI.Popups;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Manager;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Common;

namespace AwfulForumsReader.Commands.Bookmarks
{
    public class AddOrRemoveBookmarkCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var thread = parameter as ForumThreadEntity;
            if (thread == null)
                return;
            var threadManager = new ThreadManager();
            if (thread.IsBookmark)
            {
                await threadManager.RemoveBookmarkAsync(thread.ThreadId);
                thread.IsBookmark = !thread.IsBookmark;
                return;
            }
            thread.IsBookmark = !thread.IsBookmark;
            await threadManager.AddBookmarkAsync(thread.ThreadId);
            var msgDlg2 =
                   new MessageDialog(string.Format("'{0}' has been added to your bookmarks! I love you!{1}{2}",
                       thread.Name, Environment.NewLine, Constants.Ascii1))
                   {
                       DefaultCommandIndex = 1
                   };
            await msgDlg2.ShowAsync();
        }
    }
}
