using AdventOfCode.Core.Components;

namespace AdventOfCode.V2022.Days
{
    internal class Day4 : Day
    {
        public override int DayNumber => 4;

        public override string PuzzleName => "Camp Cleanup";

        protected override string Solution1(string[] lines)
        {
            return Solution(lines, p => p.Item2 - p.Item1, SetIncluded);
        }

        protected override string Solution2(string[] lines)
        {
            return Solution(lines, p => p.Item1, Overlaps);
        }

        private static string Solution(string[] lines,
            Func<(int, int), int> order,
            Func<(int, int), (int, int), bool> predicate)
        {
            return lines
                .Select(l =>
                    l.Split(',')
                    .Select(r => r.Split('-').Select(int.Parse))
                    .Select(i => (i.First(), i.Last()))
                    .OrderBy(order)
                )
                .Count(p => predicate(p.First(), p.Last()))
                .ToString();
        }

        private static bool SetIncluded((int, int) min, (int, int) max)
        {
            return max.Item1 <= min.Item1 && max.Item2 >= min.Item2;
        }

        private static bool Overlaps((int, int) min, (int, int) max)
        {
            return min.Item2 >= max.Item1;
        }
    }
}
