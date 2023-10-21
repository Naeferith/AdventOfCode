using AdventOfCode.Core.Components;
using Nae.Utils.Structs;

namespace AdventOfCode.V2022.Days
{
    internal class Day15 : Day
    {
        private IDictionary<Point, List<Point>> _beacons;

        public override int DayNumber => 15;

        public override string PuzzleName => "Beacon Exclusion Zone";

        protected override string Solution1(string[] lines)
        {
            const int ROW_Y = 2000000;

            Initialize(lines);

            var absencePoints = new HashSet<Point>();

            foreach (var beacon in _beacons)
            {
                foreach (var signal in beacon.Value)
                {
                    var maxDistance = Point.ManhattanDistance(beacon.Key, signal);
                    var dY = Math.Abs(ROW_Y - signal.Y);
                    var workingDistance = maxDistance - dY;

                    for (int x = signal.X - workingDistance; x <= signal.X + workingDistance; x++)
                    {
                        var point = new Point(x, ROW_Y);

                        if (!_beacons.ContainsKey(point))
                            absencePoints.Add(point);
                    }
                }
            }

            return absencePoints.Count.ToString();
        }

        protected override string Solution2(string[] lines)
        {
            Initialize(lines);

            throw new NotImplementedException();
        }

        private void Initialize(string[] lines)
        {
            _beacons = lines
                .Select(l =>
                {
                    var c = l
                    .Split(':')
                    .Select(p => p[p.IndexOf('x')..])
                    .ToArray();

                    return (BuildFromCoords(c[0]), BuildFromCoords(c[1]));
                })
                .GroupBy(t => t.Item2)
                .ToDictionary(g => g.Key, g => g.Select(g => g.Item1).ToList());
        }

        private static Point BuildFromCoords(string str)
        {
            var coords = str.Split(',');

            return new Point(
                int.Parse(coords[0][(coords[0].IndexOf('=') + 1)..]),
                int.Parse(coords[1][(coords[1].IndexOf('=') + 1)..])
                );
        }
    }
}
