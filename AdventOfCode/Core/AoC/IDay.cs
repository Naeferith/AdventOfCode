namespace AdventOfCode.Core.AoC
{
    public interface IDay
    {
        string PuzzleName { get; }

        void SetInput(string[] lines);

        string Solution1();

        string Solution2();
    }
}
