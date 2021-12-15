using System.IO;
using System.Linq;

namespace AdventOfCode.Business
{
    internal sealed partial class AoC21
    {
        private static class Day2
        {
            public static string Solution1()
            {
                var movements = File.ReadLines($"{INPUT_PATH}day2.txt").Select(o =>
                {
                    var args = o.Split(' ');
                    return (args[0], int.Parse(args[1]));
                });

                int hPos = 0, dPos = 0;

                foreach (var movement in movements)
                {
                    switch (movement.Item1)
                    {
                        case "forward":
                            hPos += movement.Item2;
                            break;
                        case "up":
                            dPos -= movement.Item2;
                            break;
                        case "down":
                            dPos += movement.Item2;
                            break;
                        default:
                            break;
                    }
                }

                return (dPos * hPos).ToString();
            }

            public static string Solution2()
            {
                var movements = File.ReadLines($"{INPUT_PATH}day2.txt").Select(o =>
                {
                    var args = o.Split(' ');
                    return (args[0], int.Parse(args[1]));
                });

                int hPos = 0, dPos = 0, aim = 0;

                foreach (var movement in movements)
                {
                    switch (movement.Item1)
                    {
                        case "forward":
                            hPos += movement.Item2;
                            dPos += aim * movement.Item2;
                            break;
                        case "up":
                            aim -= movement.Item2;
                            break;
                        case "down":
                            aim += movement.Item2;
                            break;
                        default:
                            break;
                    }
                }

                return (dPos * hPos).ToString();
            }
        }
    }
}
