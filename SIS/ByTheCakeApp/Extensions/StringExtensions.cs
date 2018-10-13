using System.Net;

namespace ByTheCakeApp.Extensions
{
    public static class StringExtensions
    {
        public static string UrlDecode(this string inputUrl)
        {
            return WebUtility.UrlDecode(inputUrl);
        }
    }
}
