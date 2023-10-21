using AdventOfCode.Core.Components;
using Nae.Utils.Extensions;
using System.Collections;
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
        private readonly IInputAccessor _input;

        private int Year { get; }

        public int Count => _days.Length;

        public IDay this[int index] => _days[index - 1];

        public Calendar(
            IYearAccessor yearAccessor,
            IInputAccessor inputAccessor,
            IEnumerable<IDay> days)
        {
            Year = yearAccessor.Year;
            _days = new IDay[25];
            _input = inputAccessor;

            foreach (var day in days)
            {
                day.SetInput(_input.GetInputLines(day.DayNumber));
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

        public void PrintCalendar(TextWriter writer)
        {
            var dblLine = string.Concat(Enumerable.Repeat('═', MaxLineSize));

            var strBuilder = new StringBuilder();

            strBuilder.AppendLine($"╔{dblLine}╗");
            strBuilder.AppendLine($"║{$"Advent Of Code {Year}".PadCenter(MaxLineSize)}║");
            strBuilder.AppendLine($"╠{dblLine}╣");

            foreach (var day in this)
            {
                strBuilder.AppendLine(string.Format(LineFormat,
                    day.GetType().Name.PadCenter(LabelColSize),
                    day.PuzzleName.PadCenter(ValueColSize)));

                strBuilder.AppendLine(string.Format("║{0}┼─{1}║",
                    string.Concat(Enumerable.Repeat('─', LabelColSize)),
                    string.Concat(Enumerable.Repeat('─', ValueColSize))));

                AppendSolutions(strBuilder, day.Solution1, day.Solution2);
                strBuilder.AppendLine($"╠{dblLine}╣");
            }

            writer.WriteLine(strBuilder.ToString());
        }

        private static void AppendSolutions(StringBuilder builder, params Func<string>[] solutions)
        {
            for (int i = 0; i < solutions.Length; i++)
            {
                var result = "<Unresolved Yet>";

                try
                {
                    result = solutions[i]();
                }
                catch (NotImplementedException)
                {
                    // Consume
                }
                finally
                {
                    builder.AppendLine(string.Format(LineFormat, $"Solution {i}".PadCenter(LabelColSize), result.PadRight(ValueColSize)));
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
