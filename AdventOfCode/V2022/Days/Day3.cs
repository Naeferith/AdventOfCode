using AdventOfCode.Core.AoC;
using System.Linq;

namespace AdventOfCode.V2022.Days
{
    internal class Day3 : IDay
    {
        public string PuzzleName => "Rucksack Reorganization";

        public string Solution1(string[] lines)
        {
            var sum = 0;

            foreach (var backpack in lines)
            {
                var part1 = backpack[..(backpack.Length/2)];
                var part2 = backpack[(backpack.Length/2)..];

                sum += GetPriority(part1.Intersect(part2).First());
            }

            return sum.ToString();
        }

        public string Solution2(string[] lines)
        {
            var sum = 0;

            foreach (var triple in lines.Chunk(3))
            {
                sum += GetPriority(triple[0].Intersect(triple[1]).Intersect(triple[2]).First());
            }

            return sum.ToString();
        }

        private static int GetPriority(char c)
        {
            if (c >= 'a' && c <= 'z')
                c -= '`';
            else
            {
                c -= 'A';
                c += '\u001b';
            }

            return c;
        }
    }
}
