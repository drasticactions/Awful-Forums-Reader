using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;
using AwfulForumsReader.Database.Commands;

namespace AwfulForumsReader.Commands
{
    public class NavigateToLastForumPageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var subforumManager = new SubforumManager();
            var forum = await subforumManager.ReturnLastForumEntity();
            if (forum != null)
            {
                var threadViewModel = Locator.ViewModels.ThreadListPageVm;
                threadViewModel.Initialize(forum);
            }
            App.RootFrame.GoBack();
            if (forum != null)
            {
                await subforumManager.RemoveLastEntry(forum);
            }
        }
    }
}
