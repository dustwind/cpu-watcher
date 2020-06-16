using System;
using System.Collections.Generic;
using Domain.GUI;

namespace CPUConsumer
{
    public class Controller
    {
        private readonly IGui _gui;
        private Consumer consumer;

        public Controller(IGui gui)
        {
            _gui = gui;
            consumer = new Consumer();

            Start();
        }

        private void Start()
        {
            consumer.Abort();

            var processorCount = Environment.ProcessorCount;

            _gui.ShowInfo(new List<string>
            {
                "===============",
                "Set CPU Consume",
                $"Cpu available {processorCount}",
            });

            var cpuCount = _gui.GetInteger("Enter cpu count: ", processorCount);
            var cpuUsage = _gui.GetInteger("Enter cpu usage: ", 100);

            _gui.ShowInfo(new List<string>
            {
                string.Empty,
                $"Consume {cpuCount} CPU, {cpuUsage}% usage",
            });

            consumer.Start(cpuCount, cpuUsage);

            _gui.WaitKey("Press <E> to abort CPU consume", ConsoleKey.E.ToString(), Start);
        }
    }
}
