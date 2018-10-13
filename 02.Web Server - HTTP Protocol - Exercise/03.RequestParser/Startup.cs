using System;
using System.Collections.Generic;
using System.Text;

namespace _03.RequestParser
{
    public class Startup
    {
        public static void Main()
        {
            var routes = new Dictionary<string, HashSet<string>>();

            var input = Console.ReadLine();

            while (input != "END")
            {
                var tokens = input.Split("/", StringSplitOptions.RemoveEmptyEntries);

                var httpRoute = tokens[0];
                var method = tokens[1];

                if (!routes.ContainsKey(httpRoute))
                {
                    routes.Add(httpRoute, new HashSet<string>());
                }

                routes[httpRoute].Add(method);

                input = Console.ReadLine();
            }

            var request = Console.ReadLine();
            var requestTokens = request.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var requestMethod = requestTokens[0].ToLower();
            var requestRoute = requestTokens[1].Substring(1, requestTokens[1].Length - 1);

            var statusCode = "404 NotFound";

            if (routes.ContainsKey(requestRoute))
            {
                if (routes[requestRoute].Contains(requestMethod))
                {
                    statusCode = "200 OK";
                }
            }

            var contentLength = statusCode.Length > 6 ? 8 : 2;
            var responseText = contentLength == 2 ? "OK" : "NotFound";

            var sb = new StringBuilder();

            sb.AppendLine($"HTTP/1.1 {statusCode}")
                .AppendLine($"Content-Length: {contentLength}")
                .AppendLine("Content-Type: text/plain")
                .Append(Environment.NewLine)
                .AppendLine(responseText);
            
            Console.WriteLine(sb.ToString().Trim());
        }
    }
}
