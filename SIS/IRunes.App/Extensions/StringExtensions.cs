using System.Net;

namespace IRunes.App.Extensions
{
    public static class StringExtensions
    {
        public static string DecodeUrl(this string input)
        {
            return WebUtility.UrlDecode(input);
        }

        public static string DecodeHtml(this string input)
        {
            return WebUtility.HtmlDecode(input);
        }
    }
}
