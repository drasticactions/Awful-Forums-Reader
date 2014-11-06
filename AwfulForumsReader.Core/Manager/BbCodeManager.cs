using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Core.Entity;

namespace AwfulForumsReader.Core.Manager
{
    public static class BbCodeManager
    {
        private static IEnumerable<BbCodeCategoryEntity> bbCodes;

        public static IEnumerable<BbCodeCategoryEntity> BBCodes
        {
            get { return bbCodes ?? (bbCodes = GetBbCodes()); }
        }

        private static IEnumerable<BbCodeCategoryEntity> GetBbCodes()
        {
            var bbCodeCategoryList = new List<BbCodeCategoryEntity>();
            var bbCodeList = new List<BbCodeEntity>
            {
                new BbCodeEntity("url", "url"),
                new BbCodeEntity("email", "email"),
                new BbCodeEntity("img", "img"),
                new BbCodeEntity("timg", "timg"),
                new BbCodeEntity("video", "video"),
                new BbCodeEntity("b", "b"),
                new BbCodeEntity("s", "s"),
                new BbCodeEntity("u", "u"),
                new BbCodeEntity("i", "i"),
                new BbCodeEntity("spoiler", "spoiler"),
                new BbCodeEntity("fixed", "fixed"),
                new BbCodeEntity("super", "super"),
                new BbCodeEntity("sub", "sub"),
                new BbCodeEntity("size", "size"),
                new BbCodeEntity("color", "color"),
                new BbCodeEntity("quote", "quote"),
                new BbCodeEntity("url", "url"),
                new BbCodeEntity("pre", "pre"),
                new BbCodeEntity("code", "code"),
                new BbCodeEntity("php", "php"),
                new BbCodeEntity("list", "list")
            };
            bbCodeCategoryList.Add(new BbCodeCategoryEntity("BBCode", bbCodeList));
            return bbCodeCategoryList;
        }
    }
}
