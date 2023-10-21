using AdventOfCode.Core.AoC;
using System;
using System.Linq;

namespace AdventOfCode.V2021.Days
{
    internal class Day5 : Day
    {
        public override string PuzzleName => "Hydrothermal Venture";

        protected override string Solution1(string[] lines)
        {
            var rows = lines.Select(l =>
            {
                var p = l.Split(" -> ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                return (Point.Parse(p[0]), Point.Parse(p[1]));
            });

            int[,] grid = new int[1000, 1000];

            foreach (var line in rows)
            {
                var delta = line.Item2 - line.Item1;

                if (delta.X != 0 && delta.Y != 0)
                    continue;

                grid[line.Item1.X, line.Item1.Y]++;

                while (!delta.IsNull)
                {
                    if (delta.X != 0)
                    {
                        var u = delta.X / Math.Abs(delta.X);
                        delta.X -= u;
                        line.Item1.X += u;
                    }
                    if (delta.Y != 0)
                    {
                        var u = delta.Y / Math.Abs(delta.Y);
                        delta.Y -= u;
                        line.Item1.Y += u;
                    }

                    grid[line.Item1.X, line.Item1.Y]++;
                }
            }

            var count = 0;
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if (grid[x, y] > 1)
                        count++;
                }
            }

            return count.ToString();
        }

        protected override string Solution2(string[] lines)
        {
            var rows = lines.Select(l =>
            {
                var p = l.Split(" -> ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                return (Point.Parse(p[0]), Point.Parse(p[1]));
            });

            int[,] grid = new int[1000, 1000];

            foreach (var line in rows)
            {
                var delta = line.Item2 - line.Item1;

                grid[line.Item1.X, line.Item1.Y]++;

                while (!delta.IsNull)
                {
                    if (delta.X != 0)
                    {
                        var u = delta.X / Math.Abs(delta.X);
                        delta.X -= u;
                        line.Item1.X += u;
                    }
                    if (delta.Y != 0)
                    {
                        var u = delta.Y / Math.Abs(delta.Y);
                        delta.Y -= u;
                        line.Item1.Y += u;
                    }

                    grid[line.Item1.X, line.Item1.Y]++;
                }
            }

            var count = 0;
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if (grid[x, y] > 1)
                        count++;
                }
            }

            return count.ToString();
        }

        private class Point
        {
            public int X { get; set; }
            public int Y { get; set; }

            public bool IsNull => X == 0 && Y == 0;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public static Point Parse(string str)
            {
                var pos = str.Split(',');
                return new(int.Parse(pos[0]), int.Parse(pos[1]));
            }

            public static Point operator -(Point a, Point b) => new(a.X - b.X, a.Y - b.Y);
        }
    }
}
