
namespace AdventOfCode
{
    public interface IDay
    {
        string PuzzleName { get; }
        string Solution1(string[] lines);
        string Solution2(string[] lines);
    }
}
