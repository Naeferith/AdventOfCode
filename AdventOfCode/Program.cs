using AdventOfCode.Core.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdventOfCode
{
    static class Program
    {
        static void Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureApplication()
                .Build();

            var calendar = host.Services.GetRequiredService<ICalendar>();
            var input = host.Services.GetRequiredService<IInputAccessor>();

            calendar.PrintCalendar(Console.Out, input);

            Console.ReadKey();
        }

        private static string ExcelCol(int nb)
        {
            const int BASE = 26;

            var s = new Stack<char>();

            do
            {
                s.Push((char)((nb % BASE) + 'A'));
                nb /= BASE;
            } while (nb > 0);

            return new string(s.ToArray());
        }
    }
}
