using AdventOfCode.Core;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.V2021.Days
{
    internal class Day6 : IDay
    {
        private const short REPRODUCTION_DAYS = 7;
        private const short MATURITY_DAYS = 2;

        public string PuzzleName => "Lanternfish";

        public string Solution1(string[] lines)
        {
            const short SIMULATION_DAYS = 80;

            var fishes = lines.Select(t => t.Split(',').Select(n => short.Parse(n)).ToList()).First();

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

        public string Solution2(string[] lines)
        {
            const int REPRODUCTION_DAYS = 7;
            const int MATURITY_DAYS = 2;
            const int SIMULATION_DAYS = 256;

            var input = lines.Select(t =>
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
