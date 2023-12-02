using AdventOfCode.Core.Components;

namespace AdventOfCode.V2023.Days.Day2
{
    internal class Day2 : IDay
    {
        public int DayNumber => 2;

        public string PuzzleName => "Cube Conundrum";

        public string Solution1(string[] lines)
        {
            return lines
                .Select((l, i) => new { Id = i + 1, Sets = ParseSets(l) })
                .Where(a => a.Sets.All(s => s.Red <= 12 && s.Blue <= 14 && s.Green <= 13))
                .Select(a => a.Id)
                .Sum()
                .ToString();
        }

        public string Solution2(string[] lines)
        {
            return lines
                .Select(ParseSets)
                .Select(s => s.Max(s => s.Blue) * s.Max(s => s.Red) * s.Max(s => s.Green))
                .Sum()
                .ToString();
        }

        private static IEnumerable<Set> ParseSets(string l)
        {
            return l.Split(": ")[1].Split("; ").Select(s => new Set(s));
        }
    }
}
