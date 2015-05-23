using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands.Navigation
{
    public class NavigateToSettingsCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            App.RootFrame.Navigate(typeof(SettingsPage));
        }
    }
}
