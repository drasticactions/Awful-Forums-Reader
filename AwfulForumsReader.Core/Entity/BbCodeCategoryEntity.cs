using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwfulForumsReader.Core.Entity
{
    public class BbCodeCategoryEntity
    {
        public BbCodeCategoryEntity(string category, List<BbCodeEntity> bbCodes)
        {
            BbCodes = bbCodes;
            Category = category;
        }

        public List<BbCodeEntity> BbCodes { get; private set; }

        public string Category { get; private set; }
    }

    public class BbCodeEntity
    {
        public BbCodeEntity(string title, string code)
        {
            Title = title;
            Code = code;
        }

        public string Title { get; private set; }

        public string Code { get; private set; }
    }
}
