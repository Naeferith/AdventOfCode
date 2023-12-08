using AdventOfCode.Core.Components;
using Nae.Utils.Maths;

namespace AdventOfCode.V2023.Days.Day8
{
    internal class Day8 : IDay
    {
        public int DayNumber => 8;

        public string PuzzleName => "Haunted Wasteland";

        public string Solution1(string[] lines)
        {
            var directions = lines[0];

            var paths = lines
                .Skip(2)
                .ToDictionary(
                l => l[..3],
                l =>
                {
                    var args = l[7..15].Split(", ");

                    return new { Left = args[0], Right = args[1] };
                });

            var current = "AAA";

            for (var i = 0; ; i++)
            {
                if (current == "ZZZ")
                {
                    return i.ToString();
                }

                var idx = i % directions.Length;

                current = directions[idx] == 'R'
                    ? paths[current].Right
                    : paths[current].Left;
            }
        }

        public string Solution2(string[] lines)
        {
            var directions = lines[0];

            var paths = lines
                .Skip(2)
                .ToDictionary(
                l => l[..3],
                l =>
                {
                    var args = l[7..15].Split(", ");

                    return new { Left = args[0], Right = args[1] };
                });

            var currents = paths.Keys.Where(kvp => kvp.EndsWith('A')).ToArray();

            var lcms = new List<long>();

            for (int j = 0; j < currents.Length; j++)
            {
                var knownStates = new List<State>();
                var current = currents[j];

                for (var i = 0; ; i++)
                {
                    var idx = i % directions.Length;

                    current = directions[idx] == 'R'
                        ? paths[current].Right
                        : paths[current].Left;

                    if (current.EndsWith('Z'))
                    {
                        if (knownStates.Exists(s => current == s.CurrentPath && idx == s.DirIndex))
                        {
                            break;
                        }

                        knownStates.Add(new(i + 1, idx, current));
                    }
                }

                lcms.AddRange(knownStates.Select(s => (long)s.Iteration));
            }

            return MathI.Lcm(lcms).ToString();
        }
    }

    internal record State(int Iteration, int DirIndex, string CurrentPath);
}
