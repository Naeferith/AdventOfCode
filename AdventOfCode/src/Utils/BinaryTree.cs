using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utils
{
    public class BinaryNode<T>
    {
        public BinaryNode<T> LeftNode { get; set; }
        public BinaryNode<T> RightNode { get; set; }
        public T Data { get; set; }

        public BinaryNode(T data)
        {
            Data = data;
        }

        public bool IsLeaf => LeftNode == null || RightNode == null;
    }
}
