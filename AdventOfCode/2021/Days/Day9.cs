using System;
using System.Collections.Generic;
#if DEBUG
using System.Drawing;
#endif
using System.Linq;

namespace AdventOfCode.V2021
{
    internal class Day9 : IDay
    {
        public string PuzzleName => "Smoke Basin";

        public string Solution1(string[] lines)
        {
            var rows = lines.Select(l => l.Select(c => c - '0'));
            var grid = new List<List<int>>();
            var points = new List<int>();

            foreach (var row in rows)
                grid.Add(new List<int>(row));

            foreach (var low in GetLows(grid))
                points.Add(grid[low.Y][low.X]);

            return points.Sum(i => i + 1).ToString();
        }

        public string Solution2(string[] lines)
        {
            var rows = lines.Select(l => l.Select(c => c - '0'));
            var grid = new List<List<int>>();
            var bazins = new List<HashSet<Point>>();

            foreach (var row in rows)
                grid.Add(new List<int>(row));

            foreach (var low in GetLows(grid))
            {
                var bazin = new HashSet<Point>
                    {
                        low
                    };
                bazins.Add(GetHighs(low, grid, ref bazin));
            }
#if DEBUG
#pragma warning disable CA1416 // Valider la compatibilité de la plateforme
            if (false)
            {
                using var bmp = new Bitmap(rows.First().Count(), rows.Count());
                using var gfx = Graphics.FromImage(bmp);
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                gfx.Clear(Color.Black);

                int x = 0, y = 0;

                foreach (var line in rows)
                {
                    x = 0;
                    foreach (var num in line)
                    {
                        var brush = (num == 9) ? Brushes.Red : Brushes.Blue;

                        if (num == 9 || bazins.Select(b => b.Contains(new Point(x, y))).Aggregate(false, (b, c) => b |= c))
                            gfx.FillRectangle(brush, x, y, 1, 1);
                        else if (num < 9)
                            Console.WriteLine($"Point : ({x},{y})");
                        x++;
                    }
                    y++;
                }
                bmp.Save("bmp.png");
            }
#pragma warning restore CA1416 // Valider la compatibilité de la plateforme
#endif
            return bazins.OrderByDescending(b => b.Count).Take(3).Select(b => b.Count).Aggregate(1, (s, c) => s * c).ToString();
        }

        private static IEnumerable<Point> GetLows(List<List<int>> grid)
        {
            var ok = true;
            for (int y = 0; y < grid.Count; y++)
            {
                for (int x = 0; x < grid[y].Count; x++)
                {
                    if (x > 0)
                        ok &= grid[y][x] < grid[y][x - 1];
                    if (x < grid[y].Count - 1)
                        ok &= grid[y][x] < grid[y][x + 1];
                    if (y > 0)
                        ok &= grid[y][x] < grid[y - 1][x];
                    if (y < grid.Count - 1)
                        ok &= grid[y][x] < grid[y + 1][x];

                    if (ok)
                        yield return new Point(x, y);
                    ok = true;
                }
            }
        }
        private static HashSet<Point> GetHighs(Point source, List<List<int>> grid, ref HashSet<Point> highs, Point cameFrom = null)
        {
            if (grid[source.Y][source.X] == 8)
                return highs;

            // Gauche
            if (source.X > 0 && grid[source.Y][source.X - 1] < 9)
            {
                var point = new Point(source.X - 1, source.Y);
                if (!point.Equals(cameFrom) && !highs.Contains(point))
                {
                    highs.Add(point);
                    highs = GetHighs(point, grid, ref highs, source);
                }
            }

            // Droite
            if (source.X < grid[source.Y].Count - 1 && grid[source.Y][source.X + 1] < 9)
            {
                var point = new Point(source.X + 1, source.Y);
                if (!point.Equals(cameFrom) && !highs.Contains(point))
                {
                    highs.Add(point);
                    highs = GetHighs(point, grid, ref highs, source);
                }
            }

            // Haut
            if (source.Y > 0 && grid[source.Y - 1][source.X] < 9)
            {
                var point = new Point(source.X, source.Y - 1);
                if (!point.Equals(cameFrom) && !highs.Contains(point))
                {
                    highs.Add(point);
                    highs = GetHighs(point, grid, ref highs, source);
                }
            }

            // Bas
            if (source.Y < grid.Count - 1 && grid[source.Y + 1][source.X] < 9)
            {
                var point = new Point(source.X, source.Y + 1);
                if (!point.Equals(cameFrom) && !highs.Contains(point))
                {
                    highs.Add(point);
                    highs = GetHighs(point, grid, ref highs, source);
                }
            }

            return highs;
        }

        private class Point : IEquatable<Point>
        {
            public int X { get; }
            public int Y { get; }

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                    return false;

                Point other = obj as Point;
                return other != null && other.X == X && other.Y == Y;
            }

            public override int GetHashCode() => HashCode.Combine(X, Y);

            public bool Equals(Point other)
            {
                if (other == null) return false;
                return X == other.X && Y == other.Y;
            }
        }
    }
}
