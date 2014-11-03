using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwfulForumsReader.Core.Exceptions
{
    public class WebManagerException : Exception
    {
        public WebManagerException()
        {
        }

        public WebManagerException(string message)
            : base(message)
        {
        }
    }
}
