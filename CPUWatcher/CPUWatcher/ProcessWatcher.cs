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

        public ProcessWatcher(Watcher parent, string name, CPUWatcherHandler handler)
        {
            appName = name;
            ShowCPU += handler;

            parent.OnElapseTimerEvent += (s, e) =>
            {
                RefreshProcesses(e);
            };

            parent.OnAbortWatcher += (s, e) =>
            {
                Dispose();
            };
        }

        public void Dispose()
        {
            ShowCPU = null;
        }

        private void RefreshProcesses(List<Process> list)
        {
            var now = DateTime.UtcNow;
            var tempProcesses = new List<ProcessUnit>();

            var listOfProcesses = list.Where(x => x.ProcessName.ToLower().Equals(appName)).ToList();
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
