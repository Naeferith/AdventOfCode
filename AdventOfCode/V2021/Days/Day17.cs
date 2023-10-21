using AdventOfCode.Core.AoC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.V2021.Days
{
    internal class Day17 : Day
    {
        public override string PuzzleName => "Trick Shot";

        protected override string Solution1(string[] lines)
        {
            var coords = lines[0].Split(',').Select(a => a.Split('=')[1].Split(".."))
                .Select(k => (int.Parse(k[0]), int.Parse(k[1])));
            var areaY = coords.Last();

            return Distance(-areaY.Item1 - 1).ToString();
        }

        protected override string Solution2(string[] lines)
        {
            var coords = lines[0].Split(',').Select(a => a.Split('=')[1].Split(".."))
                .Select(k => (int.Parse(k[0]), int.Parse(k[1])));

            var areaX = coords.First();
            var areaY = coords.Last();

            var xs = new List<(int, int)>();

            /*for (int x = areaX.Item1; x <= areaX.Item2; x++)
            {
                for (int i = 1; true; i++)
                {
                    var quotient = x / i;
                    //var friction = Math.Max(0, 2 * (i - 1) - 1);
                    var distance = Distance(quotient, i);

                    if (distance >= areaX.Item1 && distance <= areaX.Item2)
                        xs.Add((quotient, i));
                    else
                        break;
                }
            }*/

            for (int i = 0; i < areaX.Item2; i++)
            {
                for (int j = 0; j < areaX.Item2; j++)
                {
                    var distance = Distance(i, j);

                    if (distance >= areaX.Item1 && distance <= areaX.Item2)
                        xs.Add((i, j));
                }
            }

            var e = xs.Distinct().GroupBy(g => g.Item2).OrderByDescending(g => g.Key);

            var c = 0;

            foreach (var group in e)
            {
                var max = Math.Abs(areaY.Item1);
                for (int i = -max; i < max; i++)
                {
                    var y = Gravity(i, group.Key);

                    if (y >= areaY.Item1 && y <= areaY.Item2)
                    {
                        c += group.Count();
                        break;
                    }

                }
            }

            return string.Empty;
        }

        private static int Distance(int speed) => speed == 0 ? 0 : speed + Distance(speed - 1);
        private static int Distance(int speed, int step) => step == 0 || speed == 0 ? 0 : speed + Distance(speed - 1, step - 1);

        private static int Gravity(int speed, int step) => step == 0 ? 0 : speed + Gravity(speed - 1, step - 1);
    }
}
