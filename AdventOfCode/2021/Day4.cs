using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Business
{
    internal sealed partial class AoC21
    {
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
                        boards.Last()[nLine, num.Index] = (num.Value, false);

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
                        boards.Last()[nLine, num.Index] = (num.Value, false);

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
                        sum += board[x, y].Item2 ? 0 : board[x, y].Item1;
                }
                return sum * pull;
            }
        }
    }
}
