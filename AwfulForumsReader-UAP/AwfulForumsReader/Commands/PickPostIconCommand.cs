using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Tools;

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

            switch (Locator.ViewModels.PostIconListPageVm.ReplyBoxLocation)
            {
                case ReplyBoxLocation.NewThread:
                    Locator.ViewModels.NewThreadVm.PostIcon = postIcon;
                    break;
                    case ReplyBoxLocation.PrivateMessage:
                    Locator.ViewModels.NewPrivateMessagePageVm.PostIcon = postIcon;
                    break;
            }
            App.RootFrame.GoBack();
        }
    }
}
