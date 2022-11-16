using AdventOfCode.Core.Utils;
using System.Collections.Generic;

namespace AdventOfCode.V2019.Core.Day3
{
    internal class PointDistanceComparer : IComparer<Point>
    {
        public int Compare(Point x, Point y)
        {
            var zero = Point.Zero;

            var d1 = Point.ManhattanDistance(zero, x);
            var d2 = Point.ManhattanDistance(zero, y);

            return d1 - d2;
        }
    }
}
