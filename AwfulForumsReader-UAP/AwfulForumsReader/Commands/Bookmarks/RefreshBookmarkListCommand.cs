using AwfulForumsReader.Common;

namespace AwfulForumsReader.Commands.Bookmarks
{
    public class RefreshBookmarkListCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            await Locator.ViewModels.BookmarksPageVm.Refresh();
        }
    }
}
