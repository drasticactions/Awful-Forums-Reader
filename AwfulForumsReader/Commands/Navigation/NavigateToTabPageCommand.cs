using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands.Navigation
{
    public class NavigateToTabPageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            App.RootFrame.Navigate(typeof (TabPage));
            await Locator.ViewModels.TabPageVm.Initialize();
        }
    }
}
