using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Common;
using AwfulForumsReader.Notification;

namespace AwfulForumsReader.Commands.Forums
{
    public class CreateForumTileCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var forumEntity = parameter as ForumEntity;
            if (forumEntity == null)
            {
                return;
            }

            var result = await NotifyStatusTile.CreateSecondaryForumLinkTile(forumEntity);
        }
    }
}
