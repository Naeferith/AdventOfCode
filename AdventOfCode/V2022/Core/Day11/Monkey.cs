using AdventOfCode.Core.Utils;
using Nae.Utils.Maths;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.V2022.Core.Day11
{
    internal sealed class Monkey
    {
        public bool Reduce { get; }
        public int InspectCount { get; private set; }
        private int Divisor { get; set; }

        private List<ulong> Items { get; set; }
        private Func<ulong, ulong> WorryOperation { get; set; }
        private Func<ulong, int> TestOperation { get; set; }

        public Monkey(bool reduce)
        {
            Reduce = reduce;
        }

        private static Func<ulong, ulong> GetOperation(string op, ulong? rOp)
        {
            return op switch
            {
                "+" => (old) => old + (rOp ?? old),
                "*" => (old) => old * (rOp ?? old),
                _ => throw new NotImplementedException()
            };
        }

        public void SetItems(IEnumerable<int> items)
        {
            Items = new(items.Select(i => (ulong)i));
        }

        public void SetOperation(string operation)
        {
            var rValue = operation.Split('=')[^1].Trim();
            var args = rValue.Split(' ');

            ulong? rOp = null;

            if (ulong.TryParse(args[2], out var rInt))
                rOp = rInt;

            WorryOperation = GetOperation(args[1], rOp);
        }

        public void SetTest(int denominator, int trueMonkey, int falseMonkey)
        {
            Divisor = denominator;
            TestOperation = (level) => level % (ulong)Divisor == 0
                ? trueMonkey
                : falseMonkey;
        }

        public void Inspect(IList<Monkey> monkeys)
        {
            var lcm = MathI.Lcm(monkeys.Select(m => m.Divisor));

            InspectCount += Items.Count;

            foreach (var item in Items)
            {
                var newWorry = WorryOperation(item);

                if (Reduce)
                    newWorry /= 3;

                newWorry %= (ulong)lcm;

                var index = TestOperation(newWorry);
                monkeys[index].Items.Add(newWorry);
            }

            Items.Clear();
        }
    }
}
