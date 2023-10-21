using AdventOfCode.Core.AoC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.V2021.Days
{
    internal sealed partial class AoC21
    {
        internal class Day7 : Day
        {
            public override string PuzzleName => "The Treachery of Whales";

            protected override string Solution1(string[] lines)
            {
                var crabs = lines.Select(t => t.Split(',').Select(n => int.Parse(n))).First();
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

            protected override string Solution2(string[] lines)
            {
                var crabs = lines.Select(t => t.Split(',').Select(n => int.Parse(n))).First();
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
