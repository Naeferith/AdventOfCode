using AdventOfCode.Core.Components;

namespace AdventOfCode.V2023.Days.Day9
{
    internal class Day9 : IDay
    {
        public int DayNumber => 9;

        public string PuzzleName => "Mirage Maintenance";

        public string Solution1(string[] lines)
        {
            return GetPrediction(lines, true)
                .Sum()
                .ToString();
        }

        public string Solution2(string[] lines)
        {
            return GetPrediction(lines, false)
                .Sum()
                .ToString();
        }

        private static void ParseInitial(List<LinkedList<int>> rows)
        {
            while (!rows[^1].All(n => n == 0))
            {
                var lastRow = rows[^1];
                var newRow = new LinkedList<int>();
                var curNode = lastRow.First!;

                for (int i = 0; i < lastRow.Count - 1; i++)
                {
                    newRow.AddLast(curNode.Next!.Value - curNode.Value);
                    curNode = curNode.Next!;
                }

                rows.Add(newRow);
            }
        }

        private static IEnumerable<int> GetPrediction(string[] lines, bool forward)
        {
            foreach (var line in lines)
            {
                var nums = line.Split(' ').Select(int.Parse);
                var rows = new List<LinkedList<int>>() { new(nums) };

                ParseInitial(rows);

                rows[^1].AddLast(0);

                for (int i = rows.Count - 2; i >= 0; i--)
                {
                    if (forward)
                    {
                        rows[i].AddLast(rows[i].Last!.Value + rows[i + 1].Last!.Value);
                    }
                    else
                    {
                        rows[i].AddFirst(rows[i].First!.Value - rows[i + 1].First!.Value);
                    }
                }

                yield return forward
                    ? rows[0].Last!.Value
                    : rows[0].First!.Value;
            }
        }
    }
}
