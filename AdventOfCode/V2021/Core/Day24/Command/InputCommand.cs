using System;
using System.Linq.Expressions;

namespace AdventOfCode.V2021.Core.Day24.Command
{
    internal sealed class InputCommand : BinaryCommand
    {
        public InputCommand(MemberExpression store, Expression rExp) : base(store, rExp)
        {
        }

        public override string Command => "inp";

        protected override Func<Expression, Expression, BinaryExpression> Assign => Expression.Assign;
    }
}
