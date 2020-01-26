using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CPUWatcher
{
    class ProcessCPUWatcher
    {
        public string AppName { get; set; }

        private DateTime? startTime = null;
        private double? startCpuUsage = null;

        public ProcessCPUWatcher(string appname, TimeSpan interval, CancellationToken ct)
        {
            this.AppName = appname;

            this.MonitorUsindDiagnosticsAsync(this.AppName, interval, ct);
        }

        private async Task MonitorUsindDiagnosticsAsync(string process, TimeSpan interval, CancellationToken ct)
        {
            while (true)
            {
                var cpuUsage = await this.GetCpuUsageForProcess(process, interval, ct);

                if (ct.IsCancellationRequested)
                {
                    return;
                }

                if (cpuUsage == null)
                {
                    Console.WriteLine("Can not find process {0}", process);
                    return;
                }
                else
                {
                    Console.WriteLine("{0} : {1}", process, cpuUsage);
                }
            }
        }

        private async Task<double?> GetCpuUsageForProcess(string name, TimeSpan interval, CancellationToken ct)
        {
            var listOfProcesses = Process.GetProcesses().Where(x => x.ProcessName.ToLower().Equals(name)).ToList();

            if (listOfProcesses.Count == 0)
            {
                return null;
            }

            if (this.startTime == null)
            {
                this.startTime = DateTime.UtcNow;
            }

            if (this.startCpuUsage == null)
            {
                this.startCpuUsage = listOfProcesses.Sum(x => x.TotalProcessorTime.TotalMilliseconds);
            }

            await Task.Delay(interval);

            var endCpuUsage = listOfProcesses.Sum(x => x.TotalProcessorTime.TotalMilliseconds);
            var endTime = DateTime.UtcNow;

            var cpuUsedMs = endCpuUsage - this.startCpuUsage.Value;
            var totalMsPassed = (endTime - this.startTime.Value).TotalMilliseconds;
            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

            this.startTime = endTime;
            this.startCpuUsage = endCpuUsage;

            return Math.Round(cpuUsageTotal * 100, 2);
        }
    }
}
