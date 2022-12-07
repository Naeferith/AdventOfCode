using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.V2022.Core.Day7
{
    internal sealed class SystemDirectory : ISystemObject
    {
        private const string ROOT = "/";

        private readonly SystemDirectory _parent;
        public SystemDirectory Parent => _parent ?? this;

        public IEnumerable<ISystemObject> Children { get; set; }

        public int Size => Children.Sum(o => o.Size);

        public string Name { get; }

        public SystemDirectory() : this(ROOT, null)
        {
        }

        public SystemDirectory(string name, SystemDirectory parent)
        {
            Name = name;
            Children = Enumerable.Empty<ISystemObject>();
            _parent = parent;
        }

        public ISystemObject FindObject(string dir)
        {
            if (dir == ROOT)
                return GetRoot(this);

            return Children.Single(o => o.Name == dir);
        }

        private static SystemDirectory GetRoot(SystemDirectory dir)
        {
            return dir.Name == ROOT ? dir : GetRoot(dir.Parent);
        }

        public IEnumerable<int> GetRemovableDirectories()
        {
            foreach (var cDir in Children.OfType<SystemDirectory>())
            {
                foreach (var size in cDir.GetRemovableDirectories())
                {
                    yield return size;
                }
            }

            if (Size < 100000)
                yield return Size;
        }

        public IEnumerable<int> GetFolders()
        {
            foreach (var cDir in Children.OfType<SystemDirectory>())
            {
                foreach (var dir in cDir.GetFolders())
                {
                    yield return dir;
                }
            }

            yield return Size;
        }
    }
}
