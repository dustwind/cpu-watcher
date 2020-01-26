using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CPUWatcher
{
    class Program
    {
        static void Main(string[] args)
        {
            SetWatcher();
        }

        private static void SetWatcher()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;
            int interval;

            Console.WriteLine("===============");
            Console.WriteLine("Set watcher for processes");

            string input;
            do
            {
                Console.Write("Enter interval in sec: ");
                input = Console.ReadLine();
            } while (!int.TryParse(input, out interval) || (interval < 0));

            Console.WriteLine("Enter names separated by commas");

            input = Console.ReadLine();
            List<string> processes = input.Split(',').Select(p => p.Trim().ToLower()).ToList();

            List<ProcessCPUWatcher> watching = new List<ProcessCPUWatcher>();
            foreach (var p in processes)
            {
                watching.Add(new ProcessCPUWatcher(p, TimeSpan.FromSeconds(interval), token));
            }

            Console.WriteLine("Press <E> to abort CPU watcher");

            do
            {
            } while (Console.ReadKey(true).Key != ConsoleKey.E);

            tokenSource.Cancel();
            watching.Clear();

            Console.WriteLine();
            SetWatcher();
        }
    }
}
