using AdventOfCode.Core.Components;
using System.Text.RegularExpressions;

namespace AdventOfCode.V2022.Days
{
    internal partial class Day5 : IDay
    {
        private Stack<char>[] _stacks;

        private IEnumerable<int[]> _moves;

        public int DayNumber => 5;

        public string PuzzleName => "Supply Stacks";

        public string Solution1(string[] lines)
        {
            Initialize(lines);

            foreach (var move in _moves)
            {
                for (int i = 0; i < move[0]; i++)
                {
                    _stacks[move[2] - 1].Push(_stacks[move[1] - 1].Pop());
                }
            }

            return new string(_stacks.Select(s => s.Peek()).ToArray());
        }

        public string Solution2(string[] lines)
        {
            Initialize(lines);

            var work = new Stack<char>();

            foreach (var move in _moves)
            {
                for (int i = 0; i < move[0]; i++)
                {
                    work.Push(_stacks[move[1] - 1].Pop());
                }

                for (int i = 0; i < move[0]; i++)
                {
                    _stacks[move[2] - 1].Push(work.Pop());
                }
            }

            return new string(_stacks.Select(s => s.Peek()).ToArray());
        }

        private void Initialize(string[] lines)
        {
            var setup = lines.TakeWhile(s => !string.IsNullOrEmpty(s)).ToArray();
            _moves = lines
                .Skip(setup.Length + 1)
                .Select(m => m.Replace("move", string.Empty))
                .Select(m => m.Replace("from", string.Empty))
                .Select(m => m.Replace("to", string.Empty).Trim())
                .Select(m => MultiSpace().Split(m).Select(int.Parse).ToArray());

            // Init
            _stacks = new Stack<char>[int.Parse("" + setup[^1].TrimEnd().Last())];
            for (int i = 0; i < _stacks.Length; i++)
            {
                _stacks[i] = new Stack<char>();
            }

            var stacks = setup
                .SkipLast(1)
                .Select(GetCrates);

            foreach (var line in stacks.Reverse())
            {
                for (int i = 0; i < _stacks.Length; i++)
                {
                    if (line[i] == ' ') continue;
                    _stacks[i].Push(line[i]);
                }
            }
        }

        private static List<char> GetCrates(string line)
        {
            var ret = new List<char>();
            for (int i = 1; i < line.Length; i += 4)
            {
                ret.Add(line[i]);
            }
            return ret;
        }

        [GeneratedRegex("\\s{2,}")]
        private static partial Regex MultiSpace();
    }
}
