using AdventOfCode.Business;
using System;

namespace AdventOfCode
{
    public abstract class AoC : IAoC
    {
        public static IAoC GetCalendar(int year)
        {
            return year switch
            {
                2021 => new AoC21(),
                _ => throw new NotImplementedException("Année incorrecte"),
            };
        }

        #region Days
        public abstract string Day1_1();
        public abstract string Day1_2();
        public abstract string Day2_1();
        public abstract string Day2_2();
        public abstract string Day3_1();
        public abstract string Day3_2();
        public abstract string Day4_1();
        public abstract string Day4_2();
        public abstract string Day5_1();
        public abstract string Day5_2();
        public abstract string Day6_1();
        public abstract string Day6_2();
        public abstract string Day7_1();
        public abstract string Day7_2();
        public abstract string Day8_1();
        public abstract string Day8_2();
        public abstract string Day9_1();
        public abstract string Day9_2();
        public abstract string Day10_1();
        public abstract string Day10_2();
        public abstract string Day11_1();
        public abstract string Day11_2();
        public abstract string Day12_1();
        public abstract string Day12_2();
        public abstract string Day13_1();
        public abstract string Day13_2();
        public abstract string Day14_1();
        public abstract string Day14_2();
        public abstract string Day15_1();
        public abstract string Day15_2();
        public abstract string Day16_1();
        public abstract string Day16_2();
        public abstract string Day17_1();
        public abstract string Day17_2();
        public abstract string Day18_1();
        public abstract string Day18_2();
        public abstract string Day19_1();
        public abstract string Day19_2();
        public abstract string Day20_1();
        public abstract string Day20_2();
        public abstract string Day21_1();
        public abstract string Day21_2();
        public abstract string Day22_1();
        public abstract string Day22_2();
        public abstract string Day23_1();
        public abstract string Day23_2();
        public abstract string Day24_1();
        public abstract string Day24_2();
        public abstract string Day25_1();
        public abstract string Day25_2();
        #endregion
    }
}
