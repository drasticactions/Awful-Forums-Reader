using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Core.Tools;

namespace AwfulForumsReader.Core.Interface
{
    public interface IAuthenticationManager
    {
        string Status { get; }

        Task<bool> Authenticate(string userName, string password,
            int timeout = Constants.DefaultTimeoutInMilliseconds);
    }
}
