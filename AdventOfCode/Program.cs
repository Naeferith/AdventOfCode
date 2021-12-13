using System;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            var calendar = AoC.GetCalendar(2021);
            Console.WriteLine(calendar.Day1_1());
            Console.ReadKey();
        }
    }
}
