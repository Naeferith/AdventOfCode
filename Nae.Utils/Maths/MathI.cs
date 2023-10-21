using System.Numerics;
using System.Runtime.InteropServices;

namespace Nae.Utils.Maths
{
    /// <summary>
    /// Provides methods and algorithms treating with interger numbers.
    /// </summary>
    public static class MathI
    {
        /// <summary>
        /// Computes the Greatest Common Divisor (GCD) of an integer.
        /// </summary>
        /// <typeparam name="T">Type of the integer</typeparam>
        /// <param name="a">First integer</param>
        /// <param name="b">Second integer</param>
        /// <returns>The GCD</returns>
        public static T Gcd<T>(T a, T b) where T : IBinaryInteger<T>
        {
            while (b != default)
            {
                T temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        /// <summary>
        /// Computes the Least Common Multiple (LCM) of an integer collection.
        /// </summary>
        /// <typeparam name="T">Type of the integers</typeparam>
        /// <param name="numerics">Integer collection</param>
        /// <returns>The LCM</returns>
        public static T Lcm<T>(IEnumerable<T> numerics) where T : IBinaryInteger<T>
        {
            var res = T.One;
            foreach (var i in numerics)
            {
                res = res / Gcd(res, i) * i;
            }
            return res;
        }

        public static bool IsBetweenInclusive<T>(this T val, T lowerBound, T upperBound) where T : IComparisonOperators<T, T, bool>
        {
            return val >= lowerBound && val <= upperBound;
        }

        /// <summary>
        /// Converts an integer into another base
        /// </summary>
        /// <typeparam name="T">Type of the integer</typeparam>
        /// <param name="value">Integer to be converted</param>
        /// <param name="baseChars">Ordered array of token of the target base</param>
        /// <returns>A string representation of the new base</returns>
        public static ReadOnlySpan<char> NumberToStringBase<T>(T value, ReadOnlySpan<char> baseChars) where T : IBinaryInteger<T>
        {
            int max = Marshal.SizeOf(value) << 3;
            var i = max;
            var buffer = new char[i];
            var targetBase = T.CreateChecked(baseChars.Length);

            do
            {
                var (_, Remainder) = T.DivRem(value, targetBase);

                buffer[--i] = baseChars[int.CreateChecked(Remainder)];
                value /= targetBase;
            } while (value > T.Zero);

            var result = new char[max - i];
            Array.Copy(buffer, i, result, 0, max - i);

            return result;
        }
    }
}
