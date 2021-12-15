using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.V2021
{
    internal class Day11 : IDay
    {
        public string PuzzleName => "Dumbo Octopus";

        public string Solution1(string[] lines)
        {
            const int STEPS = 100;
            var input = lines.Select(l => l.Select(c => c - '0'));
            var width = input.First().Count();
            var height = input.Count();
            var grid = new int[width, height];
            var flashers = new List<(int, int)>();
            var sum = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                    grid[x, y] = input.ElementAt(y).ElementAt(x);
            }

            for (int i = 0; i < STEPS; i++)
            {
                flashers.Clear();

                // Increase + Flash
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        grid[x, y]++;
                        if (grid[x, y] > 9)
                        {
                            flashers.Add((x, y));
                            grid[x, y] = 0;
                        }
                    }
                }

                // Spread
                var spreaded = 0;
                foreach (var flash in flashers)
                    spreaded += Spread(grid, flash.Item1, flash.Item2);

                // Debug
                if (false)
                {
                    Console.WriteLine();
                    Console.WriteLine($"---------- Step {i + 1} ---------");
                    Console.WriteLine();

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            Console.ForegroundColor = grid[x, y] == 0 ? ConsoleColor.Red : ConsoleColor.White;
                            Console.Write(grid[x, y]);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                }

                sum += flashers.Count + spreaded;
            }

            return sum.ToString();
        }

        public string Solution2(string[] lines)
        {
            var input = lines.Select(l => l.Select(c => c - '0'));
            var width = input.First().Count();
            var height = input.Count();
            var grid = new int[width, height];
            var flashers = new List<(int, int)>();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                    grid[x, y] = input.ElementAt(y).ElementAt(x);
            }

            var i = 0;
            var spreaded = 0;
            do
            {
                i++;
                spreaded = 0;
                flashers.Clear();

                // Increase + Flash
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        grid[x, y]++;
                        if (grid[x, y] > 9)
                        {
                            flashers.Add((x, y));
                            grid[x, y] = 0;
                        }
                    }
                }

                // Spread
                foreach (var flash in flashers)
                    spreaded += Spread(grid, flash.Item1, flash.Item2);

                // Debug
                if (false)
                {
                    Console.WriteLine();
                    Console.WriteLine($"---------- Step {i} ---------");
                    Console.WriteLine();

                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            Console.ForegroundColor = grid[x, y] == 0 ? ConsoleColor.Red : ConsoleColor.White;
                            Console.Write(grid[x, y]);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                }
            } while (flashers.Count + spreaded != width * height);

            return i.ToString();
        }

        private static int TryIncrease(int[,] grid, int x, int y)
        {
            if (grid[x, y] > 0)
                grid[x, y]++;
            if (grid[x, y] > 9)
            {
                grid[x, y] = 0;
                return 1 + Spread(grid, x, y);
            }
            return 0;
        }

        private static int Spread(int[,] grid, int x, int y)
        {
            var sum = 0;
            if (x > 0)
            {
                sum += TryIncrease(grid, x - 1, y);
                if (y > 0)
                    sum += TryIncrease(grid, x - 1, y - 1);
                if (y < grid.GetLength(1) - 1)
                    sum += TryIncrease(grid, x - 1, y + 1);
            }
            if (x < grid.GetLength(0) - 1)
            {
                sum += TryIncrease(grid, x + 1, y);
                if (y > 0)
                    sum += TryIncrease(grid, x + 1, y - 1);
                if (y < grid.GetLength(1) - 1)
                    sum += TryIncrease(grid, x + 1, y + 1);
            }
            if (y > 0)
                sum += TryIncrease(grid, x, y - 1);
            if (y < grid.GetLength(1) - 1)
                sum += TryIncrease(grid, x, y + 1);

            return sum;
        }
    }
}
