using AdventOfCode.Core.Components;
using AdventOfCode.V2022.Core.Day22;
using Nae.Utils.Components;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.V2022.Days
{
    internal class Day22 : IDay
    {
        public int DayNumber => 22;

        public string PuzzleName => "Monkey Map";

        private IEnumerable<MapChunk> _mapChunks = null!;
        private MapChunk _activeChunk = null!;
        private int _facing;

        public string Solution1(string[] lines)
        {
            Initialize(lines);
            ParseDirection(lines[^1]);

            var passwd = _facing
                + (_activeChunk.Cursor[0] + 1) * 4
                + (_activeChunk.Cursor[1] + 1) * 1_000;
            return passwd.ToString();
        }

        public string Solution2(string[] lines)
        {
            throw new NotImplementedException();
        }

        private void Initialize(string[] lines)
        {
            _facing = 0;
            ParseMap(lines);
        }

        private void ParseDirection(string direction)
        {
            var isNumeric = true;
            var startIndex = 0;
            for (int i = 0; i < direction.Length; i++)
            {
                if (isNumeric != char.IsAsciiDigit(direction[i]))
                {
                    var str = direction[startIndex..i];

                    if (isNumeric)
                    {
                        Move(int.Parse(str));

                    }
                    else
                    {
                        Rotate(str[0]);
                    }

                    isNumeric = !isNumeric;
                    startIndex = i--;
                }
            }
        }

        private void Move(int num)
        {
            var forward = _facing / 2 == 0;
            var moves = forward
                ? _activeChunk.Forward(num)
                : _activeChunk.Backward(num);

            if (moves.Result != MovementStatus.EnconteredEdge)
            {
                return;
            }

            var oldChunk = _activeChunk;

            // Only crossing top and bottom edge changes the chunk (with the way it's parsed)
            if (_activeChunk.MovingDimension == 1)
            {
                var tmpChunks = _mapChunks.Where(c => c.IsIndexInDimension(0, _activeChunk.Cursor[0])).ToArray();

                var contigousChunk = Array.Find(tmpChunks, c => c.IsIndexInDimension(1, _activeChunk.Cursor[1] + 1));

                contigousChunk ??= forward
                    ? tmpChunks
                        .OrderBy(c => c.GetUpperBound(1))
                        .First()
                    : tmpChunks
                        .OrderByDescending(c => c.GetUpperBound(1))
                        .First();

                _activeChunk = contigousChunk;
                _activeChunk.MovingDimension = 1;
            }

            var oldCursor = new int[_activeChunk.Cursor.Length];
            var tmpCursor = new int[_activeChunk.Cursor.Length];
            oldChunk.Cursor.CopyTo(oldCursor, 0);
            oldChunk.Cursor.CopyTo(tmpCursor, 0);

            tmpCursor[_activeChunk.MovingDimension] = forward
                    ? _activeChunk.GetLowerBound(_activeChunk.MovingDimension)
                    : _activeChunk.GetUpperBound(_activeChunk.MovingDimension);

            _activeChunk.SetCursor(tmpCursor);

            if (_activeChunk.Current == '#')
            {
                _activeChunk = oldChunk;
                _activeChunk.SetCursor(oldCursor);
                return;
            }

            _activeChunk.Cursor[_activeChunk.MovingDimension] = forward
                ? _activeChunk.Cursor[_activeChunk.MovingDimension] - 1
                : _activeChunk.Cursor[_activeChunk.MovingDimension] + 1;

            Move(num - moves.ProcessedMoves);
        }

        private void Rotate(char rotation)
        {
            _activeChunk.MovingDimension = (_activeChunk.MovingDimension + 1) % 2;

            if (rotation == 'L')
            {
                _facing--;
                if (_facing == -1)
                {
                    _facing = 3;
                }
            }
            else
            {
                _facing++;
                if (_facing == 4)
                {
                    _facing = 0;
                }
            }
        }

        [MemberNotNull(nameof(_mapChunks), nameof(_activeChunk))]
        private void ParseMap(string[] lines)
        {
            var chars = new[] { '.', '#' };

            int xOffset = lines[0].IndexOfAny(chars), xLen = lines[0][xOffset..].TakeWhile(c => chars.Contains(c)).Count();
            var chunkBuffer = new List<char[]>();
            var chunks = new List<MapChunk>();

            for (int y = 0; y < lines.Length; y++)
            {
                var lineOffest = lines[y].IndexOfAny(chars);

                if (lineOffest < 0)
                {
                    AddChunk(xOffset, xLen, chunkBuffer, chunks, y - chunkBuffer.Count);
                    break;
                }

                var line = lines[y][lineOffest..].TakeWhile(c => chars.Contains(c)).ToArray();

                if (xOffset == lineOffest && xLen == line.Length)
                {
                    chunkBuffer.Add(line);
                }
                else
                {
                    AddChunk(xOffset, xLen, chunkBuffer, chunks, y - chunkBuffer.Count);
                    xOffset = lineOffest;
                    xLen = line.Length;
                    y--;
                }
            }

            _mapChunks = chunks;
            _activeChunk = chunks[0];
        }

        private static void AddChunk(int xOffset, int xLen, List<char[]> chunkBuffer, List<MapChunk> chunks, int yOffest)
        {
            var chunk = new MapChunk(xLen, chunkBuffer.Count, xOffset, yOffest);
            var vals = chunkBuffer.SelectMany((str, y) => Project(str, y, xOffset, yOffest));

            chunk.Initialize(vals.Select(v => v.Item1), vals.Select(v => v.Item2));
            chunks.Add(chunk);
            chunkBuffer.Clear();
        }

        private static IEnumerable<(char, int[])> Project(char[] str, int y, int xOffset, int yOffest)
        {
            for (int x = 0; x < str.Length; x++)
            {
                yield return (str[x], new[] { x + xOffset, y + yOffest });
            }
        }
    }
}
