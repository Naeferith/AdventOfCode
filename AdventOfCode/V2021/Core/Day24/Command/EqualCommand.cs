using System;
using System.Linq.Expressions;

namespace AdventOfCode.V2021.Core.Day24.Command
{
    internal class EqualCommand : BinaryCommand
    {
        public EqualCommand(MemberExpression store, Expression rExp) : base(store, rExp)
        {
        }

        public override string Command => "eql";

        protected override Func<Expression, Expression, BinaryExpression> Assign => AssignEqual;

        private BinaryExpression AssignEqual(Expression  l, Expression r)
        {
            return Expression.Assign(l,
                    Expression.Condition(
                        Expression.Equal(l, r),
                        Expression.Constant(1),
                        Expression.Constant(0),
                        typeof(int)));
        }
    }
}
