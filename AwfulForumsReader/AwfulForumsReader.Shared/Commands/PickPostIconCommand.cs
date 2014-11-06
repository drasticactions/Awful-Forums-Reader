using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;

namespace AwfulForumsReader.Commands
{
    public class PickPostIconCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var args = parameter as ItemClickEventArgs;;
            if (args == null)
            {
                return;
            }
            var postIcon = args.ClickedItem as PostIconEntity;
            if (postIcon == null)
            {
                return;
            }
            Locator.ViewModels.NewThreadVm.PostIcon = postIcon;
            App.RootFrame.GoBack();
        }
    }
}
