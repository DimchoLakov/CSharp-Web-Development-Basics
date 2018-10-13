using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace _02.SliceFile
{
    public class Startup
    {
        private const int BufferLength = 4096;

        public static void Main()
        {
            Console.Write("Please enter source file: ");
            var sourceFile = Console.ReadLine();
            Console.Write("Please enter destination path: ");
            var destinationPath = Console.ReadLine();
            Console.Write("Parts: ");
            var parts = int.Parse(Console.ReadLine());

            SliceAsync(sourceFile, destinationPath, parts);
            Console.WriteLine("Anything else?");

            var input = Console.ReadLine();

            while (input != "exit" && input != "end")
            {
                Console.WriteLine($"You just typed \"{input}\"");

                input = Console.ReadLine();
            }

        }

        private static async void SliceAsync(string sourceFile, string destinationPath, int parts)
        {
            await Task.Run(() => Slice(sourceFile, destinationPath, parts));
        }

        private static void Slice(string sourceFile, string destinationPath, int parts)
        {
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            using (var source = new FileStream(sourceFile, FileMode.Open))
            {
                var fileInfo = new FileInfo(sourceFile);

                long partLength = (source.Length / parts) + 1;
                long currentByte = 0;

                for (int currentPart = 1; currentPart <= parts; currentPart++)
                {
                    Thread.Sleep(1000); //// Simulating big operations
                    var filePath = string.Format($"{destinationPath}/Part-{currentPart}.{fileInfo.Extension}");

                    using (var destination = new FileStream(filePath, FileMode.Create))
                    {
                        var buffer = new byte[BufferLength];
                        while (currentByte <= partLength * currentPart)
                        {
                            var readBytesCount = source.Read(buffer, 0, buffer.Length);
                            if (readBytesCount == 0)
                            {
                                break;
                            }

                            destination.Write(buffer, 0, readBytesCount);
                            currentByte += readBytesCount;
                        }
                    }
                }
            }

            Console.WriteLine("Slice complete.");
        }
    }
}