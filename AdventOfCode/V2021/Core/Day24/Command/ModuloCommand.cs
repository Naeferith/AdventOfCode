using System;
using System.Linq.Expressions;

namespace AdventOfCode.V2021.Core.Day24.Command
{
    internal class ModuloCommand : BinaryCommand
    {
        public ModuloCommand(MemberExpression store, Expression rExp) : base(store, rExp)
        {
        }

        public override string Command => "mod";

        protected override Func<Expression, Expression, BinaryExpression> Assign => Expression.ModuloAssign;
    }
}
