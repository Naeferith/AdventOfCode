using AdventOfCode.Core.Components;

namespace AdventOfCode.V2022.Days
{
    internal class Day6 : IDay
    {
        public int DayNumber => 6;

        public string PuzzleName => "Tuning Trouble";

        public string Solution1(string[] lines)
        {
            return Solution(lines[0], 4);
        }

        public string Solution2(string[] lines)
        {
            return Solution(lines[0], 14);
        }

        private static string Solution(string data, int buffSize)
        {
            var buffer = new List<char>();
            for (int i = 0; i < buffSize; i++) buffer.Add(data[i]);

            for (int i = buffSize; i < data.Length; i++)
            {
                buffer.RemoveAt(0);
                buffer.Add(data[i]);

                if (buffer.Distinct().Count() == buffSize)
                    return (i + 1).ToString();
            }

            return null;
        }
    }
}
