using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;
using AwfulForumsReader.Database.Commands;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands
{
    public class NavigateToBookmarksCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            
            if (parameter != null)
            {
                if (App.RootFrame.BackStackDepth < 0)
                {
                    App.RootFrame.Navigate(typeof(BookmarksPage));
                }
                var threadId = (long) parameter;
                var bookmarkManager = new BookmarkManager();
                var thread = await bookmarkManager.GetBookmarkThreadAsync(threadId);
                var command = new NavigateToThreadPageViaToastCommand();
                command.Execute(thread);
            }
            else
            {
                App.RootFrame.Navigate(typeof(BookmarksPage));
            }
            var threadViewModel = Locator.ViewModels.BookmarksPageVm;
            await threadViewModel.Initialize();
        }
    }
}
