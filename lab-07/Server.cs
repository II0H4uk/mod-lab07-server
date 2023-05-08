using System;
using System.Diagnostics;
using System.Threading;

namespace Lab07
{
    class Server
    {
        public int requestCount = 0;
        public int processedCount = 0;
        public int rejectedCount = 0;
        public Stopwatch executeSw;
        int executTime;

        PoolRecord[] pool;
        object threadLock = new object();

        public Server(int threadsCount, int inputExecutTime)
        {
            pool = new PoolRecord[threadsCount];
            executTime = inputExecutTime;
            executeSw = new Stopwatch();
        }

        struct PoolRecord
        {
            public Thread thread; // объект потока
            public bool in_use; // флаг занятости
        }

        public void proc(object sender, procEventArgs e)
        {
            lock (threadLock)
            {
                Console.WriteLine($"Заявка с номером: {e.id}");
                requestCount++;

                for (int i = 0; i < pool.Length; i++)
                {
                    if (!pool[i].in_use)
                    {
                        pool[i].in_use = true;
                        pool[i].thread = new Thread(new ParameterizedThreadStart(Answer));
                        pool[i].thread.Start(e.id);
                        processedCount++;
                        return;
                    }
                }
                rejectedCount++;
            }
        }

        public void Answer(object inputNum)
        {
            int num = (int)inputNum;

            Console.WriteLine($"Обработка заявки: {num}");
            executeSw.Start();
            Thread.Sleep(executTime);

            for (int i = 0; i < pool.Length; i++)
                if (pool[i].thread == Thread.CurrentThread)
                    pool[i].in_use = false;
            executeSw.Stop();
        }
    }
}
