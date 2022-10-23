using AdventOfCode.Core.AoC;
using System;
using System.Collections.Generic;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(ExcelCol(28));
            var calendar = AoC.GetCalendar(2021);
            Console.WriteLine(calendar.GetPuzzle(22, 2));
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
