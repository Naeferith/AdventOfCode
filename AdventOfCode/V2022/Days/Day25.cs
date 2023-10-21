using AdventOfCode.Core.AoC;
using AdventOfCode.Core.Utils;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace AdventOfCode.V2022.Days
{
    internal class Day25 : IDay
    {
        public string PuzzleName => "Full of Hot Air";

        private static readonly IDictionary<char, int> SnafuToken = new Dictionary<char, int>
        {
            ['='] = -2,
            ['-'] = -1,
            ['0'] = 0,
            ['1'] = 1,
            ['2'] = 2
        };

        private static T SnafuToDecimal<T>(ReadOnlySpan<char> snafu) where T : IBinaryInteger<T>
        {
            var sum = T.Zero;

            for (int i = snafu.Length - 1; i >= 0; i--)
            {
                sum += T.CreateChecked(Math.Pow(5, snafu.Length - i - 1) * SnafuToken[snafu[i]]);
            }

            return sum;
        }

        private static ReadOnlySpan<char> DecimalToSnafu<T>(T number) where T : IBinaryInteger<T>
        {
            const string snafuTokens = "012=-";

            // Convert base 10 to 5
            var b5 = $"0{Maths.NumberToStringBase(number, snafuTokens)}".ToCharArray();
            var retain = false;

            for (int i = b5.Length - 1; i >= 0; i--)
            {
                // If retain, increment
                if (retain)
                {
                    b5[i] = snafuTokens[(snafuTokens.IndexOf(b5[i]) + 1) % snafuTokens.Length];
                    retain = b5[i] == '0';
                }

                retain |= b5[i] == '=' || b5[i] == '-';
            }

            return b5[0] == '0'
                ? b5[1..]
                : b5;
        }

        public string Solution1(string[] lines)
        {
            var sum = 0L;

            foreach (var line in lines)
            {
                sum += SnafuToDecimal<long>(line);
            }

            return DecimalToSnafu(sum).ToString();
        }

        public string Solution2(string[] lines)
        {
            throw new NotImplementedException();
        }
    }
}
