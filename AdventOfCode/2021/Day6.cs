using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Business
{
    internal sealed partial class AoC21
    {
        private static class Day6
        {
            public static string Test1()
            {
                const short REPRODUCTION_DAYS = 7;
                const short MATURITY_DAYS = 2;
                const short SIMULATION_DAYS = 80;

                var fishes = File.ReadLines($"{INPUT_PATH}day6.txt").Select(t => t.Split(',').Select(n => short.Parse(n)).ToList()).First();

                for (int i = 0; i < SIMULATION_DAYS; i++)
                {
                    var newFishes = new List<short>();

                    for (int j = 0; j < fishes.Count; j++)
                    {
                        if (fishes[j] == 0)
                        {
                            fishes[j] = REPRODUCTION_DAYS;
                            newFishes.Add(REPRODUCTION_DAYS + MATURITY_DAYS - 1);
                        }
                        fishes[j]--;
                    }
                    fishes.AddRange(newFishes);
                }

                return fishes.Count.ToString();
            }
            public static string Test2()
            {
                const int REPRODUCTION_DAYS = 7;
                const int MATURITY_DAYS = 2;
                const int SIMULATION_DAYS = 256;

                var input = File.ReadLines($"{INPUT_PATH}day6.txt").Select(t =>
                t
                .Split(',')
                .Select(n => int.Parse(n))
                .GroupBy(n => n)
                .Select(g => (g.Key, (ulong)g.Count()))).First();

                var fishes = new SortedDictionary<int, ulong>();

                for (int i = 0; i < MATURITY_DAYS + REPRODUCTION_DAYS; i++)
                    fishes[i] = 0;

                foreach (var tuple in input)
                    fishes[tuple.Key] = tuple.Item2;

                for (int i = 0; i < SIMULATION_DAYS; i++)
                {
                    var babyFishes = 0UL;
                    for (int j = 0; j < fishes.Count; j++)
                    {
                        if (j == 0)
                        {
                            babyFishes = fishes[j];
                            continue;
                        }
                        fishes[j - 1] = fishes[j];
                        if (j == fishes.Count - 1)
                            fishes[j] = 0;
                    }
                    fishes[REPRODUCTION_DAYS - 1] += babyFishes;
                    fishes[MATURITY_DAYS + REPRODUCTION_DAYS - 1] += babyFishes;
                }

                return fishes.Values.Aggregate((s, v) => s + v).ToString();
            }
        }
    }
}
