using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsReader.Pages;

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

            Locator.ViewModels.PostIconListPageVm.ReplyBox = replyText;
            await Locator.ViewModels.PostIconListPageVm.Initialize();
            App.RootFrame.Navigate(typeof(PostIconListPage));
        }
    }
}
