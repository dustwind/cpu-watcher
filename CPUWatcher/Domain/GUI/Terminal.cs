using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.GUI
{
    public class Terminal: IGui
    {
        public void ShowInfo(List<string> information)
        {
            foreach (var info in information)
            {
                Console.WriteLine(info);
            }
        }

        public int GetInteger(string text, int greaterCondition = int.MaxValue, int lessCondition = 0)
        {
            string input;
            int result;

            do
            {
                Console.Write(text);
                input = Console.ReadLine();
            }
            while (!int.TryParse(input, out result) || (result > greaterCondition) || (result < lessCondition));

            return result;
        }

        public List<string> GetStringArray(string text, char? separator = null)
        {
            Console.WriteLine(text);

            var input = Console.ReadLine();
            if (separator != null)
            {
                return input.Split(separator.Value).Select(p => p.Trim().ToLower()).ToList();
            }
            else
            {
                return new List<string> { input };
            }
        }

        public void WaitKey(string text, string key, Action callback)
        {
            Console.WriteLine(text);

            var consoleKey = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), key);

            while (Console.ReadKey(true).Key != consoleKey)
            {
            }

            Console.WriteLine();
            callback();
        }
    }
}
