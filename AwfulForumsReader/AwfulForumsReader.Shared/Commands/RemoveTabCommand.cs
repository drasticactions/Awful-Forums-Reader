using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Database.Commands;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands
{
    public class RemoveTabCommand : AlwaysExecutableCommand
    {
        
        public async override void Execute(object parameter)
        {
            var args = parameter as RoutedEventArgs;
            if (args == null)
            {
                AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", new Exception("Arguments are null"));
                return;
            }
            
            var appButton = args.OriginalSource as AppBarButton;
            var thread = appButton.DataContext as ForumThreadEntity;
            if (thread == null)
                return;
            var tabManager = new TabManager();
            if (Locator.ViewModels.ThreadPageVm.LinkedThreads.Count <= 1)
            {
                App.RootFrame.GoBack();
                Locator.ViewModels.ThreadPageVm.LinkedThreads = null;
                await tabManager.RemoveAllThreadsFromTabList();
                return;
            }

            if (Locator.ViewModels.ThreadPageVm.ForumThreadEntity == thread)
            {
                Locator.ViewModels.ThreadPageVm.LinkedThreads.Remove(thread);
                Locator.ViewModels.ThreadPageVm.ForumThreadEntity = Locator.ViewModels.ThreadPageVm.LinkedThreads.First();
                await Locator.ViewModels.ThreadPageVm.GetForumPostsAsync();
                return;
            }

            Locator.ViewModels.ThreadPageVm.LinkedThreads.Remove(thread);
        }
    }
}
