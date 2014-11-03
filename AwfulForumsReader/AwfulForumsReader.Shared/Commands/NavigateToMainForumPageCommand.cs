using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands
{
    public class NavigateToMainForumPageCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            App.RootFrame.Navigate(typeof(MainForumsPage));
        }
    }
}
