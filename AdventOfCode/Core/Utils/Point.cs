using System;

namespace AdventOfCode.Core.Utils
{
    internal struct Point : IEquatable<Point>
    {
        public static Point UnitX => new(1, 0);
        public static Point One => new(1);
        public static Point UnitY => new(0, 1);
        public static Point Zero => new();

        public int X;
        public int Y;

        public Point(int value) : this(value, value)
        {
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point(Point point)
        {
            X = point.X;
            Y = point.Y;
        }

        public static Point Abs(Point value)
        {
            return new(Math.Abs(value.X), Math.Abs(value.Y));
        }
        public static Point Add(Point left, Point right)
        {
            return left + right;
        }
        public static int ManhattanDistance(Point value1, Point value2)
        {
            return Math.Abs(value1.X - value2.X) + Math.Abs(value1.Y - value2.Y);
        }
        public static int CrossProduct(Point point, Point value1, Point value2)
        {
            var dxc = point.X - value1.X;
            var dyc = point.Y - value1.Y;

            var dxl = value2.X - value1.X;
            var dyl = value2.Y - value1.Y;

            return dxc * dyl - dxl * dyc;
        } 
        public static Point Divide(Point left, Point right)
        {
            return left / right;
        }
        public static Point Divide(Point left, int divisor)
        {
            return left / divisor;
        }
        public static Point Multiply(int left, Point right)
        {
            return left * right;
        }
        public static Point Multiply(Point left, int right)
        {
            return left * right;
        }
        public static Point Multiply(Point left, Point right)
        {
            return left * right;
        }
        public static Point Negate(Point value)
        {
            return new(-value.X, -value.Y);
        }

        public static Point operator +(Point left, Point right)
        {
            return new(left.X + right.X, left.Y + right.Y);
        }
        public static Point operator -(Point left, Point right)
        {
            return new(left.X - right.X, left.Y - right.Y);
        }
        public static Point operator *(int left, Point right)
        {
            return new(left * right.X, left * right.Y);
        }
        public static Point operator *(Point left, int right)
        {
            return new(left.X * right, left.Y * right);
        }
        public static Point operator *(Point left, Point right)
        {
            return new(left.X * right.X, left.Y * right.Y);
        }
        public static Point operator /(Point left, Point right)
        {
            return new(left.X / right.X, left.Y / right.Y);
        }
        public static Point operator /(Point value1, int value2)
        {
            return new(value1.X / value2, value1.Y / value2);
        }
        public static bool operator ==(Point left, Point right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Point left, Point right)
        {
            return !left.Equals(right);
        }

        public bool Equals(Point other)
        {
            return other.X == X && other.Y == Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Point point && Equals(point);
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"{{x: {X}, y: {Y}}}";
        }
    }
}
