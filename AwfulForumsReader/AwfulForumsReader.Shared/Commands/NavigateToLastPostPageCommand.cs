using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Manager;
using AwfulForumsReader.Pages;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands
{
    public class NavigateToLastPostPageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
             var replyText = parameter as TextBox;
            if (replyText == null)
            {
                return;
            }


            var vm = Locator.ViewModels.NewThreadReplyVm;
            var newVm = Locator.ViewModels.LastPostPageVm;
            
            newVm.ReplyBox = replyText;
            newVm.Name = vm.ForumThreadEntity.Name;
            App.RootFrame.Navigate(typeof(LastPostPage));
            await newVm.Initialize(vm.ForumReplyEntity);
        }
    }
}
