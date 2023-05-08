using System;
using System.Threading;
using System.Diagnostics;

namespace Lab07
{
    class Program
    {
        static void Main()
        {
            double lambda = 5;
            int executTime = 500;
            int n = 5;
            int requestCount = 10;

            Stopwatch workSw = new Stopwatch();

            Server server = new Server(n, executTime);
            Client client = new Client(server);

            workSw.Start();

            for (int i = 0; i < requestCount; i++)
            {
                client.Send(i);
                Thread.Sleep((int)(1000 / lambda));
            }

            workSw.Stop();
            Console.WriteLine();

            Research research = new Research(lambda, (double)1000 / executTime, n, workSw, server);

            Console.WriteLine($"Число заявок: {server.requestCount}");
            Console.WriteLine($"Обработано заявок: {server.processedCount}");
            Console.WriteLine($"Отклонено заявок: {server.rejectedCount}");
        }
    }
}
