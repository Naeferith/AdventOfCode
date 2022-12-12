using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Core.Utils
{
    internal static class Utils
    {
        public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value));
        }

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
    }
}
