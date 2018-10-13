using System;
using System.Globalization;

namespace SIS.HTTP.Extensions
{
    public static class StringExtensions
    {
        private const string NullOrEmptyStringMessage = "Input string cannot be null";

        public static string Capitalize(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(NullOrEmptyStringMessage);
            }
            ////return input.First().ToString().ToUpper() +
            ////       string.Join("", input.Skip(1)).ToLower();

            ////return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
    }
}
