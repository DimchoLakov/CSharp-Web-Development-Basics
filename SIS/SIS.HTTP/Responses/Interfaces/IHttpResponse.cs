using SIS.HTTP.Cookies;
using SIS.HTTP.Cookies.Interfaces;
using SIS.HTTP.Enums;
using SIS.HTTP.Headers;
using SIS.HTTP.Headers.Interfaces;

namespace SIS.HTTP.Responses.Interfaces
{
    public interface IHttpResponse
    {
        HttpResponseStatusCode StatusCode { get; set; }

        IHttpHeaderCollection Headers { get; }

        IHttpCookieCollection Cookies { get; }

        byte[] Content { get; set; }

        byte[] GetBytes();

        void AddHeader(HttpHeader header);

        void AddCookie(HttpCookie cookie);
    }
}
