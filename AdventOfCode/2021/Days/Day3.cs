using System;
using System.IO;
using System.Linq;

namespace AdventOfCode.Business
{
    internal sealed partial class AoC21
    {
        private static class Day3
        {
            public static string Test1()
            {
                const int binarySize = 12;
                var binaries = File.ReadLines($"{INPUT_PATH}day3.txt");
                var count = binaries.Count();
                var gammaRate = new int[binarySize];

                for (int i = 0; i < binarySize; i++)
                    gammaRate[i] = (int)Math.Round(binaries.Select(b => int.Parse(b[i].ToString())).Sum() / (double)count);

                var gammaB = gammaRate.Select(i => i == 1);
                var epsilonB = gammaB.Select(b => !b);

                var gammaStr = string.Join(string.Empty, gammaRate);
                var epsilonStr = string.Join(string.Empty, epsilonB.Select(b => b ? "1" : "0"));

                return (Convert.ToInt32(gammaStr, 2) * Convert.ToInt32(epsilonStr, 2)).ToString();
            }

            public static string Test2()
            {
                const int binarySize = 12;

                var binaries = File.ReadLines($"{INPUT_PATH}day3.txt");
                var count = binaries.Count();
                var o2Enum = binaries.ToList();
                var co2Enum = binaries.ToList();

                for (int i = 0; i < binarySize; i++)
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
}
