using System;
using System.Collections.Generic;

namespace AdventOfCode.V2022.Core.Day10
{
    internal sealed class Cpu
    {
        public int X { get; set; }

        private List<ICommand> Commands { get; }

        public event EventHandler<ClockEventArgs> OnNewCycle;

        public Cpu(IEnumerable<string> commands)
        {
            Commands = new List<ICommand>();

            foreach (var command in commands)
            {
                var args = command.Split(' ');
                switch (args[0])
                {
                    case "noop":
                        Commands.Add(new NoopCommand());
                        break;
                    case "addx":
                        Commands.Add(new AddCommand(int.Parse(args[1])));
                        break;
                }
            }
        }

        public void Run()
        {
            var clock = 1;
            X = 1;

            foreach (var command in Commands)
            {
                for (var i = clock; clock < i + command.ClockCycles; clock++)
                {
                    OnNewCycle?.Invoke(this, new ClockEventArgs(clock, X));
                }
                command.Perform(this);
            }
        }
    }

    internal sealed class ClockEventArgs
    {
        public int ClockCycle { get; }
        public int X { get; }

        public ClockEventArgs(int clock, int x)
        {
            ClockCycle = clock;
            X = x;
        }
    }
}
