﻿using AdventOfCode.Core.Components;

namespace AdventOfCode.V2021.Days
{
    internal class Day2 : IDay
    {
        public int DayNumber => 2;

        public string PuzzleName => "Dive !";

        public string Solution1(string[] lines)
        {
            var movements = lines.Select(o =>
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

        public string Solution2(string[] lines)
        {
            var movements = lines.Select(o =>
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
