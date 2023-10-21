using AdventOfCode.Core.AoC;
using AdventOfCode.V2022.Core.Day10;
using System;
using System.Linq;

namespace AdventOfCode.V2022.Days
{
    internal class Day10 : Day
    {
        private Cpu Cpu { get; set; }
        private int[] _observedCycles;
        private int Aggregate { get; set; }

        public override string PuzzleName => "Cathode-Ray Tube";

        protected override string Solution1(string[] lines)
        {
            return Solution(lines, ObserveCycle, 20, 60, 100, 140, 180, 220);
        }

        protected override string Solution2(string[] lines)
        {
            return Solution(lines, Draw, 40, 80, 120, 160, 200, 240);
        }

        public string Solution(string[] lines, EventHandler<ClockEventArgs> handler, params int[] cycles)
        {
            Cpu = new Cpu(lines);
            Cpu.OnNewCycle += handler;
            _observedCycles = cycles;

            Cpu.Run();

            return Aggregate.ToString();
        }

        private void ObserveCycle(object sender, ClockEventArgs e)
        {
            if (_observedCycles.Contains(e.ClockCycle))
                Aggregate += e.ClockCycle * e.X;
        }

        private void Draw(object sender, ClockEventArgs e)
        {
            var pos = (e.ClockCycle - 1) % 40;

            Console.Write(pos >= e.X - 1 && pos <= e.X + 1
                ? "#"
                : ".");

            if (_observedCycles.Contains(e.ClockCycle))
                Console.WriteLine();
        }
    }
}
