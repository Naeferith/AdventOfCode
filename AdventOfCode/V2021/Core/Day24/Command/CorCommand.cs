using System;
using System.Collections.Generic;

namespace AdventOfCode.V2021.Core.Day24.Command
{
    internal abstract class CorCommand
    {
        public abstract string Command { get; }

        public abstract void Execute(Action<int> store, Alu alu, IList<int> args);
    }
}
