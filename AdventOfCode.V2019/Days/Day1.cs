﻿using AdventOfCode.Core.Components;

namespace AdventOfCode.V2019.Days
{
    internal class Day1 : Day
    {
        public override int DayNumber => 1;

        public override string PuzzleName => "The Tyranny of the Rocket Equation";

        protected override string Solution1(string[] lines)
        {
            return lines
                .Select(int.Parse)
                .Sum(RequiredFuel)
                .ToString();
        }

        protected override string Solution2(string[] lines)
        {
            return lines
                .Select(int.Parse)
                .Sum(RecursiveRequiredFuel)
                .ToString();
        }

        private static int RequiredFuel(int mass)
        {
            return mass / 3 - 2;
        }

        private static int RecursiveRequiredFuel(int mass)
        {
            var fuel = RequiredFuel(mass);

            if (fuel <= 0)
                return 0;

            return fuel + RecursiveRequiredFuel(fuel);
        }
    }
}