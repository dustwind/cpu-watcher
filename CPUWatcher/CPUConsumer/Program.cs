using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace CPUConsumer
{
    class Program
    {
        private static List<Thread> threads = new List<Thread>();

        static void Main(string[] args)
        {
            SetCPUConsume();

            Console.WriteLine("Press <E> to abort CPU consume");
            while (Console.ReadKey().Key == ConsoleKey.E)
            {
                foreach (var t in threads)
                {
                    t.Abort();
                }

                Console.WriteLine();
                SetCPUConsume();
            }

        }

        private static void SetCPUConsume()
        {
            int processorCount = Environment.ProcessorCount;
            string input;

            Console.WriteLine("===============");
            Console.WriteLine("Set CPU Consume");
            Console.WriteLine("Cpu available {0}", processorCount);

            int cpuCount;
            do
            {
                Console.Write("Enter cpu count: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out cpuCount) || (cpuCount > processorCount) || (cpuCount < 0));

            int cpuUsage;
            do
            {
                Console.Write("Enter cpu usage: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out cpuUsage) || (cpuUsage > 100) || (cpuUsage < 0));

            Console.WriteLine();
            Console.WriteLine("Consume {0} CPU, {1}% usage", cpuCount, cpuUsage);

            for (int i = 0; i < cpuCount; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(ConsumeCPU));
                t.IsBackground = true;
                t.Start(cpuUsage);
                threads.Add(t);
            }
        }

        private static void ConsumeCPU(object cpuUsage)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                if (watch.ElapsedMilliseconds > (int)cpuUsage)
                {
                    Thread.Sleep(100 - (int)cpuUsage);
                    watch.Reset();
                    watch.Start();
                }
            }
        }
    }
}
