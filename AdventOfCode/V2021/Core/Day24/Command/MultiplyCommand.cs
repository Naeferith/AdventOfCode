using System;
using System.Linq.Expressions;

namespace AdventOfCode.V2021.Core.Day24.Command
{
    internal class MultiplyCommand : BinaryCommand
    {
        public override string Command => "mul";

        protected override Func<Expression, Expression, BinaryExpression> Assign => Expression.MultiplyAssign;

        public MultiplyCommand(MemberExpression store, Expression rExp) : base(store, rExp)
        {
        }
    }
}
