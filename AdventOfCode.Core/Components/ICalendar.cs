namespace AdventOfCode.Core.Components
{
    public interface ICalendar : IReadOnlyList<IDay>
    {
        void PrintCalendar(TextWriter writer);
    }
}
