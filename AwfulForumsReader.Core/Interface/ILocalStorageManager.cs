using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AwfulForumsReader.Core.Interface
{
    public interface ILocalStorageManager
    {
        Task SaveCookie(string filename, CookieContainer rcookie, Uri uri);
        Task<CookieContainer> LoadCookie(string filename);
        Task<bool> RemoveCookies(string filename);
    }
}
