namespace AdventOfCode.Core.Components
{
    public interface IDay
    {
        int DayNumber { get; }

        string PuzzleName { get; }

        void SetInput(string[] lines);

        string Solution1();

        string Solution2();
    }
}
