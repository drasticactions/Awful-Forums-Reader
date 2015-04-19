using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;

namespace AwfulForumsReader.Commands
{
    public class ScrollToBottomThreadPageCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            try
            {
                var threadFullView = (WebView)parameter;
                await threadFullView.InvokeScriptAsync("ScrollToBottom", null);
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
