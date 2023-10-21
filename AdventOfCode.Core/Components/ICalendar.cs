namespace AdventOfCode.Core.Components
{
    public interface ICalendar
    {
        void PrintCalendar(TextWriter writer, IInputAccessor accessor);

        void PrintDay(TextWriter writer, int day, IInputAccessor accessor);
    }
}
