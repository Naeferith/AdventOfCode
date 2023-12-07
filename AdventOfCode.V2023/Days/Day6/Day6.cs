using AdventOfCode.Core.Components;

namespace AdventOfCode.V2023.Days.Day6
{
    internal class Day6 : IDay
    {
        public int DayNumber => 6;

        public string PuzzleName => "Wait For It";

        public string Solution1(string[] lines)
        {
            return lines[0]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Select(int.Parse)
                .Zip(lines[1]
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Skip(1)
                    .Select(int.Parse))
                .Aggregate(1, (a, s) => a * Enumerable
                    .Range(0, s.First)
                    .Select(t => t * (s.First - t))
                    .Count(d => d > s.Second))
                .ToString();
        }

        /// <summary>
        /// Easiest way to do it but O(n).
        /// Can easily be made in O(log(n)). Need to optimize it later.
        /// </summary>
        public string Solution2(string[] lines)
        {
            var time = int.Parse(lines[0]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Aggregate((a, s) => a + s));

            var dist = long.Parse(lines[1]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Skip(1)
                .Aggregate((a, s) => a + s));

            return Enumerable
                .Range(0, time)
                .Select(t => (long)t * (time - t))
                .Count(d => d > dist)
                .ToString();
        }
    }
}
