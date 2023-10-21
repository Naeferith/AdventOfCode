using AdventOfCode.Core.Components;

namespace AdventOfCode.V2022.Days
{
    internal class Day8 : Day
    {
        public override int DayNumber => 8;

        public override string PuzzleName => "Treetop Tree House";

        protected override string Solution1(string[] lines)
        {
            var width = lines[0].Length;
            var height = lines.Length;

            var sum = 2 * (width + height - 2);

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    if (IsVisible(lines, x, y))
                        sum++;
                }
            }

            return sum.ToString();
        }

        protected override string Solution2(string[] lines)
        {
            var width = lines[0].Length;
            var height = lines.Length;

            var max = 0;

            for (int x = 1; x < width - 1; x++)
            {
                for (int y = 1; y < height - 1; y++)
                {
                    var score = GetScore(lines, x, y);

                    if (score > max)
                        max = score;
                }
            }

            return max.ToString();
        }

        private static bool IsVisible(string[] grid, int x, int y)
        {
            var val = grid[x][y];
            var width = grid[0].Length;
            var height = grid.Length;
            bool top = true, bot = true, left = true, right = true;

            for (int _x = 0; _x < x; _x++)
            {
                top &= grid[_x][y] < val;
            }

            for (int _x = x + 1; _x < width; _x++)
            {
                bot &= grid[_x][y] < val;
            }

            for (int _y = 0; _y < y; _y++)
            {
                left &= grid[x][_y] < val;
            }

            for (int _y = y + 1; _y < height; _y++)
            {
                right &= grid[x][_y] < val;
            }

            return top || bot || left || right;
        }

        private static int GetScore(string[] grid, int x, int y)
        {
            var val = grid[x][y];
            var width = grid[0].Length;
            var height = grid.Length;
            int top = 0, bot = 0, left = 0, right = 0;

            for (int _x = x - 1; _x >= 0; _x--)
            {
                top++;
                if (grid[_x][y] >= val)
                    break;
            }

            for (int _x = x + 1; _x < width; _x++)
            {
                bot++;
                if (grid[_x][y] >= val)
                    break;
            }

            for (int _y = y - 1; _y >= 0; _y--)
            {
                left++;
                if (grid[x][_y] >= val)
                    break;
            }

            for (int _y = y + 1; _y < height; _y++)
            {
                right++;
                if (grid[x][_y] >= val)
                    break;
            }

            return top * bot * left * right;
        }
    }
}
