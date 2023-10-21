using System.Linq.Expressions;

namespace AdventOfCode.V2021.Core.Day24.Command
{
    internal interface ICommand
    {
        MemberExpression Left { get; }
        string Command { get; }

        BinaryExpression Compute();
    }
}
