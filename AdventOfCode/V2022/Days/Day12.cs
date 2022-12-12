using AdventOfCode.Core.AoC;
using Dijkstra.NET.Graph.Simple;
using Dijkstra.NET.ShortestPath;

namespace AdventOfCode.V2022.Days
{
    internal class Day12 : IDay
    {
        private Graph _graph;
        private uint StartNode { get; set; }
        private uint EndNode { get; set; }

        public string PuzzleName => "Hill Climbing Algorithm";

        public string Solution1(string[] lines)
        {
            Initialize(lines);

            var sp = _graph.Dijkstra(StartNode, EndNode);

            return sp.Distance.ToString();
        }

        public string Solution2(string[] lines)
        {
            Initialize(lines);

            var smallest = int.MaxValue;

            for (int x = 0; x < lines.Length; x++)
            {
                var width = lines[0].Length;

                for (int y = 0; y < width; y++)
                {
                    if (GetValue(lines[x][y]) == 'a')
                    {
                        var sp = _graph.Dijkstra((uint)(x * width + y + 1), EndNode);

                        if (sp.Distance < smallest)
                            smallest = sp.Distance;
                    }
                }
            }

            return smallest.ToString();
        }

        private void Initialize(string[] lines)
        {
            _graph = new();

            // Build Nodes
            for (int x = 0; x < lines.Length; x++)
            {
                var width = lines[0].Length;

                for (int y = 0; y < width; y++)
                {
                    var node = _graph.AddNode();
                    var value = GetValue(lines[x][y]);

                    // Get Start and End
                    if (lines[x][y] == 'S')
                    {
                        StartNode = (uint)(x * lines[0].Length + y + 1);
                    }

                    if (lines[x][y] == 'E')
                    {
                        EndNode = (uint)(x * lines[0].Length + y + 1);
                    }

                    // Connect
                    if (x > 0)
                    {
                        // -> Down
                        if ((value - GetValue(lines[x - 1][y])) < 2)
                        {
                            _graph.Connect(node - (uint)width, node, 1);
                        }

                        // -> Up
                        if ((GetValue(lines[x - 1][y]) - value) < 2)
                        {
                            _graph.Connect(node, node - (uint)width, 1);
                        }
                    }

                    if (y > 0)
                    {
                        // -> Right
                        if ((value - GetValue(lines[x][y - 1])) < 2)
                        {
                            _graph.Connect(node - 1, node, 1);
                        }
                        // -> Left
                        if ((GetValue(lines[x][y - 1]) - value) < 2)
                        {
                            _graph.Connect(node, node - 1, 1);
                        }
                    }
                }
            }
        }

        private static char GetValue(char c)
        {
            if (c == 'S')
                return 'a';

            if (c == 'E')
                return 'z';

            return c;
        }

        private static int GetCost(char source, char dest)
        {
            var workStart = source == 'S' ? 'a' : source;
            var workDest = dest == 'E' ? 'z' : dest;

            return workDest - workStart > 1 ? short.MaxValue : 1;
        }

        private static bool SetIncluded((int, int) min, (int, int) max)
        {
            return max.Item1 <= min.Item1 && max.Item2 >= min.Item2;
        }

        private static bool Overlaps((int, int) min, (int, int) max)
        {
            return min.Item2 >= max.Item1;
        }
    }
}
