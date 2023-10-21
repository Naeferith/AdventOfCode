using AdventOfCode.Core.AoC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.V2021.Days
{
    internal class Day14 : Day
    {
        public override string PuzzleName => "Extended Polymerization";

        protected override string Solution1(string[] lines) => Solve(10, lines);

        protected override string Solution2(string[] lines) => Solve(40, lines);

        private static string Solve(int steps, string[] input)
        {
            var polymer = input.First();
            var rules = input.Where(l => l.Contains("-")).Select(l =>
            {
                var a = l.Split(new string[] { " -> " }, StringSplitOptions.None);
                return (a[0], Convert.ToChar(a[1]));
            }).ToDictionary(r => r.Item1, r => r.Item2);
            var counts = polymer.GroupBy(x => x).ToDictionary(y => y.Key, y => y.LongCount());

            var pairs = new Dictionary<string, long>();
            for (int i = 0; i < polymer.Length - 1; i++)
            {
                if (pairs.ContainsKey(polymer[i].ToString() + polymer[i + 1]))
                    pairs[polymer[i].ToString() + polymer[i + 1]]++;
                else
                    pairs.Add(polymer[i].ToString() + polymer[i + 1], 1);
            }

            for (int i = 0; i < steps; i++)
            {
                foreach (var pair in pairs)
                {
                    if (counts.ContainsKey(rules[pair.Key]))
                        counts[rules[pair.Key]] += pair.Value;
                    else
                        counts.Add(rules[pair.Key], 1);
                }
                pairs = NewPairs(rules, pairs);
            }

            var ret = counts.OrderByDescending(e => e.Value).Select(e => e.Value);

            return (ret.First() - ret.Last()).ToString();
        }

        private static Dictionary<string, long> NewPairs(Dictionary<string, char> rules, Dictionary<string, long> pairs)
        {
            var pairsNew = new Dictionary<string, long>();
            foreach (var pair in pairs)
            {
                var one = pair.Key[0].ToString() + rules[pair.Key];
                var two = rules[pair.Key].ToString() + pair.Key[1];

                if (pairsNew.ContainsKey(one))
                    pairsNew[one] += pair.Value;
                else
                    pairsNew.Add(one, pair.Value);

                if (pairsNew.ContainsKey(two))
                    pairsNew[two] += pair.Value;
                else
                    pairsNew.Add(two, pair.Value);
            }
            return pairsNew;
        }
    }
}
