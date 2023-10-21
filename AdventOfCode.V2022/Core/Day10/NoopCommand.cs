namespace AdventOfCode.V2022.Core.Day10
{
    internal sealed class NoopCommand : ICommand
    {
        public int ClockCycles => 1;

        public void Perform(Cpu cpu)
        {
            // Do nothing
        }
    }
}
