using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands
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
}
