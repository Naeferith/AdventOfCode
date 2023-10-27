namespace AdventOfCode.V2022.Core.Day22
{
    internal class Map
    {
        public ICollection<MapChunk> Chunks { get; }

        public Map()
        {
            Chunks = new List<MapChunk>();
        }
    }
}
