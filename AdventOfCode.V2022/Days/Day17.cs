using AdventOfCode.Core.Components;
using AdventOfCode.V2022.Core.Day17;
using Nae.Utils.Extensions;
using Nae.Utils.Structs;

namespace AdventOfCode.V2022.Days
{
    internal class Day17 : IDay
    {
        private const int CHAMBER_WIDTH = 7;
        private const int LEFT_EDGE_DIST = 2;
        private const int TOP_EDGE_DIST = 3;

        private static readonly Point<long>[][] Shapes = new[]
        {
            // Horziontal Line
            new Point<long>[]
            {
                new(0, 0),
                new(1, 0),
                new(2, 0),
                new(3, 0)
            },

            // Plus Sign
            new Point<long>[]
            {
                new(1, 0),
                new(0, 1),
                new(2, 1),
                new(1, 2),
            },

            // Reverse L
            new Point<long>[]
            {
                new(0, 0),
                new(1, 0),
                new(2, 0),
                new(2, 1),
                new(2, 2),
            },

            // Vertical Line
            new Point<long>[]
            {
                new(0, 0),
                new(0, 1),
                new(0, 2),
                new(0, 3),
            },

            // Square
            new Point<long>[]
            {
                new(0, 0),
                new(0, 1),
                new(1, 0),
                new(1, 1),
            }
        };

        private readonly TetrisBlock[] _relevantBlocks;

        private IEnumerable<TetrisBlock> RelevantBlocks => _relevantBlocks.Distinct();

        private TetrisBlock? FallingBlock { get; set; }

        public int DayNumber => 17;

        public string PuzzleName => "Pyroclastic Flow";

        public Day17()
        {
            _relevantBlocks = new TetrisBlock[CHAMBER_WIDTH];
            FallingBlock = new TetrisBlock(Point<long>.Zero, EnumerableExtensions.Range(CHAMBER_WIDTH, i => new Point<long>(i, -1)).ToArray());
            BlockFallen(0);
        }

        public string Solution1(string[] lines)
        {
            return Solution(lines[0], 2022);
        }

        public string Solution2(string[] lines)
        {
            throw new NotImplementedException();
            // Too slow to be ran yet
            return Solution(lines[0], 1_000_000_000_000);
        }

        private string Solution(string directions, long blockCount)
        {
            using var dirIterator = directions.Loop().GetEnumerator();
            dirIterator.MoveNext();

            for (long i = 0; i < blockCount;)
            {
                if (ManageTurn(dirIterator.Current))
                {
                    BlockFallen(++i);
                }

                dirIterator.MoveNext();
            }

            return BlockPeak.ToString();
        }

        private void BlockFallen(long i)
        {
            foreach (var x in FallingBlock!.Points.Select(p => p.X).Distinct())
            {
                _relevantBlocks[x] = FallingBlock;
            }

            var shape = Shapes[i % Shapes.Length];
            var newShape = new Point<long>[shape.Length];
            Array.Copy(shape, newShape, shape.Length);

            FallingBlock = new TetrisBlock(
                new Point<long>(LEFT_EDGE_DIST, BlockPeak + TOP_EDGE_DIST),
                newShape);
        }

        private bool ManageTurn(char direction)
        {
            WindPush(direction);
            return !FallDown();
        }

        private bool FallDown()
        {
            var newPos = FallingBlock!.Points
                .Select(p => p.Down())
                .ToArray();

            if (Array.TrueForAll(newPos, IsValid))
            {
                FallingBlock.Points = newPos;
                return true;
            }

            return false;
        }

        private void WindPush(char dir)
        {
            var newPos = FallingBlock!.Points
                .Select(p => ApplyDirection(p, dir))
                .ToArray();

            if (Array.TrueForAll(newPos, IsValid))
            {
                FallingBlock.Points = newPos;
            }
        }

        private bool IsValid(Point<long> point)
        {
            return point.X >= 0
                && point.Y >= 0
                && point.X < CHAMBER_WIDTH
                && !RelevantBlocks
                    .SelectMany(b => b.Points)
                    .Any(p => p == point);
        }

        private long BlockPeak => RelevantBlocks.Max(b => b.Points.Max(p => p.Y)) + 1;

        private static Point<long> ApplyDirection(Point<long> point, char c)
        {
            return c switch
            {
                '<' => point.Left(),
                '>' => point.Right(),
                _ => throw new InvalidOperationException()
            };
        }
    }
}
