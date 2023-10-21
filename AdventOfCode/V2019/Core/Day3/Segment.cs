using Nae.Utils.Structs;

namespace AdventOfCode.V2019.Core.Day3
{
    internal class Segment
    {
        public Point Start { get; init; }
        public Point End { get; init; }

        public static bool TryIntersect(Segment value1, Segment value2, out Point? intersect)
        {
            intersect = null;
            return false;
        }
    }
}
