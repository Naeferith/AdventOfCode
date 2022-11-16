using System;
using System.Linq.Expressions;

namespace AdventOfCode.V2021.Core.Day24.Command
{
    internal abstract class BinaryCommand : ICommand
    {
        public MemberExpression Left { get; }
        protected Expression Right { get; }

        public abstract string Command { get; }

        protected BinaryCommand(MemberExpression lExp, Expression rExp)
        {
            Left = lExp;
            Right = rExp;
        }

        protected abstract Func<Expression, Expression, BinaryExpression> Assign { get; }

        public BinaryExpression Compute() => Assign(Left, Right);
    }
}
