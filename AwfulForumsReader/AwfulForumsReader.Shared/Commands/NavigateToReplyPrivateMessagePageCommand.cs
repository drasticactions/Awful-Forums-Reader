using System;
using System.Collections.Generic;
using System.Text;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Pages;

namespace AwfulForumsReader.Commands
{
    public class NavigateToReplyPrivateMessagePageCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var vm = Locator.ViewModels.NewPrivateMessagePageVm;
            vm.PostRecipient = Locator.ViewModels.PrivateMessagePageVm.PrivateMessageEntity.Sender;
            vm.PostSubject = "Re: " + Locator.ViewModels.PrivateMessagePageVm.PrivateMessageEntity.Title;
            vm.PostBody = string.Empty;
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
