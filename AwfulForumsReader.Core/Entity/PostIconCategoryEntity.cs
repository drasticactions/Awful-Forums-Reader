using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwfulForumsReader.Core.Entity
{
    public class PostIconCategoryEntity
    {
        public PostIconCategoryEntity(string category, List<PostIconEntity> list)
        {
            List = list;
            Category = category;
        }

        public virtual ICollection<PostIconEntity> List { get; private set; }

        public string Category { get; private set; }
    }
}
