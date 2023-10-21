using AdventOfCode.V2021.Core.Day24.Algebra;
using AdventOfCode.V2021.Core.Day24.Command;
using System.Linq.Expressions;

namespace AdventOfCode.V2021.Core.Day24
{
    internal sealed class Alu
    {
        private static readonly ParameterExpression Serial = Expression.Parameter(typeof(int[]), "serial");

        public readonly AluData _data;

        private readonly List<Universe> _validUniverses;

        public List<List<ICommand>> Chunks { get; }

        private readonly Func<int[], int> _algorithm;

        public bool IsValidSerial => _data.Z == 0;

        private int _index;

        public Alu(string[][] instructions)
        {
            var insts = new List<ICommand>(instructions.Length);
            _validUniverses = new List<Universe>();
            _data = new AluData();
            Chunks = new List<List<ICommand>>();

            // Convert
            for (int i = 0; i < instructions.Length; i++)
            {
                var command = instructions[i][0];
                var lExp = instructions[i][1];
                var rExp = instructions[i].Length < 3 ? string.Empty : instructions[i][2];

                if (command == "inp" && insts.Any())
                {
                    Chunks.Add(insts);
                    insts = new List<ICommand>();
                }

                insts.Add(GetCommand(command, lExp, rExp));
            }

            Chunks.Add(insts);

            _data.Save();

            // Compute
            var commands = Chunks.SelectMany(c => c).Select(c => c.Compute()).ToArray();
            var block = Expression.Block(commands);

            // Rewrite
            Rewrite(new Universe(Chunks.Count, commands));

            var b2 = Expression.Block(commands);

            // Compile
            var baseAlgo = Expression.Lambda<Action<AluData, int[]>>(block, new ParameterExpression[] { AluData.Parameter, Serial }).Compile();
            var zExpression = Expression.Lambda<Func<int[], int>>(((BinaryExpression)b2.Result).Right, Serial).Compile();

            _algorithm = zExpression;
        }

        private void Rewrite(Universe universe)
        {
            var @continue = true;
            for (int i = 0; @continue && i < universe.Instructions.Length; i++)
            {
                switch (((MemberExpression)universe.Instructions[i].Left).Member.Name)
                {
                    case nameof(AluData.W):
                        @continue = RewriteExpression(ref universe.wExp, universe, i);
                        break;
                    case nameof(AluData.X):
                        @continue = RewriteExpression(ref universe.xExp, universe, i);
                        break;
                    case nameof(AluData.Y):
                        @continue = RewriteExpression(ref universe.yExp, universe, i);
                        break;
                    case nameof(AluData.Z):
                        @continue = RewriteExpression(ref universe.zExp, universe, i);
                        break;
                }
            }

            if (@continue)
                _validUniverses.Add(universe);
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

        private bool RewriteExpression(ref Expression lExp, Universe universe, int commandIndex)
        {
            Expression rExp = universe.Instructions[commandIndex].Right;
            Expression final = universe.Instructions[commandIndex].Right;

            if (rExp is MemberExpression r)
            {
                rExp = universe.GetExpressionOfMember(r.Member.Name);
            }

            switch (universe.Instructions[commandIndex].NodeType)
            {
                case ExpressionType.Assign:
                    {
                        final = ExpressionRewriter.AssignRewrite(lExp, rExp, universe);

                        if (final is ConditionalExpression condition)
                        {
                            var test = (BinaryExpression)condition.Test;

                            var t_lExp = universe.GetExpressionOfMember(((MemberExpression)test.Left).Member.Name);
                            var t_rExp = universe.GetExpressionOfMember(((MemberExpression)test.Right).Member.Name);

                            if (t_lExp.IsAssignable())
                            {
                                // Assuming Right is ArrayIndex
                                var index = (int)((ConstantExpression)((BinaryExpression)t_rExp).Right).Value;
                                var value = (int)((ConstantExpression)t_lExp).Value;
                                Rewrite(new Universe(universe, index, value));
                                Rewrite(new Universe(universe, index, -value));
                            }
                            else
                                return false;
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
                universe.Instructions[commandIndex] = Expression.Assign(universe.Instructions[commandIndex].Left, final);

            lExp = final;
            return true;
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
