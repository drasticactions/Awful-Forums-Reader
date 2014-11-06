using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;

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
        }
    }
}
