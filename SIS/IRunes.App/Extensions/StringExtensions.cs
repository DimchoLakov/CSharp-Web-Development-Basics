using System.Net;

namespace IRunes.App.Extensions
{
    public static class StringExtensions
    {
        public static string DecodeUrl(this string input)
        {
            return WebUtility.UrlDecode(input);
        }
    }
}
