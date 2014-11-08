using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;
using AwfulForumsReader.ViewModels;

namespace AwfulForumsReader.Commands
{
    public class NavigateToSmiliesPageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var replyText = parameter as TextBox;
            if (replyText == null)
            {
                return;
            }

            var test = replyText.DataContext as NewThreadPageViewModel;
            if (test != null)
            {
                Locator.ViewModels.SmiliesPageVm.ReplyBoxLocation = ReplyBoxLocation.NewThread;
            }

            var test2 = replyText.DataContext as NewThreadReplyPageViewModel;
            if (test2 != null)
            {
                Locator.ViewModels.SmiliesPageVm.ReplyBoxLocation = ReplyBoxLocation.NewReply;
            }

            var test3 = replyText.DataContext as NewPrivateMessageViewModel;
            if (test3 != null)
            {
                Locator.ViewModels.SmiliesPageVm.ReplyBoxLocation = ReplyBoxLocation.PrivateMessage;
            }

            Locator.ViewModels.SmiliesPageVm.ReplyBox = replyText;
            App.RootFrame.Navigate(typeof(SmiliesPage));
            await Locator.ViewModels.SmiliesPageVm.Initialize();
            
        }
    }
}
