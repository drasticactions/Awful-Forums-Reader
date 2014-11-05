using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;

namespace AwfulForumsReader.Commands
{
    public class ForwardThreadPageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var vm = Locator.ViewModels.ThreadPageVm;
            if (vm.ForumThreadEntity.CurrentPage >= vm.ForumThreadEntity.TotalPages) return;
            vm.ForumThreadEntity.CurrentPage++;
            vm.ForumThreadEntity.ScrollToPost = 0;
            vm.ForumThreadEntity.ScrollToPostString = string.Empty;
            await Locator.ViewModels.ThreadPageVm.GetForumPostsAsync();
        }
    }
}
