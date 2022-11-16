namespace AdventOfCode.V2021.Core.Day24
{
    internal sealed class Universe
    {
        public int[] Serial { get; }

        public Universe(int serialSize)
        {
            Serial = new int[serialSize];
        }
    }
}
