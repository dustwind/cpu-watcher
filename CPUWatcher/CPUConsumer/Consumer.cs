using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace CPUConsumer
{
    public class Consumer
    {
        private List<Thread> threads = new List<Thread>();

        public void Start(int cpuCount, int cpuUsage)
        {
            for (var i = 0; i < cpuCount; i++)
            {
                var t = new Thread(new ParameterizedThreadStart(ConsumeCPU));
                t.IsBackground = true;
                t.Start(cpuUsage);

                threads.Add(t);
            }
        }

        public void Abort()
        {
            foreach (var t in threads)
            {
                t.Abort();
            }

            threads = new List<Thread>();
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
    }
}
