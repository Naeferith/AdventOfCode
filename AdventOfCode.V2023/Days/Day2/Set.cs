namespace AdventOfCode.V2023.Days.Day2
{
    internal class Set
    {
        public Set(string str)
        {
            var args = str.Split(", ");

            foreach (var arg in args)
            {
                var objs = arg.Split(' ');

                if (objs[1] == "red")
                {
                    Red = int.Parse(objs[0]);
                }
                else if (objs[1] == "blue")
                {
                    Blue = int.Parse(objs[0]);
                }
                else if (objs[1] == "green")
                {
                    Green = int.Parse(objs[0]);
                }
            }
        }
        public Set(int blue, int red, int green)
        {
            Blue = blue;
            Red = red;
            Green = green;
        }

        public int Blue { get; set; }

        public int Red { get; set; }

        public int Green { get; set; }
    }
}
