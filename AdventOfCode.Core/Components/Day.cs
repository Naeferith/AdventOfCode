namespace AdventOfCode.Core.Components
{
    public abstract class Day : IDay
    {
        private string[]? _input;

        public abstract int DayNumber { get; }

        public abstract string PuzzleName { get; }

        public void SetInput(string[] lines)
        {
            _input = lines;
        }

        public string Solution1()
        {
            AssertInputSetted();
            return Solution1(_input!);
        }

        public string Solution2()
        {
            AssertInputSetted();
            return Solution2(_input!);
        }

        private void AssertInputSetted()
        {
            if (_input is null)
            {
                throw new InvalidOperationException("No input setted");
            }
        }

        protected abstract string Solution1(string[] lines);

        protected abstract string Solution2(string[] lines);
    }
}
