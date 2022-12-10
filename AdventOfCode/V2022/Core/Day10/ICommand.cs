namespace AdventOfCode.V2022.Core.Day10
{
    internal interface ICommand
    {
        int ClockCycles { get; }

        void Perform(Cpu cpu);
    }
}
