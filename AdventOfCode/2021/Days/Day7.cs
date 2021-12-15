using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Business
{
    internal sealed partial class AoC21
    {
        private static class Day7
        {
            public static string Solution1()
            {
                var crabs = File.ReadLines($"{INPUT_PATH}day7.txt").Select(t => t.Split(',').Select(n => int.Parse(n))).First();
                var max = crabs.Max();

                var dict = new Dictionary<int, int>();

                for (int i = 0; i <= max; i++)
                {
                    var sum = 0;
                    foreach (var crab in crabs)
                        sum += Math.Abs(crab - i);
                    dict[i] = sum;
                }

                return dict.Values.Min().ToString();
            }
            public static string Solution2()
            {
                var crabs = File.ReadLines($"{INPUT_PATH}day7.txt").Select(t => t.Split(',').Select(n => int.Parse(n))).First();
                var max = crabs.Max();

                var dict = new Dictionary<int, int>();

                for (int i = 0; i <= max; i++)
                {
                    var sum = 0;
                    foreach (var crab in crabs)
                        sum += CrabFuel(Math.Abs(crab - i));
                    dict[i] = sum;
                }

                return dict.Values.Min().ToString();
            }
            private static int CrabFuel(int delta)
                => delta == 0 ? 0 : delta + CrabFuel(--delta);
        }
    }
}
