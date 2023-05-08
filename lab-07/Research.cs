using System;
using System.IO;
using System.Diagnostics;

namespace Lab07
{
    class Research
    {
        int n;
        Stopwatch workSw;
        double ro;
        double P0;
        double Pn;
        double Q;
        double A;
        double K;

        public Research(double inputLambda, double inputMu, int inputN, Stopwatch inputSw, Server server)
        {
            double lambda = inputLambda;
            double mu = inputMu;
            n = inputN;
            workSw = inputSw;
            string result = "";

            CalcAll(lambda, mu);
            
            result += "Характеристики системы:\n";
            result += $"Интенсивность потока запросов = {Math.Round(lambda, 2)}\n" +
                $"Интенсивность потока обслуживания = {Math.Round(mu, 2)}\n" +
                $"Количество потоков = {n}\n" +
                $"Время работы = {workSw.ElapsedMilliseconds} мс\n";

            Console.WriteLine("Теоретические рассчеты:");
            result += "Теоретические рассчеты:\n";
            result += ConsoleWriter(lambda, mu);

            double roDif = ro;
            double P0Dif = P0;
            double PnDif = Pn;
            double QDif = Q;
            double ADif = A;
            double KDif = K;

            double realLambd = (double)server.requestCount * 1000 / workSw.ElapsedMilliseconds;
            double realMu = (double)server.processedCount * 1000 / (server.executeSw.ElapsedMilliseconds * n);

            CalcAll(realLambd, realMu);

            Console.WriteLine("\nПрактические рассчеты:");
            result += "\n\nПрактические рассчеты:\n";
            result += ConsoleWriter(realLambd, realMu);

            ro = Math.Abs(ro - roDif);
            P0 = Math.Abs(P0 - P0Dif);
            Pn = Math.Abs(Pn - PnDif);
            Q = Math.Abs(Q - QDif);
            A = Math.Abs(A - ADif);
            K = Math.Abs(K - KDif);

            Console.WriteLine("\nРазница рассчетов:");
            result += "\n\nРазница рассчетов:\n";
            result += ConsoleWriter(Math.Abs(lambda - realLambd), Math.Abs( mu - realMu));
            CreateReport("results.txt", result);
        }

        void CreateReport(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        string ConsoleWriter(double lambda, double mu)
        {
            string result = 
                $"Приведенная интенсивность потока запросов = {Math.Round(ro, 2)}\n" +
                $"Вероятность простоя системы = {Math.Round(P0, 2)}\n" +
                $"Вероятность отказа системы = {Math.Round(Pn, 2)}\n" +
                $"Относительная пропускная способность = {Math.Round(Q, 2)}\n" +
                $"Абсолютная пропускная способность = {Math.Round(A, 2)}\n" +
                $"Среднее число занятых каналов = {Math.Round(K, 2)}";
            Console.WriteLine(result);
            return result;
        }

        void CalcAll(double lambda, double mu)
        {
            ro = lambda / mu;
            P0 = Sum(ro, n);
            Pn = Math.Pow(ro, n) / Fact(n) * P0;
            Q = 1 - Pn;
            A = lambda * Q;
            K = A / mu;
        }

        double Sum(double ro, int n)
        {
            double result = 0;
            for (int i = 0; i <= n; i++)
                result += Math.Pow(ro, i) / Fact(i);
            return Math.Pow(result, -1);
        }

        public int Fact(int num)
        {
            int result = 1;
            for (int i = 1; i <= num; i++)
                result *= i;
            return result;
        }
    }
}
