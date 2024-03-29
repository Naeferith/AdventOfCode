﻿using AdventOfCode.Core.Components;

namespace AdventOfCode.V2021.Days
{
    internal class Day7 : IDay
    {
        public int DayNumber => 7;

        public string PuzzleName => "The Treachery of Whales";

        public string Solution1(string[] lines)
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

        public string Solution2(string[] lines)
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
