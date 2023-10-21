namespace AdventOfCode.Core.Components
{
    public class DefaultInputAccessor : IInputAccessor
    {
        private int Year { get; }

        public DefaultInputAccessor(IYearAccessor yearAccessor)
        {
            Year = yearAccessor.Year;
        }

        public string[] GetInputLines(int day)
        {
            return File.ReadAllLines($@"./Input/{Year}/day{day}.txt");
        }
    }
}
