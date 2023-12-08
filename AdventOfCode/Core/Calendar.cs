using AdventOfCode.Core.Components;
using AdventOfCode.Core.Extensions;
using System.Collections;
using System.Diagnostics;
using System.Text;

namespace AdventOfCode.Core
{
    public class Calendar : ICalendar
    {
        private const string LineFormat = "║{0}│ {1}║";
        private const int OutputSize = 50;
        private const int MaxLineSize = OutputSize - 2;
        private const int LabelColSize = 12;
        private const int ValueColSize = MaxLineSize - 2 - LabelColSize;

        private readonly IDay[] _days;

        private int Year { get; }

        public Calendar(
            IYearAccessor yearAccessor,
            IEnumerable<IDay> days)
        {
            Year = yearAccessor.Year;
            _days = new IDay[25];

            foreach (var day in days)
            {
                _days[day.DayNumber - 1] = day;
            }
        }

        public IEnumerator<IDay> GetEnumerator()
        {
            return new CalendarEnumerator(_days);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _days.GetEnumerator();
        }

        public void PrintCalendar(TextWriter writer, IInputAccessor accessor)
        {
            PrintCalendar(writer, accessor, d => true);
        }

        public void PrintDay(TextWriter writer, int day, IInputAccessor accessor)
        {
            PrintCalendar(writer, accessor, d => d.DayNumber == day);
        }

        private void PrintCalendar(TextWriter writer, IInputAccessor accessor, Func<IDay, bool> filter)
        {
            var dblLine = string.Concat(Enumerable.Repeat('═', MaxLineSize));

            var strBuilder = new StringBuilder();

            strBuilder.AppendLine($"╔{dblLine}╗");
            strBuilder.AppendLine($"║{$"Advent Of Code {Year}".PadCenter(MaxLineSize)}║");
            strBuilder.AppendLine($"╠{dblLine}╣");

            foreach (var day in this.Where(filter))
            {
                strBuilder.AppendLine(string.Format(LineFormat,
                    $"Day {day.DayNumber}".PadCenter(LabelColSize),
                    day.PuzzleName.PadCenter(ValueColSize)));

                strBuilder.AppendLine(string.Format("║{0}┼─{1}║",
                    string.Concat(Enumerable.Repeat('─', LabelColSize)),
                    string.Concat(Enumerable.Repeat('─', ValueColSize))));

                AppendSolutions(day.DayNumber, strBuilder, accessor, day.Solution1, day.Solution2);
                strBuilder.AppendLine($"╠{dblLine}╣");
            }

            strBuilder.AppendLine($"╚{dblLine}╝");
            writer.WriteLine(strBuilder.ToString());
        }

        private static void AppendSolutions(int day, StringBuilder builder, IInputAccessor accessor, params Func<string[], string>[] solutions)
        {
            for (int i = 0; i < solutions.Length; i++)
            {
                var result = "<Unresolved Yet>";

                try
                {
                    var lines = accessor.GetInputLines(day);

                    var sw = new Stopwatch();
                    sw.Start();

                    result = solutions[i](lines);

                    sw.Stop();

                    result += $" (ms: {sw.Elapsed.TotalMilliseconds})";
                }
                catch (NotImplementedException)
                {
                    // Consume
                }
                finally
                {
                    builder.AppendLine(string.Format(LineFormat, $"Solution {i + 1}".PadCenter(LabelColSize), result.PadRight(ValueColSize)));
                }
            }
        }

        private sealed class CalendarEnumerator : IEnumerator<IDay>
        {
            private readonly IDay[] _days;
            private int _index;

            public CalendarEnumerator(IDay[] days)
            {
                _days = days;
                Reset();
            }

            public IDay Current => _days[_index];

            object IEnumerator.Current => _days[_index];

            public void Dispose()
            {
                // Nothing
            }

            public bool MoveNext()
            {
                while (++_index < _days.Length)
                {
                    if (_days[_index] is not null)
                    {
                        return true;
                    }
                }

                return false;
            }

            public void Reset()
            {
                _index = -1;
            }
        }
    }
}
