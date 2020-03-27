using System;
using System.Collections.Generic;
using Domain;

namespace CPUWatcher
{
    public class Watcher
    {
        private List<ProcessWatcher> watchers = new List<ProcessWatcher>();

        private System.Timers.Timer watcherTimer;

        public void Start()
        {
            SetWatcher();
        }

        private void SetWatcher()
        {
            ConsoleInput.ShowLine("===============");
            ConsoleInput.ShowLine("Set watcher for processes");

            var interval = ConsoleInput.GetInteger("Enter interval in sec: ");
            var processes = ConsoleInput.GetString("Enter names separated by commas", ',');

            foreach (var p in processes)
            {
                watchers.Add(new ProcessWatcher(p, ConsoleInput.ShowLine));
            }

            watcherTimer = new System.Timers.Timer(interval * 1000);
            watcherTimer.Elapsed += WatcherTimer_Elapsed;
            watcherTimer.Enabled = true;

            ConsoleInput.WaitKey("Press <E> to abort CPU watcher", ConsoleKey.E, AbortWatcher);
        }

        private void WatcherTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            EventClass.RaiseElapseTimerEvent();
        }

        private void AbortWatcher()
        {
            watcherTimer.Dispose();

            EventClass.RaiseDisposeEvent();
            watchers = new List<ProcessWatcher>();

            SetWatcher();
        }
    }
}
