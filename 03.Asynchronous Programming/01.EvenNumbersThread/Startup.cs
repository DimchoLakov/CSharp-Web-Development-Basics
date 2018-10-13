using System;
using System.Text;
using System.Threading;

namespace _01.EvenNumbersThread
{
    public class Startup
    {
        public static void Main()
        {
            var start = int.Parse(Console.ReadLine());
            var end = int.Parse(Console.ReadLine());

            Thread evenNumbers = new Thread(() => PrintEvenNumbersInRange(start, end));
            evenNumbers.Start();
            evenNumbers.Join();
            Console.WriteLine("Thread finished work");
        }

        private static void PrintEvenNumbersInRange(int start, int end)
        {
            var sb = new StringBuilder();
            for (int i = start; i <= end; i++)
            {
                if (i % 2 == 0)
                {
                    sb.AppendLine(i.ToString());
                }
            }
            Console.WriteLine(sb.ToString().Trim());
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
        }
    }
}
