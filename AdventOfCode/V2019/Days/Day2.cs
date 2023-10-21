using AdventOfCode.Core.AoC;
using System;
using System.Linq;

namespace AdventOfCode.V2019.Days
{
    internal class Day2 : Day
    {
        public override string PuzzleName => "Program Alarm";

        protected override string Solution1(string[] lines)
        {
            var ints = lines.First().Split(',').Select(i => int.Parse(i)).ToArray();

            // Init
            ints[1] = 12;
            ints[2] = 2;

            // Run
            Run(ints);

            return ints[0].ToString();
        }

        protected override string Solution2(string[] lines)
        {
            var ints = lines.First().Split(',').Select(i => int.Parse(i)).ToArray();
            int[] work = new int[ints.Length];
            int noun = -1, verb = 0;

            do
            {
                // Reinit
                ints.CopyTo(work, 0);

                // Init
                noun++;
                if (noun == 100)
                {
                    noun = 0;
                    verb++;
                }

                work[1] = noun;
                work[2] = verb;

                // Run
                Run(work);
            } while (work[0] != 19690720);

            return (100 * noun + verb).ToString();
        }

        private static void Run(int[] ints)
        {
            var index = 0;

            while (true)
            {
                switch (ints[index])
                {
                    case 1:
                        Add(ints, ref index);
                        break;
                    case 2:
                        Multiply(ints, ref index);
                        break;
                    case 99:
                        return;
                }
            }
        }

        private static void Add(int[] ints, ref int opIndex) => Operate(ints, ref opIndex, (a, b) => a + b);

        private static void Multiply(int[] ints, ref int opIndex) => Operate(ints, ref opIndex, (a, b) => a * b);

        private static void Operate(int[] ints, ref int opIndex, Func<int, int, int> op)
        {
            var sum = op(ints[ints[opIndex + 1]], ints[ints[opIndex + 2]]);
            ints[ints[opIndex + 3]] = sum;

            opIndex += 4;
        }
    }
}
