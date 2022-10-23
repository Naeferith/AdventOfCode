using System.Collections.Generic;
using System.Linq;

<<<<<<<< HEAD:AdventOfCode/src/Utils/Utils.cs
namespace AdventOfCode.Utils
========
namespace AdventOfCode.Core
>>>>>>>> 76ad3ed43ea3a34dcdb9fbae1fe03cbd6294d094:AdventOfCode/Core/Utils.cs
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
    }
}
