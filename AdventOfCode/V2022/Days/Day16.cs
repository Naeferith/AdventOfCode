using AdventOfCode.Core.AoC;
using AdventOfCode.Core.Utils;
using Dijkstra.NET.Graph;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.V2022.Days
{
    internal class Day16 : IDay
    {
        private Graph<int, string> _graph;
        public string PuzzleName => "Proboscidea Volcanium";

        public string Solution1(string[] lines)
        {
            Initialize(lines);

            // First, get all nodes which have a flow
            var importantNodes = new List<uint>();
            foreach (var node in _graph)
            {
                if (node.Item > 0)
                    importantNodes.Add(node.Key);
            }

            foreach (var item in importantNodes.Permutate())
            {
            }

            return GetCalories(lines).Max().ToString();
        }

        public string Solution2(string[] lines)
        {
            var elves = GetCalories(lines);
            elves.Sort();
            return elves.TakeLast(3).Sum().ToString();
        }

        private static List<int> GetCalories(string[] lines)
        {
            var elves = new List<int>();
            var calories = 0;

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    elves.Add(calories);
                    calories = 0;
                }
                else
                {
                    calories += int.Parse(line);
                }
            }

            return elves;
        }

        private void Initialize(string[] lines)
        {
            var nodes = new Dictionary<string, uint>();
            _graph = new();

            // Split nodes and edges
            var graphItems = lines.Select(l => l.Split(';')).ToArray();

            for (int i = 0; i < 2; i++)
            {
                foreach (var item in graphItems)
                {
                    var nodeArgs = item[0].Split(' ');
                    // Build Nodes
                    if (i == 0)
                    {
                        nodes.Add(nodeArgs[1], _graph.AddNode(int.Parse(nodeArgs[^1].Split('=').Last())));
                    }
                    // Build Edges
                    else
                    {
                        var destNodes = item[i].Split(", ").ToArray();
                        destNodes[0] = destNodes[0].Split(' ').Last();

                        foreach (var node in destNodes)
                        {
                            _graph.Connect(nodes[nodeArgs[1]], nodes[node], 1, null);
                        }
                    }
                }
            }
        }
    }
}
