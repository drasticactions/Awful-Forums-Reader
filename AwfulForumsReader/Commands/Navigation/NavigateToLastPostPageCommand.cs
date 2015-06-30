using Windows.UI.Xaml.Controls;
using AwfulForumsReader.Common;

namespace AwfulForumsReader.Commands.Navigation
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


            //var vm = Locator.ViewModels.NewThreadReplyVm;
            //var newVm = Locator.ViewModels.LastPostPageVm;
            
            //newVm.ReplyBox = replyText;
            //newVm.Name = vm.ForumThreadEntity.Name;
            //App.RootFrame.Navigate(typeof(LastPostPage));
            //await newVm.Initialize(vm.ForumReplyEntity);
        }
    }
}
