using AdventOfCode.Core.Components;
using AdventOfCode.V2022.Core.Day21;
using System.Linq.Expressions;

namespace AdventOfCode.V2022.Days
{
    internal class Day21 : IDay
    {
        private const string HUMN_NAME = "humn";
        private const string ROOT_NAME = "root";

        private readonly IDictionary<string, Expression> _operationMonkeys;
        private readonly ParameterExpression _humnExpr;

        public int DayNumber => 21;

        public string PuzzleName => "Monkey Math";

        public Day21()
        {
            _operationMonkeys = new Dictionary<string, Expression>();
            _humnExpr = Expression.Parameter(typeof(long), HUMN_NAME);
        }

        public string Solution1(string[] lines)
        {
            var expr = Solution(lines, false);
            return Expression.Lambda<Func<long>>(expr).Compile()().ToString();
        }

        public string Solution2(string[] lines)
        {
            var expr = Solution(lines, true);
            return Expression.Lambda<Func<long>>(((BinaryExpression)expr).Right).Compile()().ToString();
        }

        private Expression Solution(string[] lines, bool solution2)
        {
            Initialize(lines, solution2);

            var visitor = new MonkeyVisitor(_operationMonkeys, solution2);
            return visitor.Visit(_operationMonkeys[ROOT_NAME]);
        }

        private void Initialize(string[] lines, bool sol2)
        {
            var monkeys = lines
                .Select(l =>
                {
                    var args = l.Split(": ");
                    return (args[0], args[1].Split(' '));
                });

            var lookup = monkeys.ToLookup(m => m.Item2.Length);

            foreach (var monkey in lookup[1])
            {
                _operationMonkeys[monkey.Item1] = Expression.Constant(long.Parse(monkey.Item2[0]));
            }

            foreach (var monkey in lookup[3])
            {
                if (sol2)
                {
                    if (monkey.Item1 == ROOT_NAME)
                    {
                        monkey.Item2[1] = "=";
                    }
                    else if (monkey.Item1 == HUMN_NAME)
                    {
                        _operationMonkeys[monkey.Item1] = _humnExpr;
                        continue;
                    }
                }

                Func<Expression, Expression, BinaryExpression> op = monkey.Item2[1] switch
                {
                    "+" => Expression.Add,
                    "-" => Expression.Subtract,
                    "*" => Expression.Multiply,
                    "/" => Expression.Divide,
                    "=" => Expression.Equal,
                    _ => throw new NotImplementedException()
                };

                _operationMonkeys[monkey.Item1] = op(
                    Expression.Parameter(typeof(long), monkey.Item2[0]),
                    Expression.Parameter(typeof(long), monkey.Item2[2]));
            }
        }
    }
}
