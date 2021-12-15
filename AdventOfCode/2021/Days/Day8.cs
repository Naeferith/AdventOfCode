using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Business
{
    internal sealed partial class AoC21
    {
        private static class Day8
        {
            public static string Solution1()
            {
                var segments = File.ReadLines($"{INPUT_PATH}day8.txt").Select(t =>
                {
                    return t.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries)[1].Split(' ')
                    .Where(s => s.Length == 2 || s.Length == 3 || s.Length == 4 || s.Length == 7)
                    .Count();
                }
                );

                return segments.Sum().ToString();
            }

            public static string Solution2()
            {
                var lines = File.ReadLines($"{INPUT_PATH}day8.txt");

                var sum = 0;
                foreach (var line in lines)
                {
                    var dec = new Decoder();
                    var args = line.Split(new string[] { " | " }, StringSplitOptions.RemoveEmptyEntries);
                    dec.Analyse(args[0].Split(' '));
                    sum += dec.Decode(args[1].Split(' '));
                }

                return sum.ToString();
            }

            private class Decoder
            {
                private readonly Dictionary<int, string> Numbers;

                public Decoder()
                {
                    Numbers = new Dictionary<int, string>(10)
                    {
                        [8] = "abcdefg"
                    };
                }

                public void Analyse(string[] input)
                {
                    var ez = input.Where(s => s.Length == 2 || s.Length == 3 || s.Length == 4);
                    var hard = input.Where(s => s.Length == 5 || s.Length == 6);

                    foreach (var str in ez)
                    {
                        switch (str.Length)
                        {
                            case 2:
                                Numbers[1] = string.Concat(str.OrderBy(c => c));
                                break;
                            case 3:
                                Numbers[7] = string.Concat(str.OrderBy(c => c));
                                break;
                            case 4:
                                Numbers[4] = string.Concat(str.OrderBy(c => c));
                                break;
                            default:
                                break;
                        }
                    }

                    foreach (var str in hard.OrderBy(h => h.Length))
                    {
                        switch (str.Length)
                        {
                            case 5:
                                {
                                    var strOne = Numbers[1];
                                    var strFour = Numbers[4];

                                    if (strOne.Intersect(str).Count() == strOne.Length)
                                        Numbers[3] = string.Concat(str.OrderBy(c => c));
                                    else if (strFour.Intersect(str).Count() == 3)
                                        Numbers[5] = string.Concat(str.OrderBy(c => c));
                                    else
                                        Numbers[2] = string.Concat(str.OrderBy(c => c));
                                }
                                break;
                            case 6:
                                {
                                    var strFour = Numbers[4];
                                    var strSeven = Numbers[7];

                                    if (str.Intersect(strSeven).Count() != strSeven.Length)
                                        Numbers[6] = string.Concat(str.OrderBy(c => c));
                                    else if (str.Intersect(strFour).Count() == strFour.Length)
                                        Numbers[9] = string.Concat(str.OrderBy(c => c));
                                    else
                                        Numbers[0] = string.Concat(str.OrderBy(c => c));
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                public int Decode(string[] output)
                {
                    var outstr = string.Empty;
                    foreach (var str in output)
                        outstr += Numbers.First(e => e.Value.Equals(string.Concat(str.OrderBy(c => c)))).Key;

                    return int.Parse(outstr);
                }
            }
        }
    }
}
