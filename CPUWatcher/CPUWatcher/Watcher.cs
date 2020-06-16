using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CPUWatcher
{
    public class Watcher
    {
        private List<ProcessWatcher> watchers = new List<ProcessWatcher>();

        private System.Timers.Timer watcherTimer;

        private List<Process> ListOfProcesses = new List<Process>();

        public event EventHandler<List<Process>> OnElapseTimerEvent;

        public event EventHandler OnAbortWatcher;

        public void Start(int interval, List<string> processes, CPUWatcherHandler handler)
        {
            foreach (var p in processes)
            {
                watchers.Add(new ProcessWatcher(this, p, handler));
            }

            RefreshProcesses();

            watcherTimer = new System.Timers.Timer(interval * 1000);
            watcherTimer.Elapsed += (s, e) =>
            {
                RefreshProcesses();
            };
            watcherTimer.Enabled = true;
        }

        public void Abort()
        {
            watcherTimer?.Dispose();

            OnAbortWatcher?.Invoke(typeof(Watcher), EventArgs.Empty);
            watchers = new List<ProcessWatcher>();
        }

        private void RefreshProcesses()
        {
            ListOfProcesses = Process.GetProcesses().ToList();

            OnElapseTimerEvent?.Invoke(typeof(Watcher), ListOfProcesses);
        }
    }
}
