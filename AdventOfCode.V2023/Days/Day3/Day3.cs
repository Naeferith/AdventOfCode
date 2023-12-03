using AdventOfCode.Core.Components;
using Nae.Utils.Maths;
using System.Text.RegularExpressions;

namespace AdventOfCode.V2023.Days.Day3
{
    internal partial class Day3 : IDay
    {
        private readonly Regex _numRegex = NumberReger();

        public int DayNumber => 3;

        public string PuzzleName => "Gear Ratios";

        public string Solution1(string[] lines)
        {
            var partRegex = PartRegex();
            var aloneNumbers = new HashSet<Match>();

            var pendingNumbers = new List<Match>();
            var previousIndicies = new List<int>();

            for (int i = 0; i < lines.Length; i++)
            {
                // Fetch numbers and parts
                var lineNumbers = _numRegex.Matches(lines[i]).Cast<Match>().ToList();
                var lineIncicies = partRegex.Matches(lines[i]).Cast<Match>().Select(m => m.Index).ToList();

                // Compare previous parts -> new numbers
                Compare(aloneNumbers, lineNumbers, previousIndicies);

                // Compare previous numbers -> new parts
                Compare(aloneNumbers, pendingNumbers, lineIncicies);

                // Compare within the line
                Compare(aloneNumbers, lineNumbers, lineIncicies);

                // Pending tokens are our of reach, replace them
                pendingNumbers = lineNumbers;
                previousIndicies = lineIncicies;
            }

            return aloneNumbers
                .Select(m => int.Parse(m.ValueSpan))
                .Sum()
                .ToString();
        }

        public string Solution2(string[] lines)
        {
            var sum = 0;

            for (int i = 1; i < lines.Length - 1; i++)
            {
                var gears = lines[i]
                    .Select((c, idx) => new { c, idx})
                    .Where(c => c.c == '*')
                    .Select(c => c.idx)
                    .ToList();

                var numbers = _numRegex.Matches(lines[i-1]).Cast<Match>()
                    .Concat(_numRegex.Matches(lines[i]).Cast<Match>())
                    .Concat(_numRegex.Matches(lines[i+1]).Cast<Match>())
                    .ToList();

                foreach (var gearIdx in gears)
                {
                    var adjacentNumbers = numbers
                        .Where(m => gearIdx.IsBetweenInclusive(m.Index - 1, m.Index + m.Length))
                        .Select(m => int.Parse(m.ValueSpan))
                        .ToList();

                    if (adjacentNumbers.Count == 2)
                    {
                        sum += adjacentNumbers[0] * adjacentNumbers[1];
                    }
                }
            }

            return sum.ToString();
        }

        private static void Compare(HashSet<Match> aloneNumbers, List<Match> lineNumbers, List<int> lineIncicies)
        {
            foreach (var match in lineNumbers.Where(m => lineIncicies.Exists(i => i.IsBetweenInclusive(m.Index - 1, m.Index + m.Length))).ToList())
            {
                aloneNumbers.Add(match);
                lineNumbers.Remove(match);
            }
        }

        [GeneratedRegex(@"[^\s0-9.]")]
        private static partial Regex PartRegex();

        [GeneratedRegex(@"\d+")]
        private static partial Regex NumberReger();
    }
}
