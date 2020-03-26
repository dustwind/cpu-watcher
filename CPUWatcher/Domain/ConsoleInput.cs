using System;

namespace Domain
{
    public static class ConsoleInput
    {
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
    }
}
