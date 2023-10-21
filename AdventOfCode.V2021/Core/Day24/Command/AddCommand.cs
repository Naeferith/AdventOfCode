using System.Linq.Expressions;

namespace AdventOfCode.V2021.Core.Day24.Command
{
    internal class AddCommand : BinaryCommand
    {
        public override string Command => "add";

        protected override Func<Expression, Expression, BinaryExpression> Assign => Expression.AddAssign;

        public AddCommand(MemberExpression store, Expression rExp) : base(store, rExp)
        {
        }
    }
}
