using AdventOfCode.Core.AoC;
using AdventOfCode.V2021.Core.Day24;
using System;
using System.Linq;

namespace AdventOfCode.V2021.Days
{
    internal class Day24 : Day
    {
        public override string PuzzleName => "Arithmetic Logic Unit";

        protected override string Solution1(string[] lines)
        {
            var instructions = lines.Select(l => l.Split(' ')).ToArray();
            var alu = new Alu(instructions);

            // Init
            var serial = new int[alu.Chunks.Count];
            Array.Fill(serial, 9);

            while (!alu.Run(serial))
            {
                for (int i = serial.Length - 1; i >= 0; i--)
                {
                    if (serial[i] > 1)
                    {
                        serial[i]--;
                        break;
                    }
                    else
                    {
                        serial[i] = 9;
                    }
                }
            }

            return string.Empty;
        }

        protected override string Solution2(string[] lines)
        {
            throw new System.NotImplementedException();
        }
    }
}
