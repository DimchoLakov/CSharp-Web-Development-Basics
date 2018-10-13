using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _03.SimpleWebServer
{
    public class HttpServer : IHttpServer
    {
        private TcpListener tcpListener;
        private bool isWorking;

        public HttpServer()
        {
            
        }

        public void Start()
        {
            this.isWorking = true;

            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            var port = 80;
            this.tcpListener = new TcpListener(ipAddress, port);
            this.tcpListener.Start();

            Console.WriteLine("TCP Listener has started and is waiting for incoming requests...");

            while (this.isWorking)
            {
                var client = this.tcpListener.AcceptTcpClient();

                Task.Run(() => ProcessClient(client));
            }
        }

        private async void ProcessClient(TcpClient client)
        {
            var stream = client.GetStream();
            var buffer = new byte[10240];

            Console.WriteLine($"{client.Client.RemoteEndPoint} {Thread.CurrentThread.ManagedThreadId}");

            var readLength = await stream.ReadAsync(buffer, 0, buffer.Length);
            Console.WriteLine($"{client.Client.RemoteEndPoint} {Thread.CurrentThread.ManagedThreadId}");

            var requestText = Encoding.UTF8.GetString(buffer, 0, readLength);

            ////Console.WriteLine(new string('=', 70));
            ////Console.WriteLine(requestText);

            ////await Task.Run(() => Thread.Sleep(10000));

            var responseText = File.ReadAllTextAsync("Views/index.html").Result + DateTime.Now;
            var responseBytes = Encoding.UTF8.GetBytes
            (
                "HTTP/1.1 200 OK" + Environment.NewLine +
                "Content-Type: text/html" + Environment.NewLine +
                "Content-Length: " + responseText.Length + Environment.NewLine + Environment.NewLine +
                responseText
            );

            await stream.WriteAsync(responseBytes);
            Console.WriteLine($"{client.Client.RemoteEndPoint} {Thread.CurrentThread.ManagedThreadId}");

        }

        public void Stop()
        {
            this.isWorking = false;
        }
    }
}
