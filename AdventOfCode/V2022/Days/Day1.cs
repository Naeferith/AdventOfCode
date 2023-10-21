using AdventOfCode.Core.AoC;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.V2022.Days
{
    internal class Day1 : Day
    {
        public override string PuzzleName => "Calorie Counting";

        protected override string Solution1(string[] lines)
        {
            return GetCalories(lines).Max().ToString();
        }

        protected override string Solution2(string[] lines)
        {
            var elves = GetCalories(lines);
            elves.Sort();
            return elves.TakeLast(3).Sum().ToString();
        }

        private static List<int> GetCalories(string[] lines)
        {
            var elves = new List<int>();
            var calories = 0;

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    elves.Add(calories);
                    calories = 0;
                }
                else
                {
                    calories += int.Parse(line);
                }
            }

            return elves;
        }
    }
}
