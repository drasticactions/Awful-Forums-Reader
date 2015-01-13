using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
