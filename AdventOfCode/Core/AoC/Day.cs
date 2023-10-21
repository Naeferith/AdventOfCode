namespace AdventOfCode.Core.AoC
{
    internal abstract class Day : IDay
    {
        private string[] _input;

        public abstract string PuzzleName { get; }

        public void SetInput(string[] lines)
        {
            _input = lines;
        }

        public string Solution1()
        {
            return Solution1(_input);
        }

        public string Solution2()
        {
            return Solution2(_input);
        }

        protected abstract string Solution1(string[] lines);

        protected abstract string Solution2(string[] lines);
    }
}
