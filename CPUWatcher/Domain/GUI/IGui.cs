using System;
using System.Collections.Generic;

namespace Domain.GUI
{
    public interface IGui
    {
        void ShowInfo(List<string> information);

        int GetInteger(string text, int greaterCondition = int.MaxValue, int lessCondition = 0);

        List<string> GetStringArray(string text, char? separator = null);

        void WaitKey(string text, string key, Action callback);
    }
}
