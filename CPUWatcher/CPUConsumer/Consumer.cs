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
            SetConsume();
        }

        private void SetConsume()
        {
            var processorCount = Environment.ProcessorCount;

            ConsoleInput.ShowLine("===============");
            ConsoleInput.ShowLine("Set CPU Consume");
            ConsoleInput.ShowLine($"Cpu available {processorCount}");

            var cpuCount = ConsoleInput.GetInteger("Enter cpu count: ", processorCount);
            var cpuUsage = ConsoleInput.GetInteger("Enter cpu usage: ", 100);

            ConsoleInput.ShowLine();
            ConsoleInput.ShowLine($"Consume {cpuCount} CPU, {cpuUsage}% usage");

            AddCPUConsumer(cpuCount, cpuUsage);

            ConsoleInput.WaitKey("Press <E> to abort CPU consume", ConsoleKey.E, AbortConsume);
        }

        private void AddCPUConsumer(int cpuCount, int cpuUsage)
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

        private void AbortConsume()
        {
            foreach (var t in threads)
            {
                t.Abort();
            }

            threads = new List<Thread>();

            SetConsume();
        }
    }
}
