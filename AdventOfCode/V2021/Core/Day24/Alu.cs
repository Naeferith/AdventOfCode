using AdventOfCode.V2021.Core.Day24.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AdventOfCode.V2021.Core.Day24
{
    internal sealed class Alu
    {
        private static readonly ParameterExpression Serial = Expression.Parameter(typeof(int[]), "serial");

        public readonly AluData _data;

        private readonly List<ICommand> _instructions;

        public List<List<ICommand>> Chunks { get; }

        private readonly Action<AluData, int[]> _algorithm;

        public bool IsValidSerial => _data.Z == 0;

        private int _index;

        public Alu(string[][] instructions)
        {
            _instructions = new List<ICommand>(instructions.Length);
            _data = new AluData();
            Chunks = new List<List<ICommand>>();

            // Convert
            for (int i = 0; i < instructions.Length; i++)
            {
                var command = instructions[i][0];
                var lExp = instructions[i][1];
                var rExp = instructions[i].Length < 3 ? string.Empty : instructions[i][2];

                if (command == "inp" && _instructions.Any())
                {
                    Chunks.Add(_instructions);
                    _instructions = new List<ICommand>();
                }

                _instructions.Add(GetCommand(command, lExp, rExp));
            }

            Chunks.Add(_instructions);

            _data.Save();

            // Compute
            var commands = Chunks.SelectMany(c => c).Select(c => c.Compute()).ToArray();
            var block = Expression.Block(commands);

            // Rewrite
            Expression wExp = Expression.Constant(0);
            Expression xExp = Expression.Constant(0);
            Expression yExp = Expression.Constant(0);
            Expression zExp = Expression.Constant(0);

            for (int i = 0; i < commands.Length; i++)
            {
                Expression rExp = commands[i].Right;

                if (rExp is MemberExpression r)
                {
                    switch (r.Member.Name)
                    {
                        case nameof(AluData.W):
                            rExp = wExp;
                            break;
                        case nameof(AluData.X):
                            rExp = xExp;
                            break;
                        case nameof(AluData.Y):
                            rExp = yExp;
                            break;
                        case nameof(AluData.Z):
                            rExp = zExp;
                            break;
                    }
                }

                switch (((MemberExpression)commands[i].Left).Member.Name)
                {
                    case nameof(AluData.W):
                        RewriteExpression(ref wExp, rExp, ref commands[i], wExp, xExp, yExp, zExp);
                        break;
                    case nameof(AluData.X):
                        RewriteExpression(ref xExp, rExp, ref commands[i], wExp, xExp, yExp, zExp);
                        break;
                    case nameof(AluData.Y):
                        RewriteExpression(ref yExp, rExp, ref commands[i], wExp, xExp, yExp, zExp);
                        break;
                    case nameof(AluData.Z):
                        RewriteExpression(ref zExp, rExp, ref commands[i], wExp, xExp, yExp, zExp);
                        break;
                }
            }
            var b2 = Expression.Block(commands);

            // Compile
            _algorithm = Expression.Lambda<Action<AluData, int[]>>(block, new ParameterExpression[] { AluData.Parameter, Serial }).Compile();
        }

        public bool Run(int[] serial)
        {
            // Reset
            _data.Reset();

            // Execute
            _algorithm(_data, serial);

            // Validity
            return IsValidSerial;
        }

        private ICommand GetCommand(string commandName, string storedVariable, string rightExpression)
        {
            var store = AluData.Member(storedVariable);

            Expression rExp;

            if (commandName == "inp")
            {
                rExp = Expression.ArrayIndex(Serial, Expression.Constant(_index));
                _index++;
            }
            else
            {
                if (int.TryParse(rightExpression, out var intVal))
                    rExp = Expression.Constant(intVal);
                else
                    rExp = AluData.Member(rightExpression);
            }

            return commandName switch
            {
                "inp" => new InputCommand(store, rExp),
                "add" => new AddCommand(store, rExp),
                "mul" => new MultiplyCommand(store, rExp),
                "div" => new DivideCommand(store, rExp),
                "mod" => new ModuloCommand(store, rExp),
                "eql" => new EqualCommand(store, rExp),
                _ => default,
            };
        }

        private static void RewriteExpression(ref Expression lExp, Expression rExp, ref BinaryExpression command, 
            Expression wExp, Expression xExp, Expression yExp, Expression zExp)
        {
            Expression final = command.Right;

            switch (command.NodeType)
            {
                case ExpressionType.Assign:
                    final = AssignRewrite(lExp, rExp, wExp, xExp, yExp, zExp);
                    break;
                case ExpressionType.AddAssign:
                    final = AddRewrite(lExp, rExp);
                    break;
                case ExpressionType.DivideAssign:
                    final = DivideRewrite(lExp, rExp);
                    break;
                case ExpressionType.ModuloAssign:
                    final = ModuloRewrite(lExp, rExp);
                    break;
                case ExpressionType.MultiplyAssign:
                    final = MultiplyRewrite(lExp, rExp);
                    break;
            }

            if (final != null)
                command = Expression.Assign(command.Left, final);

            lExp = final;
        }

        private static Expression AssignRewrite(Expression lExp, Expression rExp,
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
                    if (lVal > 9)
                        return condition.IfFalse;
                }
                else if (conditionRight is ConstantExpression constCRight)
                {
                    return ((int)constCRight.Value == lVal) ? condition.IfTrue : condition.IfFalse;
                }
            }

            return rExp;
        }

        private static Expression AddRewrite(Expression lExp, Expression rExp)
        {
            var l = lExp as ConstantExpression;
            var r = rExp as ConstantExpression;

            if (l != null && (int)l.Value == 0)
                return rExp;

            if (r != null && (int)r.Value == 0)
                return lExp;

            if (l != null && r != null)
                return Expression.Constant((int)l.Value + (int)r.Value);

            return Expression.Add(lExp, rExp);
        }

        private static Expression MultiplyRewrite(Expression lExp, Expression rExp)
        {
            if (lExp is ConstantExpression l)
            {
                if ((int)l.Value == 0)
                    return lExp;
                if ((int)l.Value == 1)
                    return rExp;
            }

            if (rExp is ConstantExpression r)
            {
                if ((int)r.Value == 0)
                    return rExp;
                if ((int)r.Value == 1)
                    return lExp;
            }

            return rExp;
        }

        private static Expression DivideRewrite(Expression lExp, Expression rExp)
        {
            if (rExp is ConstantExpression r && (int)r.Value == 1)
            {
                return lExp;
            }

            return rExp;
        }

        private static Expression ModuloRewrite(Expression lExp, Expression rExp)
        {
            if (lExp is ConstantExpression l && (int)l.Value == 0)
            {
                return lExp;
            }

            return rExp;
        }
    }

    internal sealed class AluData
    {
        private int _w;
        private int _x;
        private int _y;
        private int _z;

        public int W { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Func<int> Get(string varName)
        {
            return varName switch
            {
                "w" => () => W,
                "x" => () => X,
                "y" => () => Y,
                "z" => () => Z,
                _ => default,
            };
        }

        public Action<int> Set(string varName)
        {
            return varName switch
            {
                "w" => i => W = i,
                "x" => i => X = i,
                "y" => i => Y = i,
                "z" => i => Z = i,
                _ => null,
            };
        }

        public static readonly ParameterExpression Parameter = Expression.Parameter(typeof(AluData), "data");

        private static readonly MemberExpression ExpressionW = Expression.Property(Parameter, nameof(W));
        private static readonly MemberExpression ExpressionX = Expression.Property(Parameter, nameof(X));
        private static readonly MemberExpression ExpressionY = Expression.Property(Parameter, nameof(Y));
        private static readonly MemberExpression ExpressionZ = Expression.Property(Parameter, nameof(Z));

        public static MemberExpression Member(string propertyName)
        {
            return propertyName switch
            {
                "w" => ExpressionW,
                "x" => ExpressionX,
                "y" => ExpressionY,
                "z" => ExpressionZ,
                _ => null,
            };
        }

        public void Save()
        {
            _w = W;
            _x = X;
            _y = Y;
            _z = Z;
        }

        public void Reset()
        {
            W = _w;
            X = _x;
            Y = _y;
            Z = _z;
        }
    }
}
