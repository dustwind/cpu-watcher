using System;

namespace CPUWatcher
{
    public static class TimerEventClass
    {
        public static event EventHandler ElapseTimerEvent;

        public static void RaiseElapseTimerEvent()
        {
            ElapseTimerEvent?.Invoke(typeof(TimerEventClass), EventArgs.Empty);
        }
    }
}
