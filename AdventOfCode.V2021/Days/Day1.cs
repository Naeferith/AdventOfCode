using AdventOfCode.Core.Components;

namespace AdventOfCode.V2021.Days
{
    internal class Day1 : IDay
    {
        public int DayNumber => 1;

        public string PuzzleName => "Sonar Sweep";

        public string Solution1(string[] lines)
        {
            var depths = lines.Select(int.Parse);
            var previous = depths.First();
            var count = 0;

            foreach (var depth in depths)
            {
                count += depth > previous ? 1 : 0;
                previous = depth;
            }

            return count.ToString();
        }

        public string Solution2(string[] lines)
        {
            const int SLIDE_SIZE = 3;

            var depths = lines.Select(int.Parse);
            var previous = depths.Take(SLIDE_SIZE).Sum();
            var count = 0;

            for (int i = 1; i < depths.Count() - (SLIDE_SIZE - 1); i++)
            {
                var val = depths.Skip(i).Take(SLIDE_SIZE).Sum();
                count += val > previous ? 1 : 0;
                previous = val;
            }

            return count.ToString();
        }
    }
}
