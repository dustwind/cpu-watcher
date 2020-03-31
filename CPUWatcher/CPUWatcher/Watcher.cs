using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Domain;

namespace CPUWatcher
{
    public class Watcher
    {
        private List<ProcessWatcher> watchers = new List<ProcessWatcher>();

        private System.Timers.Timer watcherTimer;

        private List<Process> ListOfProcesses = new List<Process>();

        public event EventHandler<List<Process>> OnElapseTimerEvent;

        public event EventHandler OnAbortWatcher;

        public void Start()
        {
            SetWatcher();
        }

        private void SetWatcher()
        {
            ConsoleInput.ShowLine("===============");
            ConsoleInput.ShowLine("Set watcher for processes");

            var interval = ConsoleInput.GetInteger("Enter interval in sec: ");
            var processes = ConsoleInput.GetStringArray("Enter names separated by commas", ',');

            foreach (var p in processes)
            {
                watchers.Add(new ProcessWatcher(this, p, ConsoleInput.ShowLine));
            }

            RefreshProcesses();

            watcherTimer = new System.Timers.Timer(interval * 1000);
            watcherTimer.Elapsed += (s, e) =>
            {
                RefreshProcesses();
            };
            watcherTimer.Enabled = true;

            ConsoleInput.WaitKey("Press <E> to abort CPU watcher", ConsoleKey.E, AbortWatcher);
        }

        private void RefreshProcesses()
        {
            ListOfProcesses = Process.GetProcesses().ToList();

            OnElapseTimerEvent?.Invoke(typeof(Watcher), ListOfProcesses);
        }

        private void AbortWatcher()
        {
            watcherTimer.Dispose();

            OnAbortWatcher?.Invoke(typeof(Watcher), EventArgs.Empty);
            watchers = new List<ProcessWatcher>();

            SetWatcher();
        }
    }
}
