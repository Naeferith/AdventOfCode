using System;
using System.Collections.Generic;
using System.Numerics;

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
            var res = (T)Convert.ChangeType(1, typeof(T));
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
    }
}
