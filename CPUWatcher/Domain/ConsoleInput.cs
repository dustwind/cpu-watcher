using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain
{
    public static class ConsoleInput
    {
        public static void ShowLine(string text = "")
        {
            Console.WriteLine(text);
        }

        public static int GetInteger(string text, int greaterCondition = int.MaxValue, int lessCondition = 0)
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

        public static List<string> GetStringArray(string text, char? separator = null)
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

        public static void WaitKey(string text, ConsoleKey key, Action callback)
        {
            Console.WriteLine(text);

            while (Console.ReadKey(true).Key != key)
            {
            }

            Console.WriteLine();
            callback();
        }
    }
}
