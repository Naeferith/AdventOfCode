using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var calendar = AoC.GetCalendar(2021);
            Console.WriteLine(calendar.GetPuzzle(16, 2));
            Console.ReadKey();
        }
    }
}
