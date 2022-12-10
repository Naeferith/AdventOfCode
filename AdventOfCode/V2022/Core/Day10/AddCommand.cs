namespace AdventOfCode.V2022.Core.Day10
{
    internal sealed class AddCommand : ICommand
    {
        public int Increment { get; }
        public int ClockCycles => 2;

        public AddCommand(int increment)
        {
            Increment = increment;
        }

        public void Perform(Cpu cpu)
        {
            cpu.X += Increment;
        }
    }
}
