namespace AdventOfCode.Core.AoC
{
    public interface IAoC
    {
        int Year { get; }
        string GetPuzzle(int day, int version);
    }
}
