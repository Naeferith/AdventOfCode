namespace AdventOfCode.V2022.Core.Day9
{
    internal class Point : IEquatable<Point>
    {
        public static Point Zero => new();

        public int X { get; set; }
        public int Y { get; set; }

        public Point()
        {
            X = 0;
            Y = 0;
        }

        public Point(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
