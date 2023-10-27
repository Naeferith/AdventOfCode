using AdventOfCode.Core.Components;

namespace AdventOfCode.V2022.Days
{
    internal class Day20 : IDay
    {
        private readonly LinkedList<long> _numbers;
        private readonly IList<LinkedListNode<long>> _nodes;

        public int DayNumber => 20;

        public string PuzzleName => "Grove Positioning System";

        public Day20()
        {
            _numbers = new();
            _nodes = new List<LinkedListNode<long>>();
        }

        public string Solution1(string[] lines)
        {
            return Solution(lines.Select(long.Parse), 1);
        }

        public string Solution2(string[] lines)
        {
            return Solution(lines.Select(l => long.Parse(l) * 811589153), 10);
        }

        private string Solution(IEnumerable<long> nums, int cycles)
        {
            foreach (var num in nums)
            {
                _nodes.Add(_numbers.AddLast(num));
            }

            for (int i = 0; i < cycles; i++)
            {
                Compute();
            }

            var list = _numbers.ToList();
            var zeroIdx = list.IndexOf(0);

            return new[] { 1_000, 2_000, 3_000 }
                .Select(i => (i + zeroIdx) % list.Count)
                .Sum(i => list[i])
                .ToString();
        }

        private void Compute()
        {
            for (int i = 0; i < _nodes.Count; i++)
            {
                var delta = _nodes[i].Value % (_numbers.Count - 1);
                var workNode = _nodes[i];
                LinkedListNode<long> newNode;

                if (delta < 0)
                {
                    for (long j = delta; j < 0; j++)
                    {
                        workNode = workNode!.Previous ?? _numbers.Last;
                    }

                    newNode = _numbers.AddBefore(workNode!, _nodes[i].Value);
                }
                else
                {
                    for (int j = 0; j < delta; j++)
                    {
                        workNode = workNode!.Next ?? _numbers.First;
                    }
                    newNode = _numbers.AddAfter(workNode!, _nodes[i].Value);
                }

                _numbers.Remove(_nodes[i]);
                _nodes[i] = newNode;
            }
        }
    }
}
