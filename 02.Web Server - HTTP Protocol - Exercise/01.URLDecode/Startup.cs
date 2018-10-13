using System;

namespace _01.URLDecode
{
    public class Startup
    {
        public static void Main()
        {
            var uriInput = Console.ReadLine();

            var decodedUri = Uri.UnescapeDataString(uriInput);
            Console.WriteLine(decodedUri);
        }
    }
}
