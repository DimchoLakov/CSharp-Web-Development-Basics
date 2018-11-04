using SIS.HTTP.Enums;

namespace SIS.Framework.Attributes.Methods
{
    public class HttpPutAttribute : HttpMethodAttribute
    {
        public HttpPutAttribute(string path) : base(path)
        {
        }
        public override bool IsValid(string requestMethod)
        {
            return requestMethod.ToUpper() == "PUT";
        }

        public override HttpRequestMethod Method => HttpRequestMethod.Put;   
    }
}
