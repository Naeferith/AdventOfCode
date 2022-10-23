using AdventOfCode.Core.AoC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.V2021.Days
{
    internal class Day12 : IDay
    {
        public string PuzzleName => "Passage Pathing";

        public string Solution1(string[] lines)
        {
            var input = lines.Select(l =>
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

        public string Solution2(string[] lines)
        {
            var input = lines.Select(l =>
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
                    GetPath(next, path, paths);
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
                        GetPath2(next, path, paths, true);
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
            public override int GetHashCode() => Id.GetHashCode();

            public bool Equals(Cave other) => other?.Id.Equals(Id) ?? false;

            public override string ToString() => Id;
        }
    }
}
