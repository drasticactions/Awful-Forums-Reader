using System;
using AwfulForumsLibrary.Entity;
using AwfulForumsLibrary.Tools;
using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands.Navigation
{
    public class NavigateToNewPrivateMessagePageCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var vm = Locator.ViewModels.NewPrivateMessagePageVm;
            vm.PostRecipient = string.Empty;
            vm.PostSubject = string.Empty;
            vm.PostBody = string.Empty;
            vm.PostIcon = new PostIconEntity()
            {
                Id = 0,
                ImageUrl = "/Assets/ThreadTags/shitpost.png",
                Title = "Shit Post"
            };
            vm.PostRecipient = string.Empty;

            App.RootFrame.Navigate(typeof (NewPrivateMessagePage));
        }
    }

    public class NavigateToNewPrivateMessagePageLowtaxCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var vm = Locator.ViewModels.NewPrivateMessagePageVm;
            vm.PostRecipient = "Lowtax";
            vm.PostSubject = "You're a Jerk! -Cortana";
            vm.PostBody = string.Format(
                        "You're a jerk!{0}With love,{0}Cortana{0}{0}{1}",
                        Environment.NewLine, Constants.Ascii2);
            vm.PostIcon = new PostIconEntity()
            {
                Id = 0,
                ImageUrl = "/Assets/ThreadTags/shitpost.png",
                Title = "Shit Post"
            };

            App.RootFrame.Navigate(typeof(NewPrivateMessagePage));
        }
    }
}
