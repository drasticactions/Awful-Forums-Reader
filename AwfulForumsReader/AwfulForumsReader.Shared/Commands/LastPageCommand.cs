using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Database.Commands;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands
{
    public class LastPageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var thread = parameter as ForumThreadEntity;
            if (thread == null)
                return;
            thread.CurrentPage = thread.TotalPages;
            thread.RepliesSinceLastOpened = 0;
            App.RootFrame.Navigate(typeof(ThreadPage));
            var tabManager = new TabManager();
            await tabManager.RemoveAllThreadsFromTabList();
            await tabManager.AddThreadToTabListAsync(thread);
            var tabThreads = await tabManager.GetAllTabThreads();
            Locator.ViewModels.ThreadPageVm.LinkedThreads = tabThreads.ToObservableCollection();

            Locator.ViewModels.ThreadPageVm.ForumThreadEntity = thread;
            Locator.ViewModels.ThreadPageVm.Html = null;
            await Locator.ViewModels.ThreadPageVm.GetForumPostsAsync();
        }
    }
}
