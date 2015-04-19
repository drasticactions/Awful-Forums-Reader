using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;

namespace AwfulForumsReader.Commands
{
    public class RefreshBookmarkListCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            await Locator.ViewModels.BookmarksPageVm.Refresh();
        }
    }
}
