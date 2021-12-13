using System;
using System.Collections.Generic;
#if DEBUG
using System.Drawing;
#endif
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Business
{
    internal sealed partial class AoC21
    {
        private const string INPUT_PATH = @"./2021/Input/";

        #region Solutions
        private static class Day1
        {
            public static string Test1()
            {
                var depths = File.ReadLines($"{INPUT_PATH}day1.txt").Select(t => int.Parse(t));

                var previous = depths.First();
                var count = 0;

                foreach (var depth in depths)
                {
                    count += (depth > previous) ? 1 : 0;
                    previous = depth;
                }

                return count.ToString();
            }
            public static string Test2()
            {
                const int slideSize = 3;

                var depths = File.ReadLines($"{INPUT_PATH}day1.txt").Select(t => int.Parse(t));

                var previous = depths.Take(slideSize).Sum();
                var count = 0;

                for (int i = 1; i < depths.Count() - (slideSize - 1); i++)
                {
                    var val = depths.Skip(i).Take(slideSize).Sum();

                    count += (val > previous) ? 1 : 0;
                    previous = val;
                }

                return count.ToString();
            }
        }
        private static class Day2
        {
            public static string Test1()
            {
                var movements = File.ReadLines($"{INPUT_PATH}day2.txt").Select(o =>
                {
                    var args = o.Split(' ');
                    return (args[0], int.Parse(args[1]));
                });

                int hPos = 0, dPos = 0;

                foreach (var movement in movements)
                {
                    switch (movement.Item1)
                    {
                        case "forward":
                            hPos += movement.Item2;
                            break;
                        case "up":
                            dPos -= movement.Item2;
                            break;
                        case "down":
                            dPos += movement.Item2;
                            break;
                        default:
                            break;
                    }
                }

                return (dPos * hPos).ToString();
            }
            public static string Test2()
            {
                var movements = File.ReadLines($"{INPUT_PATH}day2.txt").Select(o =>
                {
                    var args = o.Split(' ');
                    return (args[0], int.Parse(args[1]));
                });

                int hPos = 0, dPos = 0, aim = 0;

                foreach (var movement in movements)
                {
                    switch (movement.Item1)
                    {
                        case "forward":
                            hPos += movement.Item2;
                            dPos += aim * movement.Item2;
                            break;
                        case "up":
                            aim -= movement.Item2;
                            break;
                        case "down":
                            aim += movement.Item2;
                            break;
                        default:
                            break;
                    }
                }

                return (dPos * hPos).ToString();
            }
        }
        private static class Day3
        {
            public static string Test1()
            {
                const int binarySize = 12;
                var binaries = File.ReadLines($"{INPUT_PATH}day3.txt");
                var count = binaries.Count();
                var gammaRate = new int[binarySize];

                for (int i = 0; i < binarySize; i++)
                {
                    gammaRate[i] = (int)Math.Round(binaries.Select(b => int.Parse(b[i].ToString())).Sum() / (double)count);
                }

                var gammaB = gammaRate.Select(i => i == 1);
                var epsilonB = gammaB.Select(b => !b);

                var gammaStr = string.Join(string.Empty, gammaRate);
                var epsilonStr = string.Join(string.Empty, epsilonB.Select(b => b ? "1" : "0"));

                return (Convert.ToInt32(gammaStr, 2) * Convert.ToInt32(epsilonStr, 2)).ToString();
            }
            public static string Test2()
            {
                const int binarySize = 12;

                var binaries = File.ReadLines($"{INPUT_PATH}day3.txt");
                var count = binaries.Count();
                var o2Enum = binaries.ToList();
                var co2Enum = binaries.ToList();

                for (int i = 0; i < binarySize; i++)
                {
                    if (o2Enum.Count > 1)
                    {
                        var v = (int)Math.Round(o2Enum.Select(b => int.Parse(b[i].ToString())).Sum() / (double)o2Enum.Count, MidpointRounding.AwayFromZero);
                        o2Enum = o2Enum.Where(b => b[i].ToString().Equals(v.ToString())).ToList();
                    }

                    if (co2Enum.Count > 1)
                    {
                        var v = (int)Math.Round(co2Enum.Select(b => int.Parse(b[i].ToString())).Sum() / (double)co2Enum.Count, MidpointRounding.AwayFromZero);
                        co2Enum = co2Enum.Where(b => !b[i].ToString().Equals(v.ToString())).ToList();
                    }

                    if (o2Enum.Count == 1 && co2Enum.Count == 1)
                        break;
                }

                return (Convert.ToInt32(o2Enum.First(), 2) * Convert.ToInt32(co2Enum.First(), 2)).ToString();
            }
        }
        private static class Day4
        {
            public static string Test1()
            {
                var input = File.ReadLines($"{INPUT_PATH}day4.txt");
                var pulls = input.First().Split(',').Select(p => int.Parse(p));

                List<(int, bool)[,]> boards = new();
                boards.Add(new (int, bool)[5, 5]);
                var nLine = 0;

                foreach (var line in input.Where(l => !string.IsNullOrEmpty(l)).Skip(1))
                {
                    if (nLine == 5)
                    {
                        boards.Add(new (int, bool)[5, 5]);
                        nLine = 0;
                    }

                    foreach (var num in Regex.Split(line.Trim(), " +").Select((x, i) => new { Index = i, Value = int.Parse(x) }))
                    {
                        boards.Last()[nLine, num.Index] = (num.Value, false);
                    }
                    nLine++;
                }

                foreach (var pull in pulls)
                {
                    foreach (var board in boards)
                    {
                        var flag = false;
                        for (int y = 0; !flag && y < board.GetLength(0); y++)
                        {
                            for (int x = 0; x < board.GetLength(1); x++)
                            {
                                if (board[x, y].Item1 == pull)
                                {
                                    board[x, y].Item2 = true;

                                    if (Enumerable
                                        .Range(0, board.GetLength(0))
                                        .Select(k => board[k, y].Item2)
                                        .Aggregate((a, b) => a && b)
                                        ||
                                        Enumerable
                                        .Range(0, board.GetLength(1))
                                        .Select(k => board[x, k].Item2)
                                        .Aggregate((a, b) => a && b))
                                    {
                                        return Result(board, pull).ToString();
                                    }

                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                return string.Empty;
            }
            public static string Test2()
            {
                var input = File.ReadLines($"{INPUT_PATH}day4.txt");
                var pulls = input.First().Split(',').Select(p => int.Parse(p));

                List<(int, bool)[,]> boards = new();
                boards.Add(new (int, bool)[5, 5]);
                var nLine = 0;

                foreach (var line in input.Where(l => !string.IsNullOrEmpty(l)).Skip(1))
                {
                    if (nLine == 5)
                    {
                        boards.Add(new (int, bool)[5, 5]);
                        nLine = 0;
                    }

                    foreach (var num in Regex.Split(line.Trim(), " +").Select((x, i) => new { Index = i, Value = int.Parse(x) }))
                    {
                        boards.Last()[nLine, num.Index] = (num.Value, false);
                    }
                    nLine++;
                }

                foreach (var pull in pulls)
                {
                    for (int i = boards.Count - 1; i > -1; i--)
                    {
                        var flag = false;
                        for (int y = 0; !flag && y < boards[i].GetLength(0); y++)
                        {
                            for (int x = 0; x < boards[i].GetLength(1); x++)
                            {
                                if (boards[i][x, y].Item1 == pull)
                                {
                                    boards[i][x, y].Item2 = true;

                                    if (Enumerable
                                        .Range(0, boards[i].GetLength(0))
                                        .Select(k => boards[i][k, y].Item2)
                                        .Aggregate((a, b) => a && b)
                                        ||
                                        Enumerable
                                        .Range(0, boards[i].GetLength(1))
                                        .Select(k => boards[i][x, k].Item2)
                                        .Aggregate((a, b) => a && b))
                                    {
                                        if (boards.Count == 1)
                                        {
                                            return Result(boards[i], pull).ToString();
                                        }
                                        else
                                        {
                                            boards.RemoveAt(i);
                                        }
                                    }

                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                return string.Empty;
            }
            private static int Result((int, bool)[,] board, int pull)
            {
                var sum = 0;
                for (int y = 0; y < board.GetLength(0); y++)
                {
                    for (int x = 0; x < board.GetLength(1); x++)
                    {
                        sum += board[x, y].Item2 ? 0 : board[x, y].Item1;
                    }
                }
                return sum * pull;
            }
        }
        private static class Day5
        {
            public static string Test1()
            {
                var lines = File.ReadLines($"{INPUT_PATH}day5.txt").Select(l =>
                {
                    var p = l.Split(" -> ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    return (Point.Parse(p[0]), Point.Parse(p[1]));
                });

                int[,] grid = new int[1000, 1000];

                foreach (var line in lines)
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
                        {
                            count++;
                        }
                    }
                }

                return count.ToString();
            }
            public static string Test2()
            {
                var lines = File.ReadLines($"{INPUT_PATH}day5.txt").Select(l =>
                {
                    var p = l.Split(" -> ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    return (Point.Parse(p[0]), Point.Parse(p[1]));
                });

                int[,] grid = new int[1000, 1000];

                foreach (var line in lines)
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
                        {
                            count++;
                        }
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
        private static class Day6
        {
            public static string Test1()
            {
                const short REPRODUCTION_DAYS = 7;
                const short MATURITY_DAYS = 2;
                const short SIMULATION_DAYS = 80;

                var fishes = File.ReadLines($"{INPUT_PATH}day6.txt").Select(t => t.Split(',').Select(n => short.Parse(n)).ToList()).First();

                for (int i = 0; i < SIMULATION_DAYS; i++)
                {
                    var newFishes = new List<short>();

                    for (int j = 0; j < fishes.Count; j++)
                    {
                        if (fishes[j] == 0)
                        {
                            fishes[j] = REPRODUCTION_DAYS;
                            newFishes.Add(REPRODUCTION_DAYS + MATURITY_DAYS - 1);
                        }
                        fishes[j]--;
                    }
                    fishes.AddRange(newFishes);
                }

                return fishes.Count.ToString();
            }
            public static string Test2()
            {
                const int REPRODUCTION_DAYS = 7;
                const int MATURITY_DAYS = 2;
                const int SIMULATION_DAYS = 256;

                var input = File.ReadLines($"{INPUT_PATH}day6.txt").Select(t =>
                t
                .Split(',')
                .Select(n => int.Parse(n))
                .GroupBy(n => n)
                .Select(g => (g.Key, (ulong)g.Count()))).First();

                var fishes = new SortedDictionary<int, ulong>();

                for (int i = 0; i < MATURITY_DAYS + REPRODUCTION_DAYS; i++)
                {
                    fishes[i] = 0;
                }

                foreach (var tuple in input)
                {
                    fishes[tuple.Key] = tuple.Item2;
                }

                for (int i = 0; i < SIMULATION_DAYS; i++)
                {
                    var babyFishes = 0UL;
                    for (int j = 0; j < fishes.Count; j++)
                    {
                        if (j == 0)
                        {
                            babyFishes = fishes[j];
                            continue;
                        }
                        fishes[j - 1] = fishes[j];
                        if (j == fishes.Count - 1)
                            fishes[j] = 0;
                    }
                    fishes[REPRODUCTION_DAYS - 1] += babyFishes;
                    fishes[MATURITY_DAYS + REPRODUCTION_DAYS - 1] += babyFishes;
                }

                return fishes.Values.Aggregate((s, v) => s + v).ToString();
            }
        }
        private static class Day7
        {
            public static string Test1()
            {
                var crabs = File.ReadLines($"{INPUT_PATH}day7.txt").Select(t => t.Split(',').Select(n => int.Parse(n))).First();
                var max = crabs.Max();

                var dict = new Dictionary<int, int>();

                for (int i = 0; i <= max; i++)
                {
                    var sum = 0;
                    foreach (var crab in crabs)
                    {
                        sum += Math.Abs(crab - i);
                    }
                    dict[i] = sum;
                }

                return dict.Values.Min().ToString();
            }
            public static string Test2()
            {
                var crabs = File.ReadLines($"{INPUT_PATH}day7.txt").Select(t => t.Split(',').Select(n => int.Parse(n))).First();
                var max = crabs.Max();

                var dict = new Dictionary<int, int>();

                for (int i = 0; i <= max; i++)
                {
                    var sum = 0;
                    foreach (var crab in crabs)
                    {
                        sum += CrabFuel(Math.Abs(crab - i));
                    }
                    dict[i] = sum;
                }

                return dict.Values.Min().ToString();
            }
            private static int CrabFuel(int delta)
                => delta == 0 ? 0 : delta + CrabFuel(--delta);
        }
        private static class Day8
        {
            public static string Test1()
            {
                var segments = File.ReadLines($"{INPUT_PATH}day8.txt").Select(t =>
                {
                    return t.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries)[1].Split(' ')
                    .Where(s => s.Length == 2 || s.Length == 3 || s.Length == 4 || s.Length == 7)
                    .Count();
                }
                );

                return segments.Sum().ToString();
            }
            public static string Test2()
            {
                var lines = File.ReadLines($"{INPUT_PATH}day8.txt");

                var sum = 0;
                foreach (var line in lines)
                {
                    var dec = new Decoder();
                    var args = line.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries);
                    dec.Analyse(args[0].Split(' '));
                    sum += dec.Decode(args[1].Split(' '));
                }


                return sum.ToString();
            }

            private class Decoder
            {
                private readonly Dictionary<int, string> Numbers;

                public Decoder()
                {
                    Numbers = new Dictionary<int, string>(10)
                    {
                        [8] = "abcdefg"
                    };
                }

                public void Analyse(string[] input)
                {
                    var ez = input.Where(s => s.Length == 2 || s.Length == 3 || s.Length == 4);
                    var hard = input.Where(s => s.Length == 5 || s.Length == 6);

                    foreach (var str in ez)
                    {
                        switch (str.Length)
                        {
                            case 2:
                                Numbers[1] = string.Concat(str.OrderBy(c => c));
                                break;
                            case 3:
                                Numbers[7] = string.Concat(str.OrderBy(c => c));
                                break;
                            case 4:
                                Numbers[4] = string.Concat(str.OrderBy(c => c));
                                break;
                            default:
                                break;
                        }
                    }

                    foreach (var str in hard.OrderBy(h => h.Length))
                    {
                        switch (str.Length)
                        {
                            case 5:
                                {
                                    var strOne = Numbers[1];
                                    var strFour = Numbers[4];

                                    if (strOne.Intersect(str).Count() == strOne.Length)
                                        Numbers[3] = string.Concat(str.OrderBy(c => c));
                                    else if (strFour.Intersect(str).Count() == 3)
                                        Numbers[5] = string.Concat(str.OrderBy(c => c));
                                    else
                                        Numbers[2] = string.Concat(str.OrderBy(c => c));
                                }
                                break;
                            case 6:
                                {
                                    var strFour = Numbers[4];
                                    var strSeven = Numbers[7];

                                    if (str.Intersect(strSeven).Count() != strSeven.Length)
                                        Numbers[6] = string.Concat(str.OrderBy(c => c));
                                    else if (str.Intersect(strFour).Count() == strFour.Length)
                                        Numbers[9] = string.Concat(str.OrderBy(c => c));
                                    else
                                        Numbers[0] = string.Concat(str.OrderBy(c => c));
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                public int Decode(string[] output)
                {
                    var outstr = string.Empty;
                    foreach (var str in output)
                    {
                        outstr += Numbers.First(e => e.Value.Equals(string.Concat(str.OrderBy(c => c)))).Key;
                    }
                    return int.Parse(outstr);
                }
            }
        }
        private static class Day9
        {
            public static string Test1()
            {
                var lines = File.ReadLines($"{INPUT_PATH}day9.txt").Select(l => l.Select(c => c - '0'));
                var grid = new List<List<int>>();
                var points = new List<int>();

                foreach (var row in lines)
                    grid.Add(new List<int>(row));

                foreach (var low in GetLows(grid))
                    points.Add(grid[low.Y][low.X]);

                return points.Sum(i => i + 1).ToString();
            }

            public static string Test2(bool debug = false)
            {
                var lines = File.ReadLines($"{INPUT_PATH}day9.txt").Select(l => l.Select(c => c - '0'));
                var grid = new List<List<int>>();
                var bazins = new List<HashSet<Point>>();

                foreach (var row in lines)
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
                if (debug)
                {
                    using var bmp = new Bitmap(lines.First().Count(), lines.Count());
                    using var gfx = Graphics.FromImage(bmp);
                    gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                    gfx.Clear(Color.Black);

                    int x = 0, y = 0;

                    foreach (var line in lines)
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
                    {
                        return false;
                    }

                    Point other = obj as Point;
                    return other != null && other.X == X && other.Y == Y;
                }

                public override int GetHashCode()
                {
                    return HashCode.Combine(X, Y);
                }

                public bool Equals(Point other)
                {
                    if (other == null) return false;
                    return X == other.X && Y == other.Y;
                }
            }
        }
        private static class Day10
        {
            private static readonly Dictionary<char, char> _tokens = new()
            {
                ['('] = ')',
                ['{'] = '}',
                ['['] = ']',
                ['<'] = '>'
            };

            public static string Test1()
            {
                var lines = File.ReadLines($"{INPUT_PATH}day10.txt");

                var sum = 0;
                foreach (var line in lines)
                {
                    if (IsCorrupted(line, out var c))
                    {
                        switch (c)
                        {
                            case ')':
                                sum += 3;
                                break;
                            case ']':
                                sum += 57;
                                break;
                            case '}':
                                sum += 1197;
                                break;
                            case '>':
                                sum += 25137;
                                break;
                            default:
                                break;
                        }
                    }
                }

                return sum.ToString();
            }
            public static string Test2()
            {
                var lines = File.ReadLines($"{INPUT_PATH}day10.txt");

                var scores = new SortedSet<ulong>();
                foreach (var line in lines.Where(l => !IsCorrupted(l, out var _)))
                {
                    if (IsIncomplete(line, out var s))
                    {
                        var score = 0UL;
                        do
                        {
                            switch (s.Pop())
                            {
                                case '(':
                                    score = score * 5 + 1;
                                    break;
                                case '[':
                                    score = score * 5 + 2;
                                    break;
                                case '{':
                                    score = score * 5 + 3;
                                    break;
                                case '<':
                                    score = score * 5 + 4;
                                    break;
                                default:
                                    break;
                            }
                        } while (s.Any());
                        scores.Add(score);
                    }
                }

                return scores.ElementAt(scores.Count / 2).ToString();
            }

            private static bool IsCorrupted(string line, out char corrupted)
            {
                var stack = new Stack<char>();

                foreach (var c in line)
                {
                    if (_tokens.ContainsKey(c))
                        stack.Push(c);
                    else if (_tokens.First(x => x.Value == c).Key == stack.Peek())
                        stack.Pop();
                    else
                    {
                        corrupted = c;
                        return true;
                    }
                }
                corrupted = default;
                return false;
            }
            private static bool IsIncomplete(string line, out Stack<char> remainings)
            {
                remainings = new Stack<char>();

                foreach (var c in line)
                {
                    if (_tokens.ContainsKey(c))
                        remainings.Push(c);
                    else if (_tokens.First(x => x.Value == c).Key == remainings.Peek())
                        remainings.Pop();
                }

                return remainings.Any();
            }
        }
        private static class Day11
        {
            public static string Test1(bool debug = false)
            {
                const int STEPS = 100;
                var input = File.ReadLines($"{INPUT_PATH}day11.txt").Select(l => l.Select(c => c - '0'));
                var width = input.First().Count();
                var height = input.Count();
                var grid = new int[width, height];
                var flashers = new List<(int, int)>();
                var sum = 0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        grid[x, y] = input.ElementAt(y).ElementAt(x);
                    }
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
                    if (debug)
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
            public static string Test2(bool debug = false)
            {
                var input = File.ReadLines($"{INPUT_PATH}day11.txt").Select(l => l.Select(c => c - '0'));
                var width = input.First().Count();
                var height = input.Count();
                var grid = new int[width, height];
                var flashers = new List<(int, int)>();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        grid[x, y] = input.ElementAt(y).ElementAt(x);
                    }
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
                    if (debug)
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
                    {
                        sum += TryIncrease(grid, x - 1, y - 1);
                    }
                    if (y < grid.GetLength(1) - 1)
                    {
                        sum += TryIncrease(grid, x - 1, y + 1);
                    }
                }
                if (x < grid.GetLength(0) - 1)
                {
                    sum += TryIncrease(grid, x + 1, y);
                    if (y > 0)
                    {
                        sum += TryIncrease(grid, x + 1, y - 1);
                    }
                    if (y < grid.GetLength(1) - 1)
                    {
                        sum += TryIncrease(grid, x + 1, y + 1);
                    }
                }
                if (y > 0)
                {
                    sum += TryIncrease(grid, x, y - 1);
                }
                if (y < grid.GetLength(1) - 1)
                {
                    sum += TryIncrease(grid, x, y + 1);
                }

                return sum;
            }
        }
        private static class Day12
        {
            public static string Test1()
            {
                var input = File.ReadLines($"{INPUT_PATH}day12.txt").Select(l =>
                {
                    var s = l.Split('-');
                    return (s[0], s[1]);
                });

                var nodes = new HashSet<Cave>();
                foreach (var tuple in input)
                {
                    Cave
                        nodeIn = new(tuple.Item1),
                        nodeOut = new(tuple.Item2);

                    nodeIn = nodes.TryGetValue(nodeIn, out var _in) ? _in : nodeIn;
                    nodeOut = nodes.TryGetValue(nodeOut, out var _out) ? _out : nodeOut;

                    nodeIn.Add(nodeOut);
                    nodeOut.Add(nodeIn);

                    nodes.Add(nodeIn);
                    nodes.Add(nodeOut);
                }

                var paths = new SortedSet<string>();
                var start = nodes.First(n => n.Id.Equals("start"));
                GetPath(start, string.Empty, paths);

                return paths.Count.ToString();
            }
            public static string Test2()
            {
                var input = File.ReadLines($"{INPUT_PATH}day12.txt").Select(l =>
                {
                    var s = l.Split('-');
                    return (s[0], s[1]);
                });

                var nodes = new HashSet<Cave>();
                foreach (var tuple in input)
                {
                    Cave
                        nodeIn = new(tuple.Item1),
                        nodeOut = new(tuple.Item2);

                    nodeIn = nodes.TryGetValue(nodeIn, out var _in) ? _in : nodeIn;
                    nodeOut = nodes.TryGetValue(nodeOut, out var _out) ? _out : nodeOut;

                    nodeIn.Add(nodeOut);
                    nodeOut.Add(nodeIn);

                    nodes.Add(nodeIn);
                    nodes.Add(nodeOut);
                }

                var paths = new SortedSet<string>();
                var start = nodes.First(n => n.Id.Equals("start"));
                GetPath2(start, string.Empty, paths);

                return paths.Count.ToString();
            }

            private static void GetPath(Cave source, string path, ICollection<string> paths)
            {
                if (source.Id.Equals("end"))
                {
                    paths.Add(path + source.Id);
                    return;
                }

                path += $"{source.Id},";
                foreach (var next in source.Neighbours)
                {
                    if (next.IsBigCave || !path.Split(',').Contains(next.Id))
                    {
                        GetPath(next, path, paths);
                    }
                }
            }
            private static void GetPath2(Cave source, string path, ICollection<string> paths, bool twice = false)
            {
                if (source.Id.Equals("end"))
                {
                    paths.Add(path + source.Id);
                    return;
                }

                path += $"{source.Id},";
                foreach (var next in source.Neighbours)
                {
                    if (next.IsBigCave || !twice || !path.Split(',').Contains(next.Id))
                    {
                        if (next.Id.Equals("start"))
                            continue;

                        if (!next.IsBigCave && path.Split(',').Contains(next.Id))
                        {
                            GetPath2(next, path, paths, true);
                        }
                        else
                            GetPath2(next, path, paths, twice);
                    }
                }
            }
            private class Cave : IEquatable<Cave>
            {
                public string Id { get; }
                public HashSet<Cave> Neighbours { get; } = new();

                public Cave(string id)
                {
                    Id = id;
                }

                public void Add(Cave node)
                {
                    if (!Equals(node))
                        Neighbours.Add(node);
                }

                public bool IsBigCave => Id.ToUpper().Equals(Id);

                public override bool Equals(object obj)
                {

                    var other = obj as Cave;
                    return other?.Id.Equals(Id) ?? false;
                }

                // override object.GetHashCode
                public override int GetHashCode()
                {
                    return Id.GetHashCode();
                }

                public bool Equals(Cave other)
                {
                    return other?.Id.Equals(Id) ?? false;
                }

                public override string ToString()
                {
                    return Id;
                }
            }
        }
        private static class Day13
        {
            public static string Test1()
            {
                var input = File.ReadLines($"{INPUT_PATH}day13.txt");
                var dots = input.Where(l => Regex.IsMatch(l, @"^\d")).Select(l => 
                {
                    var a = l.Split(',');
                    return (int.Parse(a[0]), int.Parse(a[1]));
                });
                var folds = input.Where(l => l.StartsWith("fold")).Select(l => 
                {
                    var a = l[11..].Split('=');
                    return (a[0], int.Parse(a[1]));
                });
                var width = dots.Select(d => d.Item1).Max();
                var height = dots.Select(d => d.Item2).Max();

                var grid = new List<List<bool>>();

                for (int i = 0; i <= width; i++)
                {
                    grid.Add(Enumerable.Repeat(false, height + 1).ToList());
                }

                foreach (var dot in dots)
                {
                    grid[dot.Item1][dot.Item2] = true;
                }

                // Fold
                var fold = folds.First();
                 
                switch (fold.Item1)
                {
                    case "y":
                        {
                            var newSize = height / 2;

                            for (int y = 0; y < newSize; y++)
                            {
                                for (int x = 0; x < grid.Count; x++)
                                {
                                    grid[x][y] |= grid[x][height - y];
                                }
                            }

                            foreach (var col in grid)
                            {
                                col.RemoveRange(newSize, col.Count - newSize);
                            }
                        }
                        break;
                    case "x":
                        {
                            var newSize = width / 2;

                            for (int x = 0; x < newSize; x++)
                            {
                                for (int y = 0; y < grid[y].Count; y++)
                                {
                                    grid[x][y] |= grid[width - x][y];
                                }
                            }
                            grid.RemoveRange(newSize, grid.Count - newSize);
                        }
                        break;
                }

                var sum = 0;
                for (int y = 0; y < grid[0].Count; y++)
                {
                    for (int x = 0; x < grid.Count; x++)
                    {
                        sum += grid[x][y] ? 1 : 0;
                    }
                }

                return sum.ToString();
            }
            public static string Test2()
            {
                var input = File.ReadLines($"{INPUT_PATH}day13.txt");
                var dots = input.Where(l => Regex.IsMatch(l, @"^\d")).Select(l =>
                {
                    var a = l.Split(',');
                    return (int.Parse(a[0]), int.Parse(a[1]));
                });
                var folds = input.Where(l => l.StartsWith("fold")).Select(l =>
                {
                    var a = l[11..].Split('=');
                    return (a[0], int.Parse(a[1]));
                });
                var width = dots.Select(d => d.Item1).Max();
                var height = dots.Select(d => d.Item2).Max();

                var grid = new List<List<bool>>();

                for (int i = 0; i <= width; i++)
                {
                    grid.Add(Enumerable.Repeat(false, height + 1).ToList());
                }

                foreach (var dot in dots)
                {
                    grid[dot.Item1][dot.Item2] = true;
                }

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
                                    {
                                        grid[x][y] |= grid[x][height - y];
                                    }
                                }

                                foreach (var col in grid)
                                {
                                    col.RemoveRange(newSize, col.Count - newSize);
                                }
                                height = newSize - 1;
                            }
                            break;
                        case "x":
                            {
                                var newSize = width / 2;

                                for (int x = 0; x < newSize; x++)
                                {
                                    for (int y = 0; y < grid[y].Count; y++)
                                    {
                                        grid[x][y] |= grid[width - x][y];
                                    }
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
                    {
                        Console.Write(grid[x][y] ? "#" : ".");
                    }
                    Console.WriteLine();
                }

                // See console for result
                return string.Empty;
            }
        }
        private static class Day14
        {
            public static string Test1()
            {

                return string.Empty;
            }
            public static string Test2()
            {

                return string.Empty;
            }
        }
        private static class Day15
        {
            public static string Test1()
            {

                return string.Empty;
            }
            public static string Test2()
            {

                return string.Empty;
            }
        }
        private static class Day16
        {
            public static string Test1()
            {

                return string.Empty;
            }
            public static string Test2()
            {

                return string.Empty;
            }
        }
        private static class Day17
        {
            public static string Test1()
            {

                return string.Empty;
            }
            public static string Test2()
            {

                return string.Empty;
            }
        }
        private static class Day18
        {
            public static string Test1()
            {

                return string.Empty;
            }
            public static string Test2()
            {

                return string.Empty;
            }
        }
        private static class Day19
        {
            public static string Test1()
            {

                return string.Empty;
            }
            public static string Test2()
            {

                return string.Empty;
            }
        }
        private static class Day20
        {
            public static string Test1()
            {

                return string.Empty;
            }
            public static string Test2()
            {

                return string.Empty;
            }
        }
        private static class Day21
        {
            public static string Test1()
            {

                return string.Empty;
            }
            public static string Test2()
            {

                return string.Empty;
            }
        }
        private static class Day22
        {
            public static string Test1()
            {

                return string.Empty;
            }
            public static string Test2()
            {

                return string.Empty;
            }
        }
        private static class Day23
        {
            public static string Test1()
            {

                return string.Empty;
            }
            public static string Test2()
            {

                return string.Empty;
            }
        }
        private static class Day24
        {
            public static string Test1()
            {

                return string.Empty;
            }
            public static string Test2()
            {

                return string.Empty;
            }
        }
        private static class Day25
        {
            public static string Test1()
            {

                return string.Empty;
            }
            public static string Test2()
            {

                return string.Empty;
            }
        }
        #endregion
    }
}
