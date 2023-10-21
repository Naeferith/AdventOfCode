using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;

namespace AdventOfCode.Core.Utils
{
    internal static class Maths
    {
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

        public static T Lcm<T>(IEnumerable<T> numerics) where T : IBinaryInteger<T>
        {
            var res = T.One;
            foreach (var i in numerics)
            {
                res = res / Gcd(res, i) * i;
            }
            return res;
        }

        public static bool IsBetweenInclusive<T>(this T val, T lowerBound, T upperBound) where T : IBinaryInteger<T>
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
