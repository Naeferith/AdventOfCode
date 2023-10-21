using AdventOfCode.Core.Components;

namespace AdventOfCode.V2021.Days
{
    internal class Day18 : IDay
    {
        public int DayNumber => 18;

        public string PuzzleName => "Snailfish";

        public string Solution1(string[] lines)
        {
            SnailfishNumber s = null;
            foreach (var line in lines)
            {
                if (s == null)
                    s = Parse(line);
                else
                {
                    var a = Parse(line);
                    s = SnailfishNumber.Addition(s, a);
                }

            }
            return s.Magnitude.ToString();
        }

        public string Solution2(string[] lines)
        {
            throw new System.NotImplementedException();
        }


        private sealed class Context<T>
        {
            public Stack<T> Result = new();
        }

        private interface IExpression<T>
        {
            void Interpret(Context<T> context);
        }

        private interface ISnailfishExpression
        {
            ISnailfishExpression Parent { get; set; }
            ISnailfishExpression Left { get; }
            ISnailfishExpression Right { get; }
            bool IsTerminal { get; }
            int Magnitude { get; }
        }
        private class SnailfishNumber : ISnailfishExpression
        {
            private SnailfishNumber Parent { get; set; }
            public ISnailfishExpression Left { get; private set; }
            public ISnailfishExpression Right { get; private set; }

            public bool IsTerminal => false;

            ISnailfishExpression ISnailfishExpression.Parent
            {
                get => Parent;
                set => Parent = value as SnailfishNumber;
            }

            public override string ToString() => $"[{Left},{Right}]";

            public void AddLeft(ISnailfishExpression left)
            {
                Left = left;
                Left.Parent = this;
            }

            public void AddRight(ISnailfishExpression right)
            {
                Right = right;
                Right.Parent = this;
            }

            public bool Reduce(int depth = 0)
            {
                if (Parent != null && Parent.Left != this && Parent.Right != this)
                    return false;

                if (TryExplode(depth))
                    return true;

                if (depth == 0)
                    return TrySplit();

                return false;
            }

            private bool TryExplode(int depth)
            {
                var op = false;

                if (!Left.IsTerminal)
                    op = ((SnailfishNumber)Left).Reduce(depth + 1);

                if (op)
                    return true;

                if (!Right.IsTerminal)
                    op = ((SnailfishNumber)Right).Reduce(depth + 1);

                if (op)
                    return true;

                if (Left.IsTerminal && Right.IsTerminal && Explode(depth))
                {
                    if (Parent.Left == this)
                        Parent.Left = new SnailfishValue(0)
                        {
                            Parent = Parent
                        };
                    else
                        Parent.Right = new SnailfishValue(0)
                        {
                            Parent = Parent
                        };

                    return true;
                }

                return false;
            }

            private bool TrySplit()
            {
                var s = Search(this);

                if (s == null)
                    return false;

                var l = s.Value / 2;
                var r = s.Value - l;

                var child = s.Parent.Left;

                if (s == child)
                {
                    var n = new SnailfishNumber();
                    var left = new SnailfishValue(l)
                    {
                        Parent = n
                    };
                    var right = new SnailfishValue(r)
                    {
                        Parent = n
                    };

                    n.Left = left;
                    n.Right = right;
                    n.Parent = s.Parent;

                    s.Parent.Left = n;
                }
                else
                {
                    var n = new SnailfishNumber();
                    var left = new SnailfishValue(l)
                    {
                        Parent = n
                    };
                    var right = new SnailfishValue(r)
                    {
                        Parent = n
                    };

                    n.Left = left;
                    n.Right = right;
                    n.Parent = s.Parent;

                    s.Parent.Right = n;
                }

                return true;
            }
            private bool Explode(int depth)
            {
                if (depth < 4)
                    return false;

                //ExplodeSpread(Parent, this, n => n.Left);
                //ExplodeSpread(Parent, this, n => n.Right);

                var regLeft = Parent.FirstRegular(n => n.Left);
                var regRight = Parent.FirstRegular(n => n.Right);

                if (regLeft != null)
                {
                    regLeft.Value += (Left as SnailfishValue).Value;
                }

                if (regRight != null)
                {
                    regRight.Value += (Right as SnailfishValue).Value;
                }

                return true;
            }

            private SnailfishValue FirstRegular(Func<SnailfishNumber, ISnailfishExpression> getChild)
            {
                // Si l'expression n'est par un chiffre regulier, on arrete
                if (Left.IsTerminal && Right.IsTerminal)
                    return null;

                // Recupère le fils dans la direction demandée
                var c = getChild.Invoke(this);
                var other = Left;
                Func<SnailfishNumber, ISnailfishExpression> directionOther = n => n.Left;

                if (c == GetRegular)
                    return GetRegular;

                // Récupère l'autre fils
                if (c == other)
                {
                    other = Right;
                    directionOther = n => n.Right;
                }

                var searchRoot = GetRoot(getChild);

                if (searchRoot == null)
                    return null;

                return Search(searchRoot, directionOther, getChild) as SnailfishValue;
            }

            private SnailfishValue GetRegular
            {
                get
                {
                    if (Left is SnailfishValue l && Right is SnailfishNumber)
                        return l;
                    if (Right is SnailfishValue r && Left is SnailfishNumber)
                        return r;
                    return null;
                }
            }

            public int Magnitude => 3 * Left.Magnitude + 2 * Right.Magnitude;

            private ISnailfishExpression GetRoot(Func<SnailfishNumber, ISnailfishExpression> child)
            {
                // Si on est a la racine -> aucun regulier dans la direction
                if (Parent == null)
                    return null;

                // Si on vient du sens de la direction, on remonte
                var fromDir = child.Invoke(Parent);
                if (fromDir == this)
                    return Parent.GetRoot(child);

                if (fromDir is SnailfishValue)
                    return fromDir;

                var r = GetRegular;

                if (r != null && child.Invoke(this) == r)
                    return r;

                //Divergence
                return (Parent.Left == this) ? Parent.Right : Parent.Left;
            }

            private static bool ExplodeSpread(SnailfishNumber work, SnailfishNumber exploding, Func<SnailfishNumber, ISnailfishExpression> child)
            {
                if (work == null)
                    return false;

                var c = child.Invoke(work);
                var other = work.Left;
                Func<SnailfishNumber, ISnailfishExpression> direction = n => n.Right;

                if (c == other)
                {
                    other = work.Right;
                    direction = n => n.Left;
                }

                if (c is SnailfishValue val)
                {
                    val.Value += ((SnailfishValue)child.Invoke(exploding)).Value;
                    return true;
                }

                var k = ((SnailfishNumber)c).FirstRegular(child);
                if (k != null)
                {
                    k.Value += ((SnailfishValue)direction.Invoke(exploding)).Value;
                    return true;
                }


                return ExplodeSpread(work.Parent, exploding, child);
            }

            public static SnailfishNumber Addition(SnailfishNumber left, SnailfishNumber right)
            {
                var n = new SnailfishNumber()
                {
                    Left = left,
                    Right = right
                };

                left.Parent = n;
                right.Parent = n;

                while (n.Reduce())
                {

                }
                return n;
            }

            private static ISnailfishExpression Search(ISnailfishExpression root, Func<SnailfishNumber, ISnailfishExpression> from, Func<SnailfishNumber, ISnailfishExpression> to)
            {
                if (root is SnailfishValue)
                    return root;
                //return root.Parent.Left.GetType() != root.Parent.Right.GetType() ? root : null;

                return Search(from.Invoke(root as SnailfishNumber), from, to) ?? Search(to.Invoke(root as SnailfishNumber), from, to);
            }

            private SnailfishValue Search(ISnailfishExpression root)
            {
                if (root is SnailfishValue v)
                    return v.Split() ? v : null;

                return Search(root.Left) ?? Search(root.Right);
            }
        }
        private class SnailfishValue : ISnailfishExpression
        {
            public int Value { get; set; }

            public bool IsTerminal => true;

            public SnailfishNumber Parent { get; set; }
            ISnailfishExpression ISnailfishExpression.Parent
            {
                get => Parent;
                set => Parent = value as SnailfishNumber;
            }

            public ISnailfishExpression Left => null;

            public ISnailfishExpression Right => null;

            public int Magnitude => Value;

            public SnailfishValue(int value)
            {
                Value = value;
            }

            public override string ToString() => Value.ToString();

            public bool Split()
            {
                return Value >= 10;
            }
        }

        private abstract class OperatorExpression<T> : IExpression<T>
        {
            public IExpression<T> Left { get; set; }
            public IExpression<T> Right { get; set; }

            public void Interpret(Context<T> context)
            {
                Left.Interpret(context);
                T leftValue = context.Result.Pop();

                Right.Interpret(context);
                T rightValue = context.Result.Pop();

                DoInterpret(context, leftValue, rightValue);
            }

            protected abstract void DoInterpret(Context<T> context, T leftValue, T rightValue);

            public override string ToString() => $"[{Left},{Right}]";
        }
        private class SnailfishAddition : OperatorExpression<SnailfishNumber>
        {
            protected override void DoInterpret(Context<SnailfishNumber> context, SnailfishNumber leftValue, SnailfishNumber rightValue)
            {
                throw new NotImplementedException();
            }
        }

        private static SnailfishNumber Parse(string input)
        {
            var stack = new Stack<SnailfishNumber>();

            var left = true;
            SnailfishNumber pending = null;
            foreach (var c in input)
            {
                if (pending != null)
                {
                    if (c == ',')
                        stack.Peek().AddLeft(pending);
                    else
                        stack.Peek().AddRight(pending);
                    pending = null;
                }

                if (c == '[')
                {
                    stack.Push(new());
                    left = true;
                }
                else if (c >= '0' && c <= '9')
                {
                    if (left)
                        stack.Peek().AddLeft(new SnailfishValue(c - '0'));
                    else
                        stack.Peek().AddRight(new SnailfishValue(c - '0'));
                }
                else if (c == ',')
                    left = false;
                else if (c == ']')
                {
                    pending = stack.Pop();
                }
            }
            return pending;
        }
    }
}
