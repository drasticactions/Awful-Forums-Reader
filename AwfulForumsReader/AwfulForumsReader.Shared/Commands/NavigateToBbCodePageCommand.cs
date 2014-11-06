using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

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
            Locator.ViewModels.BbCodeListVm.ReplyBox = replyText;
             Locator.ViewModels.BbCodeListVm.Initialize();
            App.RootFrame.Navigate(typeof (BbCodeListPage));
        }
    }
}
