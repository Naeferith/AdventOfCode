﻿using AdventOfCode.Core.Components;
using System.Text.RegularExpressions;

namespace AdventOfCode.V2021.Days
{
    internal class Day13 : IDay
    {
        public int DayNumber => 13;

        public string PuzzleName => "Transparent Origami";

        public string Solution1(string[] lines)
        {
            var dots = lines.Where(l => Regex.IsMatch(l, @"^\d")).Select(l =>
            {
                var a = l.Split(',');
                return (int.Parse(a[0]), int.Parse(a[1]));
            });

            var folds = lines.Where(l => l.StartsWith("fold")).Select(l =>
            {
                var a = l[11..].Split('=');
                return (a[0], int.Parse(a[1]));
            });

            var width = dots.Select(d => d.Item1).Max();
            var height = dots.Select(d => d.Item2).Max();

            var grid = new List<List<bool>>();

            for (int i = 0; i <= width; i++)
                grid.Add(Enumerable.Repeat(false, height + 1).ToList());

            foreach (var dot in dots)
                grid[dot.Item1][dot.Item2] = true;


            var fold = folds.First();

            switch (fold.Item1)
            {
                case "y":
                    {
                        var newSize = height / 2;

                        for (int y = 0; y < newSize; y++)
                        {
                            for (int x = 0; x < grid.Count; x++)
                                grid[x][y] |= grid[x][height - y];
                        }

                        foreach (var col in grid)
                            col.RemoveRange(newSize, col.Count - newSize);
                    }
                    break;
                case "x":
                    {
                        var newSize = width / 2;

                        for (int x = 0; x < newSize; x++)
                        {
                            for (int y = 0; y < grid[y].Count; y++)
                                grid[x][y] |= grid[width - x][y];
                        }
                        grid.RemoveRange(newSize, grid.Count - newSize);
                    }
                    break;
            }

            var sum = 0;
            for (int y = 0; y < grid[0].Count; y++)
            {
                for (int x = 0; x < grid.Count; x++)
                    sum += grid[x][y] ? 1 : 0;
            }

            return sum.ToString();
        }

        public string Solution2(string[] lines)
        {
            var dots = lines.Where(l => Regex.IsMatch(l, @"^\d")).Select(l =>
            {
                var a = l.Split(',');
                return (int.Parse(a[0]), int.Parse(a[1]));
            });
            var folds = lines.Where(l => l.StartsWith("fold")).Select(l =>
            {
                var a = l[11..].Split('=');
                return (a[0], int.Parse(a[1]));
            });
            var width = dots.Select(d => d.Item1).Max();
            var height = dots.Select(d => d.Item2).Max();

            var grid = new List<List<bool>>();

            for (int i = 0; i <= width; i++)
                grid.Add(Enumerable.Repeat(false, height + 1).ToList());

            foreach (var dot in dots)
                grid[dot.Item1][dot.Item2] = true;

            // Fold
            foreach (var fold in folds)
            {
                switch (fold.Item1)
                {
                    case "y":
                        {
                            var newSize = height / 2;

                            for (int y = 0; y < newSize; y++)
                            {
                                for (int x = 0; x < grid.Count; x++)
                                    grid[x][y] |= grid[x][height - y];
                            }

                            foreach (var col in grid)
                                col.RemoveRange(newSize, col.Count - newSize);
                            height = newSize - 1;
                        }
                        break;
                    case "x":
                        {
                            var newSize = width / 2;

                            for (int x = 0; x < newSize; x++)
                            {
                                for (int y = 0; y < grid[y].Count; y++)
                                    grid[x][y] |= grid[width - x][y];
                            }
                            grid.RemoveRange(newSize, grid.Count - newSize);
                            width = newSize - 1;
                        }
                        break;
                }
            }

            for (int y = 0; y < grid[0].Count; y++)
            {
                for (int x = 0; x < grid.Count; x++)
                    Console.Write(grid[x][y] ? "#" : " ");
                Console.WriteLine();
            }

            // See console for result
            return string.Empty;
        }
    }
}
