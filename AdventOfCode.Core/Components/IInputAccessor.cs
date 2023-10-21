namespace AdventOfCode.Core.Components
{
    /// <summary>
    /// Provides access to the input related to a specific day
    /// </summary>
    public interface IInputAccessor
    {
        /// <summary>
        /// Gets the lines of the input to be given to the algorithms.
        /// </summary>
        /// <param name="day">Day number</param>
        /// <returns>The input lines related to the given day number</returns>
        string[] GetInputLines(int day);
    }
}
