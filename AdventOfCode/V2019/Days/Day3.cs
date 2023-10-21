using AdventOfCode.Core.AoC;
using AdventOfCode.V2019.Core.Day3;
using Nae.Utils.Structs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.V2019.Days
{
    internal class Day3 : Day
    {
        public override string PuzzleName => "Crossed Wires";

        protected override string Solution1(string[] lines)
        {
            var wire1 = BuildWire(lines[0].Split(','));
            var wire2 = BuildWire(lines[1].Split(','));

            var intersections = GetIntersections(wire1, wire2);
            var closest = intersections.OrderBy(p => p, new PointDistanceComparer()).First();
            return Point.ManhattanDistance(Point.Zero, closest).ToString();
        }

        protected override string Solution2(string[] lines)
        {
            throw new NotImplementedException();
        }

        private static List<Segment> BuildWire(IEnumerable<string> directions)
        {
            var work = Point.Zero;
            ReadOnlySpan<char> span;
            var wire = new List<Segment>(directions.Count() - 1);

            foreach (var item in directions)
            {
                var start = new Point(work);
                span = item;
                var value = int.Parse(span[1..]);

                switch (span[0])
                {
                    case 'R':
                        work = new(work.X + value, work.Y);
                        break;
                    case 'L':
                        work = new(work.X - value, work.Y);
                        break;
                    case 'U':
                        work = new(work.X, work.Y - value);
                        break;
                    case 'D':
                        work = new(work.X, work.Y + value);
                        break;
                }

                wire.Add(new()
                {
                    Start = start,
                    End = new(work)
                });
            }

            return wire;
        }

        private static List<Point> GetIntersections(List<Segment> wire1, List<Segment> wire2)
        {
            var result = new List<Point>();
            var combins = wire1.SelectMany(c => wire2.Select(g => new { Segment1 = c, Segment2 = g }));

            foreach (var combin in combins)
            {
                if (Segment.TryIntersect(combin.Segment1, combin.Segment2, out var intersect))
                    result.Add(intersect.Value);
            }

            return result;
        }
    }
}
