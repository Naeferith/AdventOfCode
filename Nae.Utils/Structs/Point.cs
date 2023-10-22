using System.Numerics;
using System.Runtime.CompilerServices;

namespace Nae.Utils.Structs
{
    /// <summary>
    /// Custom implementation of a Vector2 of <typeparamref name="T"/>
    /// </summary>
    public readonly struct Point<T> : IEquatable<Point<T>>
        where T : INumber<T>
    {
        public static Point<T> UnitX => new(T.One, T.Zero);
        public static Point<T> One => new(T.One);
        public static Point<T> UnitY => new(T.Zero, T.One);
        public static Point<T> Zero => new();

        public readonly T X;

        public readonly T Y;

        public Point(T value) : this(value, value)
        {
        }

        public Point(T x, T y)
        {
            X = x;
            Y = y;
        }

        public Point(Point<T> point)
        {
            X = point.X;
            Y = point.Y;
        }

        public Point<T> Left() => this - UnitX;

        public Point<T> Right() => this + UnitX;

        public Point<T> Down() => this - UnitY;

        public Point<T> Up() => this + UnitY;

        public static Point<T> Abs(Point<T> value)
        {
            return new(T.Abs(value.X), T.Abs(value.Y));
        }
        public static Point<T> Add(Point<T> left, Point<T> right)
        {
            return left + right;
        }
        public static T ManhattanDistance(Point<T> value1, Point<T> value2)
        {
            return T.Abs(value1.X - value2.X) + T.Abs(value1.Y - value2.Y);
        }
        public static T CrossProduct(Point<T> point, Point<T> value1, Point<T> value2)
        {
            var dxc = point.X - value1.X;
            var dyc = point.Y - value1.Y;

            var dxl = value2.X - value1.X;
            var dyl = value2.Y - value1.Y;

            return dxc * dyl - dxl * dyc;
        }
        public static Point<T> Divide(Point<T> left, Point<T> right)
        {
            return left / right;
        }
        public static Point<T> Divide(Point<T> left, T divisor)
        {
            return left / divisor;
        }
        public static Point<T> Multiply(T left, Point<T> right)
        {
            return left * right;
        }
        public static Point<T> Multiply(Point<T> left, T right)
        {
            return left * right;
        }
        public static Point<T> Multiply(Point<T> left, Point<T> right)
        {
            return left * right;
        }
        public static Point<T> Negate(Point<T> value)
        {
            return new(-value.X, -value.Y);
        }

        public static Point<T> operator +(Point<T> left, Point<T> right)
        {
            return new(left.X + right.X, left.Y + right.Y);
        }
        public static Point<T> operator -(Point<T> left, Point<T> right)
        {
            return new(left.X - right.X, left.Y - right.Y);
        }
        public static Point<T> operator *(T left, Point<T> right)
        {
            return new(left * right.X, left * right.Y);
        }
        public static Point<T> operator *(Point<T> left, T right)
        {
            return new(left.X * right, left.Y * right);
        }
        public static Point<T> operator *(Point<T> left, Point<T> right)
        {
            return new(left.X * right.X, left.Y * right.Y);
        }
        public static Point<T> operator /(Point<T> left, Point<T> right)
        {
            return new(left.X / right.X, left.Y / right.Y);
        }
        public static Point<T> operator /(Point<T> value1, T value2)
        {
            return new(value1.X / value2, value1.Y / value2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Point<T> left, Point<T> right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Point<T> left, Point<T> right)
        {
            return !left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly bool Equals(Point<T> other)
        {
            return other.X == X && other.Y == Y;
        }

        public override readonly bool Equals(object? obj)
        {
            return obj is Point<T> point && Equals(point);
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override readonly string ToString()
        {
            return $"{{x: {X}, y: {Y}}}";
        }
    }
}
