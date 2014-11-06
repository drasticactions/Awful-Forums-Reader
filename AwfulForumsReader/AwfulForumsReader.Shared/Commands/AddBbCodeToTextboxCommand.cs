using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Tools;

namespace AwfulForumsReader.Commands
{
    public class AddBbCodeToTextboxCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var args = parameter as ItemClickEventArgs; ;
            if (args == null)
            {
                return;
            }
            var bbCode = args.ClickedItem as BbCodeEntity;
            if (bbCode == null)
            {
                return;
            }
            App.RootFrame.GoBack();
            if (!string.IsNullOrEmpty(Locator.ViewModels.BbCodeListVm.ReplyBox.SelectedText))
            {
                string selectedText = "[{0}]" + Locator.ViewModels.BbCodeListVm.ReplyBox.SelectedText + "[/{0}]";
                Locator.ViewModels.BbCodeListVm.ReplyBox.SelectedText = string.Format(selectedText, bbCode.Code);
            }
            else
            {
                string text = string.Format("[{0}][/{0}]", bbCode.Code);
                string replyText = string.IsNullOrEmpty(Locator.ViewModels.BbCodeListVm.ReplyBox.Text) ? string.Empty : Locator.ViewModels.BbCodeListVm.ReplyBox.Text;
                if (replyText != null) Locator.ViewModels.BbCodeListVm.ReplyBox.Text = replyText.Insert(Locator.ViewModels.BbCodeListVm.ReplyBox.SelectionStart, text);
            }

            switch (Locator.ViewModels.BbCodeListVm.ReplyBoxLocation)
            {
                case ReplyBoxLocation.NewThread:
                    Locator.ViewModels.NewThreadVm.PostBody = Locator.ViewModels.BbCodeListVm.ReplyBox.Text;
                    break;
                case ReplyBoxLocation.NewReply:
                    Locator.ViewModels.NewThreadReplyVm.PostBody = Locator.ViewModels.BbCodeListVm.ReplyBox.Text;
                    break;
                default:
                    throw new Exception("Unsupported type");
            }
        }
    }
}
