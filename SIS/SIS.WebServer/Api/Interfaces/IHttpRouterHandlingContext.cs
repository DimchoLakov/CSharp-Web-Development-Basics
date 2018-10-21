using SIS.HTTP.Requests;
using SIS.HTTP.Responses;

namespace SIS.WebServer.Api.Interfaces
{
    public interface IHttpHandlingContext
    {
        IHttpResponse Handle(IHttpRequest request);
    }
}
