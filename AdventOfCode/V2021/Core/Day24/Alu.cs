using AdventOfCode.V2021.Core.Day24.Algebra;
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
        private readonly List<Universe> _universes;

        public List<List<ICommand>> Chunks { get; }

        private readonly Func<int[], int> _algorithm;

        public bool IsValidSerial => _data.Z == 0;

        private int _index;

        public Alu(string[][] instructions)
        {
            _instructions = new List<ICommand>(instructions.Length);
            _universes = new List<Universe>();
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

            // Universe
            _universes.Add(new Universe(Chunks.Count));

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
                    rExp = GetExpressionOfMember(r.Member.Name, wExp, xExp, yExp, zExp);
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
            var baseAlgo = Expression.Lambda<Action<AluData, int[]>>(block, new ParameterExpression[] { AluData.Parameter, Serial }).Compile();
            var zExpression = Expression.Lambda<Func<int[], int>>(((BinaryExpression)b2.Result).Right, Serial).Compile();

            _algorithm = zExpression;
        }

        public bool Run(int[] serial)
        {
            // Execute
            var z = _algorithm(serial);

            // Validity
            return z == 0;
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

        private void RewriteExpression(ref Expression lExp, Expression rExp, ref BinaryExpression command, 
            Expression wExp, Expression xExp, Expression yExp, Expression zExp)
        {
            Expression final = command.Right;

            switch (command.NodeType)
            {
                case ExpressionType.Assign:
                    {
                        final = ExpressionRewriter.AssignRewrite(lExp, rExp, wExp, xExp, yExp, zExp);

                        if (final is ConditionalExpression condition)
                        {
                            var test = (BinaryExpression)condition.Test;

                            var t_lExp = GetExpressionOfMember(((MemberExpression)test.Left).Member.Name, wExp, xExp, yExp, zExp);
                            var t_rExp = GetExpressionOfMember(((MemberExpression)test.Right).Member.Name, wExp, xExp, yExp, zExp);

                            // Assuming Right is ArrayIndex
                            var uni = new Universe(Chunks.Count);
                            var index = (int)((ConstantExpression)((BinaryExpression)t_rExp).Right).Value;
                            uni.Serial[index] = (int)((ConstantExpression)t_lExp).Value;
                            _universes.Add(uni);

                        }
                    }
                    break;
                case ExpressionType.AddAssign:
                    final = ExpressionRewriter.AddRewrite(lExp, rExp);
                    break;
                case ExpressionType.DivideAssign:
                    final = ExpressionRewriter.DivideRewrite(lExp, rExp);
                    break;
                case ExpressionType.ModuloAssign:
                    final = ExpressionRewriter.ModuloRewrite(lExp, rExp);
                    break;
                case ExpressionType.MultiplyAssign:
                    final = ExpressionRewriter.MultiplyRewrite(lExp, rExp);
                    break;
            }

            if (final != null)
                command = Expression.Assign(command.Left, final);

            lExp = final;
        }

        private static Expression GetExpressionOfMember(string varName, 
            Expression wExp, Expression xExp, Expression yExp, Expression zExp)
        {
            return varName switch
            {
                nameof(AluData.W) => wExp,
                nameof(AluData.X) => xExp,
                nameof(AluData.Y) => yExp,
                nameof(AluData.Z) => zExp,
            };
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
