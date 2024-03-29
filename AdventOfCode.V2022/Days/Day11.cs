﻿using AdventOfCode.Core.Components;
using AdventOfCode.V2022.Core.Day11;

namespace AdventOfCode.V2022.Days
{
    internal class Day11 : IDay
    {
        private IList<Monkey> _monkeys;

        public int DayNumber => 11;

        public string PuzzleName => "Monkey in the Middle";

        public string Solution1(string[] lines)
        {
            return Solution(lines, 20, true);
        }

        public string Solution2(string[] lines)
        {
            return Solution(lines, 10000, false);
        }

        private string Solution(string[] lines, int rounds, bool reduce)
        {
            Initialize(lines, reduce);

            for (int i = 0; i < rounds; i++)
            {
                foreach (var monkey in _monkeys)
                {
                    monkey.Inspect(_monkeys);
                }
            }

            return _monkeys
                .Select(m => m.InspectCount)
                .OrderDescending()
                .Take(2)
                .Aggregate(1UL, (a, b) => a * (ulong)b)
                .ToString();
        }

        private void Initialize(string[] lines, bool reduce)
        {
            _monkeys = new List<Monkey>();

            for (int i = 0; i < lines.Length; i += 7)
            {
                var work = new Monkey(reduce);

                // Add Items
                work.SetItems(lines[i + 1]
                    .Split(": ")
                    .Last()
                    .Split(", ")
                    .Select(int.Parse));

                // Add Operation
                work.SetOperation(lines[i + 2]);

                // Add Test
                work.SetTest(
                    int.Parse(lines[i + 3].Split(' ').Last()),
                    int.Parse(lines[i + 4].Split(' ').Last()),
                    int.Parse(lines[i + 5].Split(' ').Last())
                    );

                _monkeys.Add(work);
            }
        }
    }
}
