
namespace AdventOfCode
{
    public interface IAoC
    {
        int Year { get; }
        string GetPuzzle(int day, int version);
    }
}
