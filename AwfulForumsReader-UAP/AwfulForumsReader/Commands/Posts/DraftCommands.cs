using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Common;
using AwfulForumsReader.Database;

namespace AwfulForumsReader.Commands.Posts
{
    public class SaveThreadDraftCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var replyText = parameter as TextBox;
            if (replyText == null)
            {
                return;
            }

            var vm = Locator.ViewModels.NewThreadReplyVm;
            var db = new MainForumsDatabase();
            await db.SaveThreadReplyDraft(replyText.Text, vm.ForumThreadEntity);
        }
    }

    public class LoadDraftCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var replyText = parameter as TextBox;
            if (replyText == null)
            {
                return;
            }
            var db = new MainForumsDatabase();
            var vm = Locator.ViewModels.NewThreadReplyVm;
            var draft = await db.LoadThreadDraftEntity(vm.ForumThreadEntity);
            if (draft == null)
            {
                return;
            }

            replyText.Text = draft.Draft;
        }
    }

    public class SaveNewThreadDraftCommand : AlwaysExecutableCommand
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

    public class LoadNewThreadDraftCommand : AlwaysExecutableCommand
    {
        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
