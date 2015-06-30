using Windows.UI.Xaml.Controls;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Common;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands.Threads
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
