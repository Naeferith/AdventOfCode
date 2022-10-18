using AdventOfCode.Core;
using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var calendar = AoC.GetCalendar(2021);
            Console.WriteLine(calendar.GetPuzzle(17, 1));
            Console.ReadKey();
        }
    }
}
