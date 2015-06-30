using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Common;
using AwfulForumsReader.Database;
using AwfulForumsReader.Tools;
using AwfulForumsReader.ViewModels;

namespace AwfulForumsReader.Commands.Threads
{
    public class RemoveTabCommand : AlwaysExecutableCommand
    {
        
        public async override void Execute(object parameter)
        {
            var args = parameter as ForumThreadEntity;
            if (args == null)
            {
                await AwfulDebugger.SendMessageDialogAsync("Thread navigation failed!:(", new Exception("Arguments are null"));
                return;
            }

            Locator.ViewModels.TabPageVm.TabThreads.Remove(args);

            var tabManager = new MainForumsDatabase();
            await tabManager.RemoveThreadFromTabListAsync(args);

            var tabs = await tabManager.GetAllTabThreads();
            if (tabs.Any())
            {
                return;
            }
            Locator.ViewModels.TabPageVm.IsEmpty = true;
        }
    }
}
