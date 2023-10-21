using AdventOfCode.Core.AoC;
using System;
using System.Linq;

namespace AdventOfCode.V2021.Days
{
    internal class Day22 : Day
    {
        public override string PuzzleName => throw new NotImplementedException();

        protected override string Solution1(string[] lines)
        {
            bool[,,] core = new bool[100, 100, 100];

            foreach (var line in lines)
            {
                var status = line.StartsWith("on");
                var coords = line.Split(' ')[1].Split(',')
                    .Select(s => s[2..]).Select(s => (int.Parse(s.Split("..")[0]), int.Parse(s.Split("..")[1]))).ToArray();

                if (coords.Any(c => c.Item1 > 50 || c.Item2 > 50))
                    break; // Because input is nicely formated, else "continue"

                for (int x = coords[0].Item1; x <= coords[0].Item2; x++)
                {
                    for (int y = coords[1].Item1; y <= coords[1].Item2; y++)
                    {
                        for (int z = coords[2].Item1; z <= coords[2].Item2; z++)
                        {
                            core[x + 50, y + 50, z + 50] = status;
                        }
                    }
                }
            }

            var sum = 0;
            foreach (var cube in core)
            {
                sum += cube ? 1 : 0;
            }

            return sum.ToString();
        }

        protected override string Solution2(string[] lines)
        {
            throw new NotImplementedException();
        }
    }
}
