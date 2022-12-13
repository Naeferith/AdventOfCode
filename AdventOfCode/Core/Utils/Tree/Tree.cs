using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Core.Utils.Tree
{
    internal class Tree<T> : IEnumerable<Tree<T>>
    {
        public Tree<T> Parent { get; set; }
        public T Value { get; set; }

        private readonly List<Tree<T>> _children;
        public IReadOnlyList<Tree<T>> Children => _children;

        public Tree() : this(parent: null)
        {
        }

        public Tree(T value)
        {
            Value = value;
        }

        public Tree(IEnumerable<T> children)
        {
            _children = new();
            AddRange(children);

        }

        public Tree(Tree<T> parent, IEnumerable<T> children) : this(children)
        {
            Parent = parent;
        }

        public Tree(Tree<T> parent) : this(Enumerable.Empty<T>())
        {
            Parent = parent;
        }

        public void AddRange(IEnumerable<T> children)
        {
            foreach (var child in children)
            {
                Add(child);
            }
        }

        public void Add(T child)
        {
            var c = new Tree<T>(child)
            {
                Parent = this
            };
            _children.Add(c);
        }

        public IEnumerator<Tree<T>> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Children).GetEnumerator();
        }
    }
}
