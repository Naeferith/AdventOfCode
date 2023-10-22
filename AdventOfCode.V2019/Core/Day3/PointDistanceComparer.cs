using Nae.Utils.Structs;

namespace AdventOfCode.V2019.Core.Day3
{
    internal class PointDistanceComparer : IComparer<Point<int>>
    {
        public int Compare(Point<int> x, Point<int> y)
        {
            var zero = Point<int>.Zero;

            var d1 = Point<int>.ManhattanDistance(zero, x);
            var d2 = Point<int>.ManhattanDistance(zero, y);

            return d1 - d2;
        }
    }
}
