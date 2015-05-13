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
            try
            {
                await Locator.ViewModels.BookmarksPageVm.Refresh();
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
