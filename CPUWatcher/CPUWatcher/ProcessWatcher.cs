using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CPUWatcher
{
    class ProcessWatcher : IDisposable
    {
        private readonly string appName;

        private List<ProcessUnit> processes = new List<ProcessUnit>();

        public delegate void CPUWatcherHandler(string message);

        public event CPUWatcherHandler ShowCPU;

        public ProcessWatcher(string name, CPUWatcherHandler handler)
        {
            appName = name;
            ShowCPU += handler;

            UpdateProcesses();

            EventClass.ElapseTimerEvent += EventClass_ElapseTimerEvent;
            EventClass.DisposeEvent += EventClass_DisposeEvent;
        }

        public void Dispose()
        {
            ShowCPU = null;
        }

        private void EventClass_ElapseTimerEvent(object sender, EventArgs e)
        {
            UpdateProcesses();
        }

        private void EventClass_DisposeEvent(object sender, EventArgs e)
        {
            Dispose();
        }

        private void UpdateProcesses()
        {
            var now = DateTime.UtcNow;
            var tempProcesses = new List<ProcessUnit>();

            var listOfProcesses = Process.GetProcesses().Where(x => x.ProcessName.ToLower().Equals(appName)).ToList();
            foreach (var process in listOfProcesses)
            {
                var p = processes.Find(x => x.ProcessId == process.Id);
                if (p == null)
                {
                    tempProcesses.Add(new ProcessUnit(process.Id, now, process.TotalProcessorTime.TotalMilliseconds));
                }
                else
                {
                    var endCpuUsage = process.TotalProcessorTime.TotalMilliseconds;
                    var endTime = now;

                    var cpuUsedMs = endCpuUsage - p.StartCpuUsage;
                    var totalMsPassed = (endTime - p.StartTime).TotalMilliseconds;
                    var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

                    p.StartTime = endTime;
                    p.StartCpuUsage = endCpuUsage;

                    ShowCPU?.Invoke($"{p.ProcessId} {appName}: {Math.Round(cpuUsageTotal * 100, 2)}");

                    tempProcesses.Add(p);
                }
            }

            processes = tempProcesses;
        }
    }
}
