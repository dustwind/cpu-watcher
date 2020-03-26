using System;
using System.Collections.Generic;
using Domain;

namespace CPUWatcher
{
    public class Watcher
    {
        private List<CPUWatcher> watching = new List<CPUWatcher>();

        private System.Timers.Timer intervalWatcher;

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

            watching = new List<CPUWatcher>();
            foreach (var p in processes)
            {
                watching.Add(new CPUWatcher(p, ConsoleInput.ShowLine));
            }

            intervalWatcher = new System.Timers.Timer(interval * 1000);
            intervalWatcher.Elapsed += Timer_Elapsed;
            intervalWatcher.Enabled = true;

            ConsoleInput.WaitKey("Press <E> to abort CPU watcher", ConsoleKey.E, AbortWatching);
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimerEventClass.RaiseElapseTimerEvent();
        }

        private void AbortWatching()
        {
            intervalWatcher.Dispose();
            watching.ForEach(x => x.Dispose());
            watching = null;

            SetWatcher();
        }
    }
}
