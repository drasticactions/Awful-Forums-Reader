using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AwfulForumsReader.Core.Manager;

namespace AwfulForumsReader.Core.Interface
{
    public interface IWebManager
    {
        bool IsNetworkAvailable { get; }
        Task<WebManager.Result> GetData(string uri);
        Task<CookieContainer> PostData(string uri, string data);
        Task<HttpResponseMessage> PostFormData(string uri, MultipartFormDataContent form);
    }
}
