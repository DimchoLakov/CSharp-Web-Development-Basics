using SIS.HTTP.Enums;

namespace SIS.Framework.Attributes.Methods
{
    public class HttpPostAttribute : HttpMethodAttribute
    {
        public HttpPostAttribute(string path) : base(path)
        {
        }

        public override bool IsValid(string requestMethod)
        {
            return requestMethod.ToUpper() == "POST";
        }

        public override HttpRequestMethod Method => HttpRequestMethod.Post;
    }
}
