namespace AdventOfCode.V2022.Core.Day7
{
    internal sealed class SystemFile : ISystemObject
    {
        public int Size { get; }

        public string Name { get; }

        public SystemFile(string name, int size)
        {
            Name = name;
            Size = size;
        }
    }
}
