using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CPUWatcher
{
    class CPUWatcher : IDisposable
    {
        private readonly string appName;

        private List<ProcessWatcher> processes = new List<ProcessWatcher>();

        public delegate void CPUWatcherHandler(string message);

        public event CPUWatcherHandler ShowCPU;

        public CPUWatcher(string name, CPUWatcherHandler handler)
        {
            appName = name;
            ShowCPU += handler;

            UpdateProcesses();

            TimerEventClass.ElapseTimerEvent += TimerEventClass_ElapseTimerEvent;
        }

        public void Dispose()
        {
            ShowCPU = null;
        }

        private void TimerEventClass_ElapseTimerEvent(object sender, EventArgs e)
        {
            UpdateProcesses();
        }

        private void UpdateProcesses()
        {
            var now = DateTime.UtcNow;
            var tempProcesses = new List<ProcessWatcher>();

            var listOfProcesses = Process.GetProcesses().Where(x => x.ProcessName.ToLower().Equals(appName)).ToList();
            foreach (var process in listOfProcesses)
            {
                var p = processes.Find(x => x.ProcessId == process.Id);
                if (p == null)
                {
                    tempProcesses.Add(new ProcessWatcher(process.Id, now, process.TotalProcessorTime.TotalMilliseconds));
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
