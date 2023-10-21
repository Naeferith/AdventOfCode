using Nae.Utils.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.V2022.Core.Day13
{
    internal sealed class SignalComparer : IComparer<string>
    {
        private const string EMPTY_ITEM = "[]";
        private const char ITEM_DIVIDER = ',';

        public int Compare(string left, string right)
        {
            if (string.IsNullOrEmpty(left) && !string.IsNullOrEmpty(right))
                return 1;

            if (!string.IsNullOrEmpty(left) && string.IsNullOrEmpty(right))
                return -1;

            var lCursor = 1;
            var rCursor = 1;

            while (true)
            {
                if (lCursor + 1 >= left.Length && rCursor + 1 >= right.Length)
                    return 0;

                if (left[lCursor] == ITEM_DIVIDER)
                    lCursor++;
                if (right[rCursor] == ITEM_DIVIDER)
                    rCursor++;

                // Potential Int Conversion
                int? lVal = ParseAsInt(left, ref lCursor);
                int? rVal = ParseAsInt(right, ref rCursor);

                // Int - Int
                if (rVal.HasValue && lVal.HasValue)
                {
                    if (lVal.Value == rVal.Value)
                    {
                        continue;
                    }

                    return rVal.Value - lVal.Value;
                }

                // Potential List conversion + Mismatch if necessary
                string lSub = ParseAsList(left) ?? $"[{lVal}]";
                string rSub = ParseAsList(right) ?? $"[{rVal}]";

                var res = Compare(lSub, rSub);
                if (res != 0)
                    return res;
                else
                {
                    left = Strip(left, lVal?.ToString().Length ?? lSub.Length);
                    right = Strip(right, rVal?.ToString().Length ?? rSub.Length);

                    lCursor = 1;
                    rCursor = 1;
                }
            }
        }

        private static int? ParseAsInt(string expr, ref int cursor)
        {
            if (expr == EMPTY_ITEM)
                return -1;

            if (!expr[cursor].IsNumeric())
                return null;

            var start = cursor;

            while (expr[cursor].IsNumeric())
            {
                cursor++;
            }

            return int.Parse(expr[start..cursor]);
        }

        private static string ParseAsList(string expr)
        {
            if (expr == EMPTY_ITEM)
                return string.Empty;

            if (expr[1] != '[')
                return null;

            var cursor = 1;
            var stack = new Stack<int>();
            stack.Push(cursor);
            var start = cursor;

            do
            {
                cursor++;
                switch (expr[cursor])
                {
                    case '[':
                        stack.Push(cursor);
                        break;
                    case ']':
                        stack.Pop();
                        break;
                }
            } while (stack.Any());

            return expr[start..(cursor + 1)];
        }

        private static string Strip(string str, int count)
        {
            if (str == EMPTY_ITEM)
                return str;

            if (!str.Contains(ITEM_DIVIDER))
                return EMPTY_ITEM;

            if (str[count + 1] == ITEM_DIVIDER)
                count++;

            return "[" + str[(count + 1)..];
        }
    }
}
