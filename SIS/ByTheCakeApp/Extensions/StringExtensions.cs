using System.Net;

namespace ByTheCake.App.Extensions
{
    public static class StringExtensions
    {
        public static string UrlDecode(this string inputUrl)
        {
            return WebUtility.UrlDecode(inputUrl);
        }
    }
}
