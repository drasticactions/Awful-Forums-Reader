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
            App.RootFrame.Navigate(typeof(BookmarksPage));
            if (parameter != null)
            {
                var threadId = (long) parameter;
                var bookmarkManager = new BookmarkManager();
                var thread = await bookmarkManager.GetBookmarkThreadAsync(threadId);
                var command = new NavigateToThreadPageViaToastCommand();
                command.Execute(thread);
            }
            var threadViewModel = Locator.ViewModels.BookmarksPageVm;
            await threadViewModel.Initialize();
        }
    }
}
