using System.Linq.Expressions;

namespace AdventOfCode.V2022.Core.Day21
{
    internal class MonkeyVisitor : ExpressionVisitor
    {
        private bool Solution2 { get; }

        private IDictionary<string, Expression> Monkeys { get; }

        public MonkeyVisitor(IDictionary<string, Expression> monkeys, bool sol2)
        {
            Monkeys = monkeys;
            Solution2 = sol2;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return !Solution2 || node.Name != "humn"
                ? Visit(Monkeys[node.Name!])
                : base.VisitParameter(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            var exprReduced = node.Update(Visit(node.Left), node.Conversion, Visit(node.Right));

            if (exprReduced is not BinaryExpression workingExpr)
            {
                return exprReduced;
            }

            if (workingExpr.NodeType == ExpressionType.Equal)
            {
                do
                {
                    if (workingExpr.Left is not BinaryExpression binary)
                    {
                        return workingExpr;
                    }

                    workingExpr = ReverseExpression(binary, workingExpr.Right);
                } while (true);
            }

            return workingExpr;
        }

        private static bool IsNumberOnly(Expression ex)
        {
            if (ex is ParameterExpression)
            {
                return false;
            }

            if (ex is ConstantExpression)
            {
                return true;
            }

            if (ex is BinaryExpression binary)
            {
                return IsNumberOnly(binary.Left) && IsNumberOnly(binary.Right);
            }

            return false;
        }

        private static BinaryExpression ReverseExpression(BinaryExpression binary, Expression z)
        {
            Expression x = binary.Left;
            Expression y = binary.Right;
            Expression right = null!;

            // Reversing is tricky for non commutative operations
            if (binary.NodeType == ExpressionType.Divide)
            {
                right = !IsNumberOnly(x)
                    ? Expression.Multiply(z, y)
                    : Expression.Divide(x, z);
            }
            else if (binary.NodeType == ExpressionType.Subtract)
            {
                right = !IsNumberOnly(x)
                    ? Expression.Add(z, y)
                    : Expression.Subtract(x, z);
            }

            if (!IsNumberOnly(y))
            {
                x = binary.Right;
                y = binary.Left;
            }

            right = binary.NodeType switch
            {
                ExpressionType.Add => Expression.Subtract(z, y),
                ExpressionType.Multiply => Expression.Divide(z, y),
                _ => right
            };

            return Expression.Equal(x, right);
        }
    }
}
