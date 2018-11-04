using SIS.HTTP.Enums;

namespace SIS.Framework.Attributes.Methods
{
    public class HttpDeleteAttribute : HttpMethodAttribute
    {
        public HttpDeleteAttribute(string path) : base(path)
        {
        }

        public override bool IsValid(string requestMethod)
        {
            return requestMethod.ToUpper() == "DELETE";
        }

        public override HttpRequestMethod Method => HttpRequestMethod.Delete;
    }
}
