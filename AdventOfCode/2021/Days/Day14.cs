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
                var polymer = input.First();
                var rules = input.Where(l => l.Contains("-")).Select(l =>
                {
                    var a = l.Split(new string[] { " -> " }, StringSplitOptions.None);
                    return (a[0], a[1]);
                });

                for (var i = 0; i < polymer.Length; i += 2)
                {

                }

                var ret = DivideAndConquer(input.First(), steps, rules).GroupBy(c => c).OrderByDescending(g => g.LongCount());

                return (ret.First().LongCount() - ret.Last().LongCount()).ToString();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="atom">2 char string</param>
            /// <param name="steps"></param>
            /// <param name="rules"></param>
            /// <returns></returns>
            private static string DivideAndConquer(string atom, int steps, IEnumerable<(string, string)> rules)
            {
                if (steps == 0)
                    return atom;

                var polymer = new StringBuilder(atom);
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

                    // here polymer.Length = 3
                    var s = polymer.ToString();
                    steps--;
                    doRules.Clear();

                    return DivideAndConquer(s[..1], steps, rules)[..1] + DivideAndConquer(s[1..], steps, rules);                    
                }

                return polymer.ToString();
            }
        }
    }
}
