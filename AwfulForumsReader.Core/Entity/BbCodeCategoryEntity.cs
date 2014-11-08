using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwfulForumsReader.Core.Entity
{
    public class BbCodeCategoryEntity
    {
        public BbCodeCategoryEntity()
        {
            BbCodes = new List<BbCodeEntity>();
        }

        public virtual ICollection<BbCodeEntity> BbCodes { get; set; }

        public string Name { get; set; }
    }

    public class BbCodeEntity
    {
        public string Title { get; set; }

        public string Code { get; set; }
    }
}
