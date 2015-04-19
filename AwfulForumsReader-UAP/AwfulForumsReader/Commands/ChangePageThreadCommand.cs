using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Popups;
using AwfulForumsReader.Common;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands
{
    public class ChangePageThreadCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var vm = Locator.ViewModels.ThreadPageVm;
            var page = Locator.ViewModels.ThreadPageVm.PageSelection;
            if (string.IsNullOrEmpty(page))
            {
                return;
            }

            int userInputPageNumber;
            try
            {
                userInputPageNumber = Convert.ToInt32(page);
            }
            catch (Exception)
            {
                // User entered invalid number, return.
                return;
            }

            if (userInputPageNumber < 1 || userInputPageNumber > vm.ForumThreadEntity.TotalPages) return;
            vm.ForumThreadEntity.CurrentPage = userInputPageNumber;
            vm.ForumThreadEntity.ScrollToPost = 0;
            vm.ForumThreadEntity.ScrollToPostString = string.Empty;
            await vm.GetForumPostsAsync();
        }
    }
}
