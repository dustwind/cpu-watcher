using System;

namespace CPUWatcher
{
    public static class EventClass
    {
        public static event EventHandler ElapseTimerEvent;

        public static event EventHandler DisposeEvent;

        public static void RaiseElapseTimerEvent()
        {
            ElapseTimerEvent?.Invoke(typeof(EventClass), EventArgs.Empty);
        }

        public static void RaiseDisposeEvent()
        {
            DisposeEvent?.Invoke(typeof(EventClass), EventArgs.Empty);
        }
    }
}
