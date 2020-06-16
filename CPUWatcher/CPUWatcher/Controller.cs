using System;
using System.Collections.Generic;
using Domain.GUI;

namespace CPUWatcher
{
    public delegate void CPUWatcherHandler(List<string> message);

    public class Controller
    {
        private readonly IGui _gui;
        private Watcher watcher;

        public Controller(IGui gui)
        {
            _gui = gui;
            watcher = new Watcher();

            Start();
        }

        private void Start()
        {
            watcher.Abort();

            _gui.ShowInfo(new List<string>
            {
                "===============",
                "Set watcher for processes",
            });

            var interval = _gui.GetInteger("Enter interval in sec: ");
            var processes = _gui.GetStringArray("Enter names separated by commas", ',');

            watcher.Start(interval, processes, _gui.ShowInfo);

            _gui.WaitKey("Press <E> to abort CPU watcher", ConsoleKey.E.ToString(), Start);
        }
    }
}
