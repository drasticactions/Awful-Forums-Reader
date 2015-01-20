using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands
{
    public class NavigateToSaclopedia : AlwaysExecutableCommand
    {
        public override async void Execute(object parameter)
        {
            App.RootFrame.Navigate(typeof (SaclopediaPage));
            await Locator.ViewModels.SaclopediaPageVm.Initialize();
        }
    }
}
