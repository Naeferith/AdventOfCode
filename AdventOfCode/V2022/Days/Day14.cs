using AdventOfCode.Core.AoC;
using AdventOfCode.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.V2022.Days
{
    internal class Day14 : IDay
    {
        private List<List<Point>> _rockPaths;
        private HashSet<Point> _solidPoints;
        public string PuzzleName => "Regolith Reservoir";

        public string Solution1(string[] lines)
        {
            return Solution(lines, false);
        }

        public string Solution2(string[] lines)
        {
            return Solution(lines, true);
        }

        public string Solution(string[] lines, bool hasGround)
        {
            Initialize(lines);
            Materialize();

            var sandSource = new Point(500, 0);
            var curSand = sandSource;

            var (minX, maxX) = _rockPaths.SelectMany(p => p.Select(p => p.X)).Bounds();
            var maxY = _rockPaths.SelectMany(p => p.Select(p => p.Y)).Max() + 2;

            int? ground = hasGround ? maxY : null;

            var nbRocks = _solidPoints.Count;
            var oldSandCount = _solidPoints.Count;

            do
            {
                do
                {
                    var fall = FallDirection(curSand, ground);

                    if (!fall.HasValue)
                    {
                        _solidPoints.Add(curSand);
                        curSand = sandSource;
                        break;
                    }
                    else
                    {
                        curSand = fall.Value;
                    }
                } while ((hasGround || curSand.X.IsBetweenInclusive(minX, maxX)) && curSand.Y.IsBetweenInclusive(0, maxY));

                oldSandCount++;
            } while (_solidPoints.Count == oldSandCount);

            return (_solidPoints.Count - nbRocks).ToString();
        }

        private void Initialize(string[] lines)
        {
            _rockPaths = new();

            foreach (var line in lines)
            {
                var coords = line
                    .Split(" -> ")
                    .Select(t => t
                        .Split(',')
                        .Select(int.Parse))
                    .SelectMany(i => i)
                    .ToArray();

                var work = new List<Point>();
                for (int i = 0; i < coords.Length; i += 2)
                {
                    work.Add(new Point(coords[i], coords[i + 1]));
                }
                _rockPaths.Add(work);
            }
        }

        private void Materialize()
        {
            _solidPoints = new();

            foreach (var path in _rockPaths)
            {
                for (int i = 1; i <= path.Count - 1; i++)
                {
                    int min, max;

                    // Horizontal
                    if (path[i - 1].Y - path[i].Y == 0)
                    {
                        min = Math.Min(path[i - 1].X, path[i].X);
                        max = Math.Max(path[i - 1].X, path[i].X);

                        for (int x = min; x <= max; x++)
                            _solidPoints.Add(new Point(x, path[i].Y));
                    }
                    else
                    {
                        min = Math.Min(path[i - 1].Y, path[i].Y);
                        max = Math.Max(path[i - 1].Y, path[i].Y);

                        for (int y = min; y <= max; y++)
                            _solidPoints.Add(new Point(path[i].X, y));
                    }
                }
            }
        }

        private Point? FallDirection(Point sand, int? ground)
        {
            var under = sand + Point.UnitY;

            if (under.Y >= ground)
                return null;

            if (!_solidPoints.Contains(under))
                return under;

            var left = under - Point.UnitX;
            if (!_solidPoints.Contains(left))
                return left;

            var right = under + Point.UnitX;
            if (!_solidPoints.Contains(right))
                return right;

            return null;
        }
    }
}
