using AdventOfCode.Core.Components;

namespace AdventOfCode.V2022.Days
{
    internal class Day6 : Day
    {
        public override int DayNumber => 6;

        public override string PuzzleName => "Tuning Trouble";

        protected override string Solution1(string[] lines)
        {
            return Solution(lines[0], 4);
        }

        protected override string Solution2(string[] lines)
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
