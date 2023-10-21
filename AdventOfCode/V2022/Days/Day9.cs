using AdventOfCode.Core.AoC;
using AdventOfCode.V2022.Core.Day9;
using System;
using System.Collections.Generic;

namespace AdventOfCode.V2022.Days
{
    internal class Day9 : Day
    {
        private HashSet<Point> _tPositions;
        private Point[] _knots;

        public override string PuzzleName => "Rope Bridge";

        protected override string Solution1(string[] lines)
        {
            return Solution(lines, 2);
        }

        protected override string Solution2(string[] lines)
        {
            return Solution(lines, 10);
        }

        private void Move(Action<Point> movement, int distance)
        {
            for (int i = 0; i < distance; i++)
            {
                // Move
                movement(_knots[0]);

                // Adjust
                for (int k = 0; k < _knots.Length - 1; k++)
                {
                    var hPos = _knots[k];
                    var tPos = _knots[k + 1];

                    var dX = hPos.X - tPos.X;
                    var absX = Math.Abs(dX);

                    var dY = hPos.Y - tPos.Y;
                    var absY = Math.Abs(dY);

                    if (absX > 1)
                    {
                        tPos.X += absX / dX;

                        if (absY > 0)
                            tPos.Y += absY / dY;
                    }
                    else if (absY > 1)
                    {
                        tPos.Y += absY / dY;

                        if (absX > 0)
                            tPos.X += absX / dX;
                    }
                }

                //Save
                _tPositions.Add(new(_knots[^1]));
            }
        }

        private string Solution(string[] lines, int nbKnots)
        {
            #region Init
            _tPositions = new() { Point.Zero };
            _knots = new Point[nbKnots];

            for (int i = 0; i < _knots.Length; i++)
            {
                _knots[i] = Point.Zero;
            }
            #endregion

            foreach (var line in lines)
            {
                var args = line.Split(' ');
                var direction = args[0];
                var distance = int.Parse(args[1]);

                switch (direction)
                {
                    case "U":
                        Move(p => p.X--, distance);
                        break;
                    case "D":
                        Move(p => p.X++, distance);
                        break;
                    case "L":
                        Move(p => p.Y--, distance);
                        break;
                    case "R":
                        Move(p => p.Y++, distance);
                        break;
                }
            }

            return _tPositions.Count.ToString();
        }
    }
}
