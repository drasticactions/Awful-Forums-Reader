using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands.Navigation
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
