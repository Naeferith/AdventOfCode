using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AdventOfCode.Core.AoC
{
    public class AoC : IAoC
    {
        public int Year { get; }

        public int Count => _days.Length;

        public IDay this[int index] => _days[index - 1];

        private readonly IDay[] _days;

        private AoC(int year)
        {
            Year = year;
            _days = new IDay[25];

            var days = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass
                    && t.IsAssignableTo(typeof(IDay))
                    && (t.Namespace?.Equals($"{nameof(AdventOfCode)}.V{Year}.Days") ?? false))
                .Select(t => new { type = t, day = int.Parse(string.Concat(t.Name.SkipWhile(char.IsLetter))) });

            foreach (var day in days)
            {
                _days[day.day - 1] = Activator.CreateInstance(day.type) as IDay;
                _days[day.day - 1].SetInput(File.ReadAllLines(Path(day.day)));
            }
        }

        private string Path(int day) => $@"./V{Year}/Input/day{day}.txt";

        public static IAoC GetCalendar(int year) => new AoC(year);

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
            writer.WriteLine($"-- Advent Of Code {Year} --");

            foreach (var day in this)
            {
                writer.WriteLine();
                writer.WriteLine($" --- {day.GetType().Name} : {day.PuzzleName}");

                var s1 = "<Unresolved Yet>";
                var s2 = "<Unresolved Yet>";

                try
                {
                    s1 = day.Solution1();
                    s2 = day.Solution2();
                }
                catch (NotImplementedException)
                {
                    // Consume
                }

                writer.WriteLine($"Solution 1 : {s1}");
                writer.WriteLine($"Solution 2 : {s2}");
            }
        }

        private sealed class CalendarEnumerator : IEnumerator<IDay>
        {
            private readonly IDay[] _days;
            private int _index;

            public CalendarEnumerator(IDay[] days)
            {
                _days = days;
                _index = 0;
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
                _index = 0;
            }
        }
    }
}
