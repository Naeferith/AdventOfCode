using AdventOfCode.Core.Components;

namespace AdventOfCode.V2022.Days
{
    internal class Day3 : IDay
    {
        public int DayNumber => 3;

        public string PuzzleName => "Rucksack Reorganization";

        public string Solution1(string[] lines)
        {
            return lines
                .Select(l => (l[..(l.Length / 2)], l[(l.Length / 2)..]))
                .Sum(b => GetPriority(b.Item1.Intersect(b.Item2).Single()))
                .ToString();
        }

        public string Solution2(string[] lines)
        {
            return lines
                .Chunk(3)
                .Sum(t => GetPriority(t[0].Intersect(t[1]).Intersect(t[2]).Single()))
                .ToString();
        }

        private static int GetPriority(char c)
        {
            if (c >= 'a')
                c -= '`';
            else
            {
                c -= 'A';
                c += '\u001b';
            }

            return c;
        }
    }
}
