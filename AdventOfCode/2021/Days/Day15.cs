using Dijkstra.NET.Graph.Simple;
using Dijkstra.NET.ShortestPath;
using System.IO;
using System.Linq;

namespace AdventOfCode.Business
{
    internal sealed partial class AoC21
    {
        private static class Day15
        {
            public static string Solution1() => Solve(1);

            public static string Solution2() => Solve(5);

            private static string Solve(int size)
            {
                var input = File.ReadAllLines($"{INPUT_PATH}day15.txt").Select(l => l.Select(c => short.Parse(c.ToString())));
                var widthOne = input.First().Count();
                var heightOne = input.Count();
                var width = widthOne * size;
                var height = heightOne * size;

                var tiles = new short[height, width];

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var s = (x / widthOne) + (y / heightOne);
                        var val = ((input.ElementAt(y % heightOne).ElementAt(x % widthOne) + s - 1) % 9) + 1;
                        tiles[y, x] = (short)val;
                    }
                }

                var graph = BuildGraph(tiles, height, width);

                return graph.Dijkstra(1, (uint)graph.NodesCount).Distance.ToString();
            }

            private static Graph BuildGraph(short[,] grid, int height, int width)
            {
                var graph = new Graph();

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var idx = graph.AddNode();
                        if (x > 0)
                            graph.Connect(idx - 1, idx, grid[y, x]);
                        if (y > 0)
                            graph.Connect((uint)(idx - width), idx, grid[y, x]);
                    }
                }

                return graph;
            }
        }
    }
}
