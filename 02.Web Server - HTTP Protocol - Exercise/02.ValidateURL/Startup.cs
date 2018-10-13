using System;

namespace _02.ValidateURL
{
    public class Startup
    {
        public static void Main()
        {
            var inputUri = Console.ReadLine();
            var decodedUri = Uri.UnescapeDataString(inputUri);

            var uri = TryGetUri(decodedUri);

            var isValid = IsUriValid(decodedUri);
            var areSchemeAndPortValid = IsSchemePortValid(uri);

            if (!isValid || !areSchemeAndPortValid)
            {
                Console.WriteLine($"Invalid URL!");
                Environment.Exit(0);
            }

            PrintResult(uri);
        }

        private static bool IsSchemePortValid(Uri uri)
        {
            var protocol = uri.Scheme;
            var port = uri.Port;

            if ((protocol == "http" && port == 443) || (protocol == "https" && port == 80))
            {
                return false;
            }

            return true;
        }

        private static void PrintResult(Uri uri)
        {
            var protocol = uri.Scheme;
            var host = uri.Host;
            var port = uri.Port;
            var path = uri.AbsolutePath;
            var query = uri.Query;
            var fragment = uri.Fragment;

            Console.WriteLine($"Protocol: {protocol}");
            Console.WriteLine($"Host: {host}");
            Console.WriteLine($"Port: {port}");
            Console.WriteLine($"Path: {path}");
            if (!string.IsNullOrWhiteSpace(query))
            {
                query = query.Substring(1, query.Length - 1);
                Console.WriteLine($"Query: {query}");
                if (!string.IsNullOrWhiteSpace(fragment))
                {
                    fragment = fragment.Substring(1, fragment.Length - 1);
                    Console.WriteLine($"Fragment: {fragment}");
                }
            }
        }

        private static Uri TryGetUri(string decodedUri)
        {
            try
            {
                var uri = new Uri(decodedUri);
                return uri;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invalid URL!");
                Environment.Exit(0);
            }

            return new Uri(decodedUri);
        }

        private static bool IsUriValid(string decodedUri)
        {
            Uri uriResult;
            return Uri.TryCreate(decodedUri, UriKind.Absolute, out uriResult) &&
                uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps;
        }
    }
}
