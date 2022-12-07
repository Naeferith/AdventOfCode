using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Core.Utils
{
    internal class Tree<T>
    {
        public Tree<T> Parent { get; set; }
        public IEnumerable<Tree<T>> Children { get; set; }

        public Tree()
        {
            Children = Enumerable.Empty<Tree<T>>();
        }

        public Tree(IEnumerable<Tree<T>> children)
        {
            Children = children;

            foreach (var child in children)
            {
                child.Parent = this;
            }
        }

        public Tree(Tree<T> parent, IEnumerable<Tree<T>> children) : this(children)
        {
            Parent = parent;
        }
    }
}
