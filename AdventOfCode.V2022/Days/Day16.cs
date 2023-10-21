using AdventOfCode.Core.Components;
using Dijkstra.NET.Graph;

namespace AdventOfCode.V2022.Days
{
    internal class Day16 : IDay
    {
        private Graph<int, string> _graph;

        public int DayNumber => 16;

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

            throw new NotImplementedException();
        }

        public string Solution2(string[] lines)
        {
            Initialize(lines);

            throw new NotImplementedException();
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
                        nodes.Add(nodeArgs[1], _graph.AddNode(int.Parse(nodeArgs[^1].Split('=')[^1])));
                    }
                    // Build Edges
                    else
                    {
                        var destNodes = item[i].Split(", ").ToArray();
                        destNodes[0] = destNodes[0].Split(' ')[^1];

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
