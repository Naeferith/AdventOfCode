using AdventOfCode.Core.Components;

namespace AdventOfCode.V2022.Days
{
    internal class Day1 : IDay
    {
        public int DayNumber => 1;

        public string PuzzleName => "Calorie Counting";

        public string Solution1(string[] lines)
        {
            return GetCalories(lines).Max().ToString();
        }

        public string Solution2(string[] lines)
        {
            var elves = GetCalories(lines);
            elves.Sort();
            return elves.TakeLast(3).Sum().ToString();
        }

        private static List<int> GetCalories(string[] lines)
        {
            var elves = new List<int>();
            var calories = 0;

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    elves.Add(calories);
                    calories = 0;
                }
                else
                {
                    calories += int.Parse(line);
                }
            }

            return elves;
        }
    }
}
