using Domain.GUI;

namespace CPUConsumer
{
    class Program
    {
        private static void Main(string[] args)
        {
            new Controller(new Terminal());
        }
    }
}
