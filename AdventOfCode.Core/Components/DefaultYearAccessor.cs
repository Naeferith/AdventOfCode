namespace AdventOfCode.Core.Components
{
    /// <summary>
    /// Default implementation of <see cref="IYearAccessor"/>
    /// </summary>
    public class DefaultYearAccessor : IYearAccessor
    {
        public int Year { get; }

        public DefaultYearAccessor(int year)
        {
            Year = year;
        }
    }
}
