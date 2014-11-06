using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;

namespace AwfulForumsReader.Commands
{
    public class NavigateToSmiliesPageCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var replyText = parameter as TextBox;
            if (replyText == null)
            {
                return;
            }
        }
    }
}
