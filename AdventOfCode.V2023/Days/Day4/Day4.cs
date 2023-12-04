using AdventOfCode.Core.Components;

namespace AdventOfCode.V2023.Days.Day4
{
    internal class Day4 : IDay
    {
        public int DayNumber => 4;

        public string PuzzleName => "Scratchcards";

        public string Solution1(string[] lines)
        {
            return lines
                .Select(SameNumbers)
                .Where(n => n > 0)
                .Select(n => Math.Pow(2, n - 1))
                .Sum()
                .ToString();
        }

        public string Solution2(string[] lines)
        {
            var cardTable = new Dictionary<int, int>(lines.Length);
            var sum = 0;

            for (int i = lines.Length - 1; i >= 0; i--)
            {
                cardTable[i] = Enumerable
                    .Range(i, SameNumbers(lines[i]))
                    .Select(i => cardTable[i + 1])
                    .Sum() + 1;

                sum += cardTable[i];
            }

            return sum.ToString();
        }

        private static int SameNumbers(string line)
        {
            var args = line.Split(" | ");
            var winNum = args[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Skip(2);
            var playNum = args[1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            return winNum.Intersect(playNum).Count();
        }
    }
}
