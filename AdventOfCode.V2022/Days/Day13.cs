using AdventOfCode.Core.Components;
using AdventOfCode.V2022.Core.Day13;

namespace AdventOfCode.V2022.Days
{
    internal class Day13 : Day
    {
        private static readonly SignalComparer _comparer = new();

        public override int DayNumber => 13;

        public override string PuzzleName => "Distress Signal";

        protected override string Solution1(string[] lines)
        {
            return lines
                .Where(s => !string.IsNullOrEmpty(s))
                .Chunk(2)
                .Select((p, i) => _comparer.Compare(p[0], p[1]) >= 0 ? i + 1 : 0)
                .Sum()
                .ToString();
        }

        protected override string Solution2(string[] lines)
        {
            const string d1 = "[[2]]";
            const string d2 = "[[6]]";

            var signals = lines
                .Where(s => !string.IsNullOrEmpty(s))
                .Append(d1)
                .Append(d2)
                .OrderDescending(_comparer)
                .ToArray();

            return ((Array.IndexOf(signals, d1) + 1) * (Array.IndexOf(signals, d2) + 1)).ToString();
        }
    }
}
