namespace AdventOfCode.Core.Components
{
    public interface IDay
    {
        int DayNumber { get; }

        string PuzzleName { get; }

        string Solution1(string[] lines);

        string Solution2(string[] lines);
    }
}
