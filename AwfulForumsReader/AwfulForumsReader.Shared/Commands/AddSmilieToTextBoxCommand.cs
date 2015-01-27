using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands
{
    public class AddSmilieToTextBoxCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var args = parameter as ItemClickEventArgs; ;
            if (args == null)
            {
                return;
            }
            var smileEntity = args.ClickedItem as SmileEntity;
            if (smileEntity == null)
            {
                return;
            }
            App.RootFrame.GoBack();
            var vm = Locator.ViewModels.SmiliesPageVm;
            if (!string.IsNullOrEmpty(vm.ReplyBox.SelectedText))
            {
                string selectedText = "[{0}]" + vm.ReplyBox.SelectedText + "[/{0}]";
                vm.ReplyBox.SelectedText = string.Format(selectedText, smileEntity.Title);
            }
            else
            {
                string text = string.Format("[{0}][/{0}]", smileEntity.Title);
                string replyText = string.IsNullOrEmpty(vm.ReplyBox.Text) ? string.Empty : vm.ReplyBox.Text;
                if (replyText != null) vm.ReplyBox.Text = replyText.Insert(vm.ReplyBox.SelectionStart, text);
            }
            switch (Locator.ViewModels.BbCodeListVm.ReplyBoxLocation)
            {
                case ReplyBoxLocation.NewThread:
                    Locator.ViewModels.NewThreadVm.PostBody = vm.ReplyBox.Text;
                    break;
                case ReplyBoxLocation.NewReply:
                    Locator.ViewModels.NewThreadReplyVm.PostBody = vm.ReplyBox.Text;
                    break;
                default:
                    throw new Exception("Unsupported type");
            }
        }
    }
}
