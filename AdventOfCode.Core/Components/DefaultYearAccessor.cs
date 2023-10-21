namespace AdventOfCode.Core.Components
{
    public class DefaultYearAccessor : IYearAccessor
    {
        public int Year { get; }

        public DefaultYearAccessor(int year)
        {
            Year = year;
        }
    }
}
