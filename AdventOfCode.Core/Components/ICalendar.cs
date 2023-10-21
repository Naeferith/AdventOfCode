namespace AdventOfCode.Core.Components
{
    /// <summary>
    /// Contract for a whole Advent of Code calendar.
    /// </summary>
    public interface ICalendar : IEnumerable<IDay>
    {
        /// <summary>
        /// Prints the already defined days solutions in this calendar.
        /// </summary>
        /// <param name="writer">Text writer to write the solutions</param>
        /// <param name="accessor">Input accessor</param>
        void PrintCalendar(TextWriter writer, IInputAccessor accessor);

        /// <summary>
        /// Prints one specific day's solutions defined in this calendar.
        /// </summary>
        /// <param name="writer">Text writer to write the solutions</param>
        /// <param name="accessor">Input accessor</param>
        void PrintDay(TextWriter writer, int day, IInputAccessor accessor);
    }
}
