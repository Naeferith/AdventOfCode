using AdventOfCode.Core.Components;

namespace AdventOfCode.V2023.Days.Day1
{
    internal class Day1 : IDay
    {
        public int DayNumber => 1;

        public string PuzzleName => "Trebuchet?!";

        public string Solution1(string[] lines)
        {
            return lines
                .Select(l => l.Where(char.IsDigit))
                .Select(l => int.Parse($"{l.First()}{l.Last()}"))
                .Sum()
                .ToString();
        }

        public string Solution2(string[] lines)
        {
            return lines
                .Select(StrToNum)
                .Sum()
                .ToString();
        }

        private static int StrToNum(string s)
        {
            char? n1 = null;
            char? n2 = null;

            for (int i = 1; i < s.Length; i++)
            {
                if (n1.HasValue && n2.HasValue)
                {
                    return int.Parse($"{n1}{n2}");
                }

                n1 ??= TryGetNum(s[..i], false);
                n2 ??= TryGetNum(s[^i..], true);
            }

            return int.Parse($"{n1 ?? n2}{n2 ?? n1}");
        }

        private static char? TryGetNum(string s, bool reverse)
        {
            var newChar = reverse
                ? s[0]
                : s[^1];

            if (char.IsNumber(newChar))
            {
                return newChar;
            }

            Predicate<string> predicate = reverse
                ? s.StartsWith
                : s.EndsWith;

            var dict = new Dictionary<string, char?>
            {
                ["one"] = '1',
                ["two"] = '2',
                ["three"] = '3',
                ["four"] = '4',
                ["five"] = '5',
                ["six"] = '6',
                ["seven"] = '7',
                ["eight"] = '8',
                ["nine"] = '9',
            };

            return dict.FirstOrDefault(kvp => predicate(kvp.Key)).Value;
        }
    }
}
