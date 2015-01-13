using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwfulForumsReader.Core.Entity
{
    public class SearchEntity
    {
        public string ResultNumber { get; set; }

        public string ThreadTitle { get; set; }

        public string ThreadLink { get; set; }

        public string Username { get; set; }

        public string ForumName { get; set; }

        public string Blurb { get; set; }
    }
}
