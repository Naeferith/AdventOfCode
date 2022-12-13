using System.Collections.Generic;
using System.Linq;

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

        /// <summary>
        /// Vérifie si le caratère est numérique.
        /// </summary>
        /// <param name="c">Le caractère à tester</param>
        /// <returns>
        ///     <see langword="true"/> si le caractère est compris entre 0 et 9, <see langword="false"/> sinon.
        /// </returns>
        public static bool IsNumeric(this char c) => c.IsBetweenInclusive('0', '9');
    }
}
