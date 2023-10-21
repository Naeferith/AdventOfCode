using System.Collections.Generic;
using System.IO;

namespace AdventOfCode.Core.AoC
{
    public interface IAoC : IReadOnlyList<IDay>
    {
        int Year { get; }

        void PrintCalendar(TextWriter writer);
    }
}
