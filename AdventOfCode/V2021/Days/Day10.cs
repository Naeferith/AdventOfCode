using AdventOfCode.Core.AoC;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.V2021.Days
{
    internal class Day10 : IDay
    {
        private static readonly Dictionary<char, char> _tokens = new()
        {
            ['('] = ')',
            ['{'] = '}',
            ['['] = ']',
            ['<'] = '>'
        };

        public string PuzzleName => "Syntax Scoring";

        public string Solution1(string[] lines)
        {
            var sum = 0;
            foreach (var line in lines)
            {
                if (IsCorrupted(line, out var c))
                {
                    switch (c)
                    {
                        case ')':
                            sum += 3;
                            break;
                        case ']':
                            sum += 57;
                            break;
                        case '}':
                            sum += 1197;
                            break;
                        case '>':
                            sum += 25137;
                            break;
                        default:
                            break;
                    }
                }
            }

            return sum.ToString();
        }

        public string Solution2(string[] lines)
        {
            var scores = new SortedSet<ulong>();
            foreach (var line in lines.Where(l => !IsCorrupted(l, out var _)))
            {
                if (IsIncomplete(line, out var s))
                {
                    var score = 0UL;
                    do
                    {
                        switch (s.Pop())
                        {
                            case '(':
                                score = score * 5 + 1;
                                break;
                            case '[':
                                score = score * 5 + 2;
                                break;
                            case '{':
                                score = score * 5 + 3;
                                break;
                            case '<':
                                score = score * 5 + 4;
                                break;
                            default:
                                break;
                        }
                    } while (s.Any());
                    scores.Add(score);
                }
            }

            return scores.ElementAt(scores.Count / 2).ToString();
        }

        private static bool IsCorrupted(string line, out char corrupted)
        {
            var stack = new Stack<char>();

            foreach (var c in line)
            {
                if (_tokens.ContainsKey(c))
                    stack.Push(c);
                else if (_tokens.First(x => x.Value == c).Key == stack.Peek())
                    stack.Pop();
                else
                {
                    corrupted = c;
                    return true;
                }
            }
            corrupted = default;
            return false;
        }

        private static bool IsIncomplete(string line, out Stack<char> remainings)
        {
            remainings = new Stack<char>();

            foreach (var c in line)
            {
                if (_tokens.ContainsKey(c))
                    remainings.Push(c);
                else if (_tokens.First(x => x.Value == c).Key == remainings.Peek())
                    remainings.Pop();
            }

            return remainings.Any();
        }
    }
}
