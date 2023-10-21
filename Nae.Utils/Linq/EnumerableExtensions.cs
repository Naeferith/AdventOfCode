using Nae.Utils.Maths;

namespace Nae.Utils.Linq
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value));
        }

        /// <summary>
        /// Extrait les bornes inferieures et supérieures de l'<see cref="IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">Le type de l'énumeration</typeparam>
        /// <param name="source">L'énumeration à parcourir</param>
        /// <returns>Un tuple (min, max)</returns>
        public static (T Lowest, T Highest) Bounds<T>(this IEnumerable<T> source) where T : IComparable<T>
        {
            T min = default, max = default;

            foreach (var item in source)
            {
                if (item?.CompareTo(min) < 0) min = item;
                else if (item?.CompareTo(max) > 0) max = item;
            }

            return (min, max);
        }

        /// <summary>
        /// Vérifie si le caratère est numérique.
        /// </summary>
        /// <param name="c">Le caractère à tester</param>
        /// <returns>
        ///     <see langword="true"/> si le caractère est compris entre 0 et 9, <see langword="false"/> sinon.
        /// </returns>
        public static bool IsNumeric(this char c) => c.IsBetweenInclusive('0', '9');

        public static IEnumerable<T[]> Permutate<T>(this IEnumerable<T> source)
        {
            return Inner_Permutate(source, Enumerable.Empty<T>());

            IEnumerable<T[]> Inner_Permutate(IEnumerable<T> reminder, IEnumerable<T> prefix) =>
                !reminder.Any() ? new[] { prefix.ToArray() } :
                reminder.SelectMany((c, i) => Inner_Permutate(
                    reminder.Take(i).Concat(reminder.Skip(i + 1)).ToArray(),
                    prefix.Append(c)));
        }
    }
}
