using System.Linq.Expressions;

namespace AdventOfCode.V2021.Core.Day24.Command
{
    internal class DivideCommand : BinaryCommand
    {
        public DivideCommand(MemberExpression store, Expression rExp) : base(store, rExp)
        {
        }

        public override string Command => "div";

        protected override Func<Expression, Expression, BinaryExpression> Assign => Expression.DivideAssign;
    }
}
