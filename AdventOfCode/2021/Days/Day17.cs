using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.V2021
{
    internal class Day17 : IDay
    {
        public string PuzzleName => throw new System.NotImplementedException();

        public string Solution1(string[] lines) => Solve(lines);

        public string Solution2(string[] lines)
        {
            throw new System.NotImplementedException();
        }

        private static string Solve(string[] lines)
        {
            var coords = lines[0].Split(',').Select(a => a.Split('=')[1].Split(".."))
                .Select(k => (int.Parse(k[0]), int.Parse(k[1])));

            var areaX = coords.First();
            var areaY = coords.Last();
            var s = (0, 0);
            var width = areaX.Item2 - areaX.Item1;
            var height = areaY.Item2 - areaY.Item1;

            var deltaX = (areaX.Item1 - s.Item1, areaX.Item1 - s.Item1 + width);
            var deltaY = (areaY.Item1 - s.Item2, areaY.Item1 - s.Item2 + height);

            int minSteps = 0;
            while (true)
            {
                var distance = Distance(minSteps);

                if (distance >= deltaX.Item1 && distance <= deltaX.Item2)
                    break;
                minSteps++;
            }

            int y = int.MaxValue;

            return string.Empty;
        }

        private static int Gravity(int speed, int steps) => steps == 0 ? 0 : speed + Gravity(speed - 1, steps - 1);

        private static int Distance(int speed) => speed == 0 ? 0 : speed + Distance(speed - 1);

        private class BySteps : IComparer<(int, int)>
        {
            public int Compare((int, int) x, (int, int) y)
            {
                return (x.Item2 < y.Item2) ? 1 : -1;
            }
        }
    }
}
