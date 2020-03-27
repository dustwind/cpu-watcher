using System;

namespace CPUWatcher
{
    class ProcessUnit
    {
        public ProcessUnit(int processId, DateTime startTime, double startCPU)
        {
            ProcessId = processId;
            StartTime = startTime;
            StartCpuUsage = startCPU;
        }

        public int ProcessId { get; }

        public DateTime StartTime { get; set; }

        public double StartCpuUsage { get; set; }
    }
}
