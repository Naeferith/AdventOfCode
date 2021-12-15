﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AdventOfCode
{
    public class AoC : IAoC
    {
        public int Year { get; }

        private AoC(int year)
        {
            Year = year;
        }

        public string GetPuzzle(int day, int version)
        {
            if (day < 1 || day > 25)
                throw new ArgumentOutOfRangeException(nameof(day));

            var d = GetDay(day);
            var input = File.ReadAllLines(Path(day));

            return version switch
            {
                1 => d.Solution1(input),
                2 => d.Solution2(input),
                _ => throw new InvalidOperationException(nameof(version)),
            };
        }

        private string Path(int day) => $@"./{Year}/Input/day{day}.txt";

        private IDay GetDay(int day)
        {
            var dType = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass && (t.Namespace?.Equals($"{nameof(AdventOfCode)}.V{Year}") ?? false))
                .FirstOrDefault(t => t.Name.Equals($"Day{day}"));

            if (dType == null)
                throw new NotImplementedException(nameof(day));

            return Activator.CreateInstance(dType) as IDay;
        }

        public static IAoC GetCalendar(int year)
        {
            return year switch
            {
                2021 => new AoC(year),
                _ => throw new NotImplementedException("Année incorrecte"),
            };
        }
    }
}
