using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _03.SimpleWebServer
{
    public class Startup
    {
        public static void Main()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            var port = 1300;
            var tcpListener = new TcpListener(ipAddress, port);
            tcpListener.Start();
            
            Console.WriteLine("Server started.");
            Console.WriteLine($"Listening to TCP clients at 127.0.0.1:{port}");

            var task = Task.Run(() => ConnectWithTcpClient(tcpListener));
            task.Wait();
        }

        private static async Task ConnectWithTcpClient(TcpListener tcpListener)
        {
            while (true)
            {
                Console.WriteLine("Waiting for client...");
                var client = await tcpListener.AcceptTcpClientAsync();

                var buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, buffer.Length);

                var message = Encoding.UTF8.GetString(buffer);
                Console.WriteLine(message);

                var response = Encoding.UTF8.GetBytes("Hello from Server!");
                client.GetStream().Write(response, 0, response.Length);

                Console.WriteLine("Closing connection.");
                client.Dispose();
            }
        }
    }
}
