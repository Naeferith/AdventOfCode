using AdventOfCode.Core.Components;
using System.Collections.Immutable;

namespace AdventOfCode.V2023.Days.Day7
{
    internal class Day7 : IDay
    {
        public int DayNumber => 7;

        public string PuzzleName => "Camel Cards";

        public string Solution1(string[] lines)
        {
            return Solution(lines, false);
        }

        public string Solution2(string[] lines)
        {
            return Solution(lines, true);
        }

        private static string Solution(string[] lines, bool sol2)
        {
            var hands = lines
                .ToImmutableSortedDictionary(
                l => l[..5],
                l => int.Parse(l[5..]),
                new CamelHandComparer(sol2));

            return hands
                .Select((h, i) => h.Value * (i + 1))
                .Sum()
                .ToString();
        }
    }
}
