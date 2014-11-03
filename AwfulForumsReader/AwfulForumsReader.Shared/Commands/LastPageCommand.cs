using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;
using AwfulForumsReader.Core.Entity;

namespace AwfulForumsReader.Commands
{
    public class LastPageCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            var thread = parameter as ForumThreadEntity;
            if (thread == null)
                return;
            thread.CurrentPage = thread.TotalPages;
            //string jsonObjectString = JsonConvert.SerializeObject(thread);
            //var rootFrame = Window.Current.Content as Frame;
            //if (rootFrame != null) rootFrame.Navigate(typeof(ThreadPage), jsonObjectString);
        }
    }
}
