using Domain.GUI;

namespace CPUWatcher
{
    class Program
    {
        private static void Main(string[] args)
        {
            new Controller(new Terminal());
        }
    }
}
