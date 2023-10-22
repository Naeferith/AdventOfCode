using Nae.Utils.Structs;

namespace AdventOfCode.V2022.Core.Day17
{
    internal class TetrisBlock
    {
        public Point<long>[] Points { get; set; }

        public TetrisBlock(Point<long> bottomLeft, Point<long>[] points)
        {
            Points = points;

            for (int i = 0; i < Points.Length; i++)
            {
                Points[i] = bottomLeft + Points[i];
            }
        }
    }
}
