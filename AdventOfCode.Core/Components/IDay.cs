namespace AdventOfCode.Core.Components
{
    /// <summary>
    /// Contract for an Advent of Code day.
    /// </summary>
    public interface IDay
    {
        /// <summary>
        /// Day number in the calendar.
        /// </summary>
        int DayNumber { get; }

        /// <summary>
        /// Name of the puzzle.
        /// </summary>
        string PuzzleName { get; }

        /// <summary>
        /// Method that resolves the first part of the daily problem.
        /// </summary>
        /// <param name="lines">Input lines</param>
        /// <returns>The solution in string representation</returns>
        string Solution1(string[] lines);

        /// <summary>
        /// Method that resolves the final part of the daily problem.
        /// </summary>
        /// <param name="lines">Input lines</param>
        /// <returns>The solution in string representation</returns>
        string Solution2(string[] lines);
    }
}
