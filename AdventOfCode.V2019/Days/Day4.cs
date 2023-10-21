using AdventOfCode.Core.Components;

namespace AdventOfCode.V2019.Days
{
    internal class Day4 : IDay
    {
        public int DayNumber => 4;

        public string PuzzleName => "Secure Container";

        public string Solution1(string[] lines)
            => Solution(lines, s => HasDoubleDigit(s) && DoesntDecrease(s));

        public string Solution2(string[] lines)
            => Solution(lines, s => HasDoubleDigit(s) && DoesntDecrease(s) && IsOnlyDouble(s));

        private static string Solution(string[] lines, Func<string, bool> clause)
        {
            var range = lines[0].Split('-');
            var low = int.Parse(range[0]);
            var high = int.Parse(range[1]);

            return Enumerable.Range(low, high - low)
                .Select(i => i.ToString())
                .Count(clause)
                .ToString();
        }

        #region Criteria
        private static bool HasDoubleDigit(string str)
        {
            char work = default;
            foreach (var c in str)
            {
                if (c == work)
                    return true;
                work = c;
            }
            return false;
        }

        private static bool DoesntDecrease(string str)
        {
            char work = str[0];
            foreach (var c in str)
            {
                if (work > c)
                    return false;
                work = c;
            }
            return true;
        }

        private static bool IsOnlyDouble(string str)
        {
            char work = default;
            var count = 0;

            foreach (var c in str)
            {
                if (work == c)
                {
                    count++;
                }
                else
                {
                    if (count == 2)
                        return true;

                    work = c;
                    count = 1;
                }
            }
            return count == 2;
        }
        #endregion
    }
}
