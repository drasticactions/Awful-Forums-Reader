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
    public class NavigateToPostIconPageCommand : AlwaysExecutableCommand
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

            var test3 = replyText.DataContext as NewPrivateMessageViewModel;
            if (test3 != null)
            {
                Locator.ViewModels.PostIconListPageVm.ReplyBoxLocation = ReplyBoxLocation.PrivateMessage;
            }

            Locator.ViewModels.PostIconListPageVm.ReplyBox = replyText;
            await Locator.ViewModels.PostIconListPageVm.Initialize();
            App.RootFrame.Navigate(typeof(PostIconListPage));
        }
    }
}
