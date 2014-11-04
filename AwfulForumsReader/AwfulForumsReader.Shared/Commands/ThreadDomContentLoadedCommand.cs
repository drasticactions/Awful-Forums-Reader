using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;

namespace AwfulForumsReader.Commands
{
    public class ThreadDomContentLoadedCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var test = parameter as WebView;
            if (test == null)
            {
                return;
            }
            try
            {
                if (Locator.ViewModels.ThreadPageVm.ForumThreadEntity.ScrollToPost > 0)
                {
                    await test.InvokeScriptAsync("ScrollToDiv", new[] { Locator.ViewModels.ThreadPageVm.ForumThreadEntity.ScrollToPostString });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Webview Failer {0}", ex);
            }
        }
    }
}
