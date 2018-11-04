using SIS.HTTP.Enums;

namespace SIS.Framework.Attributes.Methods
{
    public class HttpGetAttribute : HttpMethodAttribute
    {
        public HttpGetAttribute(string path) : base(path)
        {
        }

        public override bool IsValid(string requestMethod)
        {
            return requestMethod.ToUpper() == "GET";
        }

        public override HttpRequestMethod Method => HttpRequestMethod.Get;
    }
}
