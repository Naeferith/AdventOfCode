namespace AdventOfCode.Core
{
    public interface IAoC
    {
        int Year { get; }
        string GetPuzzle(int day, int version);
    }
}
