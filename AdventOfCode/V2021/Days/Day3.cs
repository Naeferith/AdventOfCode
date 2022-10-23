using AdventOfCode.Core.AoC;
using System;
using System.Linq;

namespace AdventOfCode.V2021.Days
{
    internal class Day3 : IDay
    {
        private const int BINARY_SIZE = 12;

        public string PuzzleName => "Binary Diagnostic";

        public string Solution1(string[] lines)
        {
            var count = lines.Length;
            var gammaRate = new int[BINARY_SIZE];

            for (int i = 0; i < BINARY_SIZE; i++)
                gammaRate[i] = (int)Math.Round(lines.Select(b => int.Parse(b[i].ToString())).Sum() / (double)count);

            var gammaB = gammaRate.Select(i => i == 1);
            var epsilonB = gammaB.Select(b => !b);

            var gammaStr = string.Join(string.Empty, gammaRate);
            var epsilonStr = string.Join(string.Empty, epsilonB.Select(b => b ? "1" : "0"));

            return (Convert.ToInt32(gammaStr, 2) * Convert.ToInt32(epsilonStr, 2)).ToString();
        }

        public string Solution2(string[] lines)
        {
            var count = lines.Length;
            var o2Enum = lines.ToList();
            var co2Enum = lines.ToList();

            for (int i = 0; i < BINARY_SIZE; i++)
            {
                if (o2Enum.Count > 1)
                {
                    var v = (int)Math.Round(o2Enum.Select(b => int.Parse(b[i].ToString())).Sum() / (double)o2Enum.Count, MidpointRounding.AwayFromZero);
                    o2Enum = o2Enum.Where(b => b[i].ToString().Equals(v.ToString())).ToList();
                }

                if (co2Enum.Count > 1)
                {
                    var v = (int)Math.Round(co2Enum.Select(b => int.Parse(b[i].ToString())).Sum() / (double)co2Enum.Count, MidpointRounding.AwayFromZero);
                    co2Enum = co2Enum.Where(b => !b[i].ToString().Equals(v.ToString())).ToList();
                }

                if (o2Enum.Count == 1 && co2Enum.Count == 1)
                    break;
            }

            return (Convert.ToInt32(o2Enum.First(), 2) * Convert.ToInt32(co2Enum.First(), 2)).ToString();
        }
    }
}
