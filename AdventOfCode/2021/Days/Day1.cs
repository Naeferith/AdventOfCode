using System.IO;
using System.Linq;

namespace AdventOfCode.Business
{
    internal sealed partial class AoC21
    {
        private static class Day1
        {
            public static string Solution1()
            {
                var depths = File.ReadLines($"{INPUT_PATH}day1.txt").Select(t => int.Parse(t));
                var previous = depths.First();
                var count = 0;

                foreach (var depth in depths)
                {
                    count += (depth > previous) ? 1 : 0;
                    previous = depth;
                }

                return count.ToString();
            }

            public static string Solution2()
            {
                const int slideSize = 3;

                var depths = File.ReadLines($"{INPUT_PATH}day1.txt").Select(t => int.Parse(t));
                var previous = depths.Take(slideSize).Sum();
                var count = 0;

                for (int i = 1; i < depths.Count() - (slideSize - 1); i++)
                {
                    var val = depths.Skip(i).Take(slideSize).Sum();
                    count += (val > previous) ? 1 : 0;
                    previous = val;
                }

                return count.ToString();
            }
        }
    }
}
