using AdventOfCode.Core;
using System.Linq;

namespace AdventOfCode.V2021.Days
{
    internal class Day17 : IDay
    {
        public string PuzzleName => "Trick Shot";

        public string Solution1(string[] lines)
        {
            var coords = lines[0].Split(',').Select(a => a.Split('=')[1].Split(".."))
                .Select(k => (int.Parse(k[0]), int.Parse(k[1])));
            var areaY = coords.Last();

            return Distance(-areaY.Item1 - 1).ToString();
        }

        public string Solution2(string[] lines)
        {
            throw new System.NotImplementedException();
        }

        private static int Distance(int speed) => speed == 0 ? 0 : speed + Distance(speed - 1);
    }
}
