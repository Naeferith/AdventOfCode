using System.Linq.Expressions;

namespace AdventOfCode.V2021.Core.Day24.Algebra
{
    internal static class ExpressionRewriter
    {
        private static bool TryGetMaxValue(this Expression expr, out int max)
        {
            if (expr is ConstantExpression constant)
            {
                max = (int)constant.Value;
                return true;
            }

            if (expr.NodeType == ExpressionType.ArrayIndex) 
            {
                max = 9;
                return true;
            }

            if (expr is BinaryExpression binary)
            {
                switch (binary.NodeType)
                {
                    case ExpressionType.Add:
                        var sum = 0;

                        if (binary.Left.TryGetMaxValue(out var lMax))
                        {
                            sum += lMax;
                        }

                        if (binary.Right.TryGetMaxValue(out var rMax))
                        {
                            sum += rMax;
                        }

                        max = sum;
                        return true;
                    case ExpressionType.Divide:
                        break;
                    case ExpressionType.Modulo:
                        break;
                    case ExpressionType.Multiply:
                        break;
                }
            }

            max = 0;
            return false;
        }

        public static Expression AssignRewrite(Expression lExp, Expression rExp,
            Expression wExp, Expression xExp, Expression yExp, Expression zExp)
        {
            if (rExp is ConditionalExpression condition)
            {
                if (lExp is not ConstantExpression conditionLeft)
                    return rExp;

                BinaryExpression test = (BinaryExpression)condition.Test;
                Expression conditionRight = test.Right;

                if (test.Right is MemberExpression mem)
                {
                    switch (mem.Member.Name)
                    {
                        case nameof(AluData.W):
                            conditionRight = wExp;
                            break;
                        case nameof(AluData.X):
                            conditionRight = xExp;
                            break;
                        case nameof(AluData.Y):
                            conditionRight = yExp;
                            break;
                        case nameof(AluData.Z):
                            conditionRight = zExp;
                            break;
                    }
                }

                var lVal = (int)conditionLeft.Value;

                if (conditionRight.NodeType == ExpressionType.ArrayIndex)
                {
                    if (lVal > 9 || lExp.TryGetMaxValue(out var lMaxValue) && lMaxValue > 9)
                        return condition.IfFalse;
                }
                else if (conditionRight is ConstantExpression constCRight)
                {
                    return ((int)constCRight.Value == lVal) ? condition.IfTrue : condition.IfFalse;
                }
            }

            return rExp;
        }

        public static Expression AddRewrite(Expression lExp, Expression rExp)
        {
            var l = lExp as ConstantExpression;
            var r = rExp as ConstantExpression;

            // 0 + x => x
            if (l != null && (int)l.Value == 0)
                return rExp;

            // x + 0 => x
            if (r != null && (int)r.Value == 0)
                return lExp;

            // x + y => z | x,y E N
            if (l != null && r != null)
                return Expression.Constant((int)l.Value + (int)r.Value);

            return Expression.Add(lExp, rExp);
        }

        public static Expression MultiplyRewrite(Expression lExp, Expression rExp)
        {
            if (lExp is ConstantExpression l)
            {
                // 0 * x => 0
                if ((int)l.Value == 0)
                    return lExp;

                // 1 * x => x
                if ((int)l.Value == 1)
                    return rExp;
            }

            if (rExp is ConstantExpression r)
            {
                // x * 0 => 0
                if ((int)r.Value == 0)
                    return rExp;

                // x * 1 => x
                if ((int)r.Value == 1)
                    return lExp;
            }

            return Expression.Multiply(lExp, rExp);
        }

        public static Expression DivideRewrite(Expression lExp, Expression rExp)
        {
            // x / 1 => x
            if (rExp is ConstantExpression r && (int)r.Value == 1)
            {
                return lExp;
            }

            return Expression.Divide(lExp, rExp);
        }

        public static Expression ModuloRewrite(Expression lExp, Expression rExp)
        {
            var l = lExp as ConstantExpression;
            var r = rExp as ConstantExpression;

            // 0 % x => 0
            if (l != null && (int)l.Value == 0)
            {
                return lExp;
            }

            // x % y => z | x,y E N
            if (l != null && r != null)
            {
                return Expression.Constant((int)l.Value % (int)r.Value);
            }

            if (lExp.TryGetMaxValue(out var lMaxValue) && r != null)
            {
                return Expression.Constant(lMaxValue % (int)r.Value);
            }

            return Expression.Modulo(lExp, rExp);
        }
    }
}
