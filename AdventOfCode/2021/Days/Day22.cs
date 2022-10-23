using Octree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.V2021
{
    internal class Day22 : IDay
    {
        public string PuzzleName => throw new System.NotImplementedException();

        public string Solution1(string[] lines)
        {
            bool[,,] core = new bool[100,100,100];

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

        public string Solution2(string[] lines)
        {
            var core = new List<Cube>();

            foreach (var line in lines)
            {
                var status = line.StartsWith("on");
                var coords = line.Split(' ')[1].Split(',')
                    .Select(s => s[2..]).Select(s => (int.Parse(s.Split("..")[0]), int.Parse(s.Split("..")[1]))).ToArray();

                var dx = coords[0].Item2 - coords[0].Item1;
                var dy = coords[1].Item2 - coords[1].Item1;
                var dz = coords[2].Item2 - coords[2].Item1;

                var center = new Point(coords[0].Item1 + dx / 2f, coords[1].Item1 + dy / 2f, coords[2].Item1 + dz / 2f);
                var size = new Point(dx, dy, dz);

                core.Add(new Cube(center, size, status));
            }

            var tree = new BoundsOctree<bool>(300000, Point.Zero, 1, 1.9f);

            foreach (var cube in core.OrderByDescending(c => c.Status))
            {
                tree.Add(cube.Status, new BoundingBox(cube.Center, cube.Size));
            }

            var l = new List<bool>();

            tree.GetColliding(l, tree.MaxBounds);

            return null;
        }

        public class Cube
        {
            public Range X { get; }
            public Range Y { get; }
            public Range Z { get; }

            public Point Center { get; }
            public Point Size { get; }

            public bool Status { get; }

            public Cube(Range x, Range y, Range z, bool val)
            {
                X = x;
                Y = y;
                Z = z;
                Status = val;
            }

            public Cube(Point center, Point size, bool val)
            {
                Center = center;
                Size = size;
                Status = val;
            }
        }
    }
}
