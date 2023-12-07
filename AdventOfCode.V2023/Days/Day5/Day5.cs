using AdventOfCode.Core.Components;

namespace AdventOfCode.V2023.Days.Day5
{
    internal class Day5 : IDay
    {
        public int DayNumber => 5;

        public string PuzzleName => "If You Give A Seed A Fertilizer";

        public string Solution1(string[] lines)
        {
            var seeds = lines[0].Split(' ').Skip(1).Select(long.Parse).ToArray();
            var maps = new List<List<Map>>();

            // Parse data
            var skipNext = false;

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrEmpty(line))
                {
                    maps.Add([]);
                    skipNext = true;
                }

                else if (skipNext)
                {
                    skipNext = false;
                    continue;
                }
                else
                {
                    var args = line.Split(' ');
                    maps[^1].Add(new()
                    {
                        DstStart = long.Parse(args[0]),
                        SrcStart = long.Parse(args[1]),
                        Range = long.Parse(args[2]),
                    });
                }
            }

            return seeds
                .Select(s => Compute(s, maps))
                .Min()
                .ToString();
        }

        private static long Compute(long s, List<List<Map>> maps)
        {
            var work = s;

            foreach (var step in maps)
            {
                var m = step.Find(m => work >= m.SrcStart && work <= m.SrcStart + m.Range);

                if (m is null)
                {
                    continue;
                }

                work = work - m.SrcStart + m.DstStart;
            }

            return work;
        }

        public string Solution2(string[] lines)
        {
            var seeds = lines[0].Split(' ').Skip(1)
                .Select(long.Parse)
                .Chunk(2)
                .SelectMany(p => LongRange(p[0], p[1]));

            var maps = new List<List<Map>>();

            // Parse data
            var skipNext = false;

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrEmpty(line))
                {
                    maps.Add([]);
                    skipNext = true;
                }
                else if (skipNext)
                {
                    skipNext = false;
                    continue;
                }
                else
                {
                    var args = line.Split(' ');

                    maps[^1].Add(new()
                    {
                        DstStart = long.Parse(args[0]),
                        SrcStart = long.Parse(args[1]),
                        Range = long.Parse(args[2]),
                    });
                }
            }

            return seeds
                .Select(s => Compute(s, maps))
                .Min()
                .ToString();
        }

        private static IEnumerable<long> LongRange(long start, long range)
        {
            for (long i = start; i < start + range; i++)
            {
                yield return i;
            }
        }
    }
}
