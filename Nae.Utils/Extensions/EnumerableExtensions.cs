using System.Diagnostics.CodeAnalysis;

namespace Nae.Utils.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Extrait les bornes inferieures et supérieures de l'<see cref="IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="T">Le type de l'énumeration</typeparam>
        /// <param name="source">L'énumeration à parcourir</param>
        /// <returns>Un tuple (min, max)</returns>
        public static (T Lowest, T Highest) Bounds<T>(this IEnumerable<T> source) where T : IComparable<T>
        {
            T min = default!, max = default!;

            foreach (var item in source)
            {
                if (item?.CompareTo(min) < 0) min = item;
                else if (item?.CompareTo(max) > 0) max = item;
            }

            return (min, max);
        }

        public static IEnumerable<T[]> Permutate<T>(this IEnumerable<T> source)
        {
            return Inner_Permutate(source, Enumerable.Empty<T>());

            IEnumerable<T[]> Inner_Permutate(IEnumerable<T> reminder, IEnumerable<T> prefix) =>
                !reminder.Any() ? new[] { prefix.ToArray() } :
                reminder.SelectMany((c, i) => Inner_Permutate(
                    reminder.Take(i).Concat(reminder.Skip(i + 1)).ToArray(),
                    prefix.Append(c)));
        }

        public static IEnumerable<T> Range<T>(int length, Func<int, T> factory)
        {
            for (int i = 0; i < length; i++)
            {
                yield return factory(i);
            }
        }

        /// <summary>
        /// Perform an enumeration loop on a <see cref="IEnumerable{T}"/>.
        /// Since the returned <see cref="IEnumerable{T}"/> is infinite,
        /// consumers should :
        /// <para>
        ///     - Never materialize the <see cref="IEnumerable{T}"/>.
        /// </para>
        /// <para>
        ///     - Have a way to break any loops using this <see cref="IEnumerable{T}"/>.
        /// </para>
        /// </summary>
        /// <typeparam name="T">Type of the enumeration</typeparam>
        /// <param name="source">An infinite enumeration</param>
        /// <returns></returns>
        [SuppressMessage("Blocker Bug", "S2190:Loops and recursions should not be infinite", Justification = "Whole point of this method")]
        public static IEnumerable<T> Loop<T>(this IEnumerable<T> source)
        {
            while (true)
            {
                foreach (var item in source)
                {
                    yield return item;
                }
            }
        }
    }
}
