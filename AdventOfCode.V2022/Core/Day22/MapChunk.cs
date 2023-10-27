using Nae.Utils.Components;

namespace AdventOfCode.V2022.Core.Day22
{
    internal class MapChunk : CursorMatrix<char>
    {
        public MapChunk(int maxX, int maxY, int xOffset, int yOffset)
            : base(new MatrixMovingStrategy()
            {
                BypassCrossable = false,
                Behavior = MatrixEdgeBehavior.Stopping
            }, new int[] { maxX, maxY }, new int[] { xOffset, yOffset })
        {
        }

        protected override bool IsCrossable(char element) => element == '.';
    }
}
