using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Domain;

namespace CPUConsumer
{
    public class Consumer
    {
        private List<Thread> threads = new List<Thread>();

        public void Start()
        {
            SetCPUConsume();
        }

        private void SetCPUConsume()
        {
            var processorCount = Environment.ProcessorCount;

            Console.WriteLine("===============");
            Console.WriteLine("Set CPU Consume");
            Console.WriteLine("Cpu available {0}", processorCount);

            int cpuCount = ConsoleInput.GetInteger("Enter cpu count: ", processorCount);
            int cpuUsage = ConsoleInput.GetInteger("Enter cpu usage: ", 100);

            Console.WriteLine();
            Console.WriteLine("Consume {0} CPU, {1}% usage", cpuCount, cpuUsage);

            AddConsumer(cpuCount, cpuUsage);

            WaitAbort();
        }

        private void AddConsumer(int cpuCount, int cpuUsage)
        {
            for (var i = 0; i < cpuCount; i++)
            {
                var t = new Thread(new ParameterizedThreadStart(ConsumeCPU));
                t.IsBackground = true;
                t.Start(cpuUsage);

                threads.Add(t);
            }
        }

        private void ConsumeCPU(object cpuUsage)
        {
            int percent;
            try
            {
                percent = (int)cpuUsage;
            }
            catch (InvalidCastException)
            {
                return;
            }

            Stopwatch watch = new Stopwatch();
            watch.Start();

            while (true)
            {
                if (watch.ElapsedMilliseconds > percent)
                {
                    Thread.Sleep(100 - percent);
                    watch.Reset();
                    watch.Start();
                }
            }
        }

        private void WaitAbort()
        {
            Console.WriteLine("Press <E> to abort CPU consume");
            while (Console.ReadKey().Key == ConsoleKey.E)
            {
                foreach (var t in threads)
                {
                    t.Abort();
                }

                threads = new List<Thread>();

                Console.WriteLine();
                SetCPUConsume();
            }
        }
    }
}
