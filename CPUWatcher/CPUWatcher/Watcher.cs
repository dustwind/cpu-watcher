using System;
using System.Collections.Generic;
using System.Threading;
using Domain;

namespace CPUWatcher
{
    public class Watcher
    {
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private List<ProcessCPUWatcher> watching = new List<ProcessCPUWatcher>();

        public void Start()
        {
            SetWatcher();
        }

        private void SetWatcher()
        {
            var token = tokenSource.Token;

            Console.WriteLine("===============");
            Console.WriteLine("Set watcher for processes");

            int interval = ConsoleInput.GetInteger("Enter interval in sec: ");
            List<string> processes = ConsoleInput.GetString("Enter names separated by commas", ',');

            foreach (var p in processes)
            {
                watching.Add(new ProcessCPUWatcher(p, TimeSpan.FromSeconds(interval), token));
            }

            ConsoleInput.WaitKey("Press <E> to abort CPU watcher", ConsoleKey.E, AbortWatching);
        }

        private void AbortWatching()
        {
            tokenSource.Cancel();
            watching.Clear();

            SetWatcher();
        }
    }
}
