using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Core.Tools;

namespace AwfulForumsReader.Core.Entity
{
    public class ForumCategoryEntity
    {
        public ForumCategoryEntity()
        {
            ForumList = new List<ForumEntity>();
        }
        public string Name { get; set; }

        public string Location { get; set; }

        public int CategoryId { get; set; }

        /// <summary>
        ///     The forums that belong to that category (Ex. GBS, FYAD)
        /// </summary>
        public virtual ICollection<ForumEntity> ForumList { get; set; }
    }
}
