using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;
using AwfulForumsReader.ViewModels;

namespace AwfulForumsReader.Commands
{
    public class NavigateToBbCodePageCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var replyText = parameter as TextBox;
            if (replyText == null)
            {
                return;
            }
          
            var test = replyText.DataContext as NewThreadPageViewModel;
            if (test != null)
            {
                Locator.ViewModels.BbCodeListVm.ReplyBoxLocation = ReplyBoxLocation.NewThread;
            }

            var test2 = replyText.DataContext as NewThreadReplyPageViewModel;
            if (test2 != null)
            {
                Locator.ViewModels.BbCodeListVm.ReplyBoxLocation = ReplyBoxLocation.NewReply;
            }

            Locator.ViewModels.BbCodeListVm.ReplyBox = replyText;
             Locator.ViewModels.BbCodeListVm.Initialize();
            App.RootFrame.Navigate(typeof (BbCodeListPage));
        }
    }
}
