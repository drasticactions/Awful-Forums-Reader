using System.Linq;
using AwfulForumsReader.Common;
using AwfulForumsReader.Context;
using AwfulForumsReader.Core.Entity;

namespace AwfulForumsReader.Commands
{
    public class AddOrRemoveFavoriteCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var forumEntity = (ForumEntity) parameter;
            using (var db = new MainForumListContext())
            {
                var forum = db.Forums.FirstOrDefault(node => node.ForumId == forumEntity.ForumId);
                if (forum == null)
                {
                    return;
                }
                forumEntity.IsBookmarks = !forumEntity.IsBookmarks;
                forum.IsBookmarks = !forum.IsBookmarks;
                await db.SaveChangesAsync();
                await Locator.ViewModels.MainForumsPageVm.GetFavoriteForums();
            }
            
        }
    }
}
