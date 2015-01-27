using System.Linq;
using AwfulForumsReader.Database.Context;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;

namespace AwfulForumsReader.Commands
{
    public class AddOrRemoveFavoriteCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var forumEntity = (ForumEntity) parameter;
            forumEntity.IsBookmarks = !forumEntity.IsBookmarks;
            var realForumList =
                Locator.ViewModels.MainForumsPageVm.ForumGroupList.Where(node => !node.Name.Equals("Favorites")).SelectMany(node => node.ForumList);
            forumEntity = realForumList.FirstOrDefault(node => node.ForumId == forumEntity.ForumId);
            forumEntity.IsBookmarks = !forumEntity.IsBookmarks;
            using (var db = new MainForumListContext())
            {
                var forum = db.Forums.FirstOrDefault(node => node.ForumId == forumEntity.ForumId);
                if (forum == null)
                {
                    return;
                }
                
                forum.IsBookmarks = !forum.IsBookmarks;
                await db.SaveChangesAsync();
            }
            await Locator.ViewModels.MainForumsPageVm.GetFavoriteForums();
        }
    }
}
