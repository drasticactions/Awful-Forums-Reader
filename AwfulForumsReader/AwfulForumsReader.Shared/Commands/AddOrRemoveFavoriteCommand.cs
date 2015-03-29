using System.Linq;
using AwfulForumsReader.Common;
using AwfulForumsLibrary.Entity;
using AwfulForumsReader.Database;

namespace AwfulForumsReader.Commands
{
    public class AddOrRemoveFavoriteCommand : AlwaysExecutableCommand
    {
        public async override void Execute(object parameter)
        {
            var forumEntity = (ForumEntity) parameter;
            var realForumList =
                Locator.ViewModels.MainForumsPageVm.ForumGroupList.Where(node => !node.Name.Equals("Favorites")).SelectMany(node => node.ForumList);
            forumEntity = realForumList.FirstOrDefault(node => node.ForumId == forumEntity.ForumId);
            forumEntity.IsBookmarks = !forumEntity.IsBookmarks;
            var db = new DataSource();
            await db.ForumRepository.Update(forumEntity);
            await Locator.ViewModels.MainForumsPageVm.GetFavoriteForums();
        }
    }
}
