using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using AwfulForumsLibrary.Manager;
using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands.Navigation
{
    public class NavigateToLoginPageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            Locator.ViewModels.MainPageVm.IsReloggingIn = true;
            App.RootFrame.Navigate(typeof(LoginPage));
            App.RootFrame.BackStack.Clear();
        }
    }
}
