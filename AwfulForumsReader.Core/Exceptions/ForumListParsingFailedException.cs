using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwfulForumsReader.Core.Exceptions
{
    public class ForumListParsingFailedException : Exception
    {
        public ForumListParsingFailedException()
        {
        }

        public ForumListParsingFailedException(string message)
            : base(message)
        {
        }
    }
}
