using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Business
{
    internal sealed partial class AoC21
    {
        private static class Day14
        {
            public static string Test1() => Test(10);

            public static string Test2() => Test(40);

            private static string Test(int steps)
            {
                var input = File.ReadAllLines($"{INPUT_PATH}day14.txt");
                var polymer = new StringBuilder(input.First());
                var rules = input.Where(l => l.Contains("-")).Select(l =>
                {
                    var a = l.Split(new string[] { " -> " }, StringSplitOptions.None);
                    return (a[0], a[1]);
                }).OrderBy(h => h.Item1);

                var doRules = new List<(Group, string)>();

                for (int i = 0; i < steps; i++)
                {
                    foreach (var rule in rules)
                    {
                        var matches = Regex.Matches(polymer.ToString(), $"(?={rule.Item1})");
                        foreach (Match match in matches)
                        {
                            foreach (var g in match.Groups.Values)
                            {
                                doRules.Add((g, rule.Item2));
                            }
                        }
                    }

                    var orderedRules = doRules.OrderBy(g => g.Item1.Index);

                    var j = 0;
                    foreach (var group in orderedRules)
                    {
                        polymer.Insert(group.Item1.Index + 1 + j, group.Item2);
                        j++;
                    }

                    doRules.Clear();
                }
                var ret = polymer.ToString().GroupBy(c => c).OrderByDescending(g => g.LongCount());

                return (ret.First().LongCount() - ret.Last().LongCount()).ToString();
            }
        }
    }
}
