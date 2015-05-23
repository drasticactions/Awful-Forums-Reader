using System;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;

namespace AwfulForumsReader.Commands.Posts
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
