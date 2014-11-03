using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwfulForumsReader.Core.Entity
{
    public class ForumEntity
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public int CurrentPage { get; set; }

        public bool IsSubforum { get; set; }

        public int TotalPages { get; set; }

        public int ForumId { get; set; }

        public int CategoryId { get; set; }

        public int ParentForumId { get; set; }

        public virtual ForumEntity ParentForum { get; set; }

        public virtual ForumCategoryEntity ForumCategory { get; set; }

        public bool IsBookmarks { get; set; }
    }
}
