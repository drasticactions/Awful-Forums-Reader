using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Core.Entity;
using AwfulForumsReader.Database.Context;
namespace AwfulForumsReader.Database.Commands
{
    public class MainForumsManager
    {
        public List<ForumCategoryEntity>  GetMainForumsList()
        {
            using (var db = new MainForumListContext())
            {
                var list = new List<ForumCategoryEntity>();
                db.Forums.ToList();
                var forumCategoryEntities = db.ForumCategories.ToList();
                if (!forumCategoryEntities.Any()) return list;
                foreach (var forumCategoryEntity in forumCategoryEntities)
                {
                    var testForumList = new List<ForumEntity>();
                    foreach (var forum in forumCategoryEntity.ForumList.Where(node => node.ParentForum == null))
                    {
                        testForumList.Add(forum);
                        ForumEntity forum1 = forum;
                        testForumList.AddRange(forumCategoryEntity.ForumList.Where(node => node.ParentForum == forum1));
                    }
                    forumCategoryEntity.ForumList = testForumList;
                    list.Add(forumCategoryEntity);
                }
                return list;
            }

        }

        public List<ForumEntity> GetSubforums(int forumId)
        {
            using (var db = new MainForumListContext())
            {
                return db.Forums.Where(node => node.ParentForumId == forumId).ToList();
            }
        }

        public async Task SaveMainForumsList(List<ForumCategoryEntity> forumGroupList)
        {
            using (var db = new MainForumListContext())
            {
                foreach (var forumGroup in forumGroupList)
                {
                    foreach (var forumEntity in forumGroup.ForumList)
                    {
                        db.Add(forumEntity);
                    }
                    db.Add(forumGroup);
                }

                await db.SaveChangesAsync();
            }
        }

        public static async Task RemoveForums()
        {
            using (var db = new MainForumListContext())
            {
                var entries = db.ForumCategories;
                var forums = db.Forums;
                db.ForumCategories.RemoveRange(entries);
                db.Forums.RemoveRange(forums);
                await db.SaveChangesAsync();
            }
        }

        public List<ForumEntity> GetFavoriteForums()
        {
            using (var db = new MainForumListContext())
            {
                return db.Forums.Where(node => node.IsBookmarks).ToList();
            }
        }
    }
}
