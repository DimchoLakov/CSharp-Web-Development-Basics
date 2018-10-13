using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SoftUniHttpServer
{
    public class Startup
    {
        public static void Main()
        {
            IHttpServer server = new HttpServer();
            server.Start();
        }

        public interface IHttpServer
        {
            void Start();

            void Stop();
        }

        internal class HttpServer : IHttpServer
        {
            private bool isWorking;

            private TcpListener tcpListener;

            public HttpServer()
            {
                this.tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);
            }

            public void Start()
            {
                Console.WriteLine("TCP Listener is starting...");
                this.tcpListener.Start();
                this.isWorking = true;

                while (this.isWorking)
                {
                    var client = this.tcpListener.AcceptTcpClient();

                    var stream = client.GetStream();
                    var buffer = new byte[10500];
                    var readLength = stream.Read(buffer, 0, buffer.Length);
                    var requestText = Encoding.UTF8.GetString(buffer, 0, readLength);

                    Console.WriteLine(new string('=', 70));
                    Console.WriteLine(requestText);

                    var responseText = File.ReadAllText("index.html");
                    var responseBytes = Encoding.UTF8.GetBytes(
                        "HTTP/1.1 200 OK" + Environment.NewLine +
                        "Content-Type: text/html" + Environment.NewLine +
                        "Content-Length: " + responseText.Length + Environment.NewLine + Environment.NewLine +
                        responseText
                    );
                    foreach (var responseByte in responseBytes)
                    {
                        Console.Write(responseByte);
                    }
                    stream.Write(responseBytes);
                }
            }

            public void Stop()
            {
                this.isWorking = false;
            }
        }
    }
}
