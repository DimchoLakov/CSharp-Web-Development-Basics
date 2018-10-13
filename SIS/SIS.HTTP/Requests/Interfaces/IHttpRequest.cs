using System.Collections.Generic;
using SIS.HTTP.Cookies.Interfaces;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers.Interfaces;
using SIS.HTTP.Sessions.Interfaces;

namespace SIS.HTTP.Requests.Interfaces
{
    public interface IHttpRequest
    {
        string Path { get; }

        string Url { get; }

        Dictionary<string, object> FormData { get; }

        Dictionary<string, object> QueryData { get; }

        IHttpHeaderCollection Headers { get; }

        HttpRequestMethod RequestMethod { get; }
        
        IHttpCookieCollection Cookies { get; }

        IHttpSession Session { get; set; }
    }
}
