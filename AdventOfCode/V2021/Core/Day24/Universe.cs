using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AdventOfCode.V2021.Core.Day24
{
    internal sealed class Universe
    {
        public int[] Serial { get; }
        public List<int>[] ForbiddenValues { get; }
        
        public BinaryExpression[] Instructions { get; }

        public Expression wExp;
        public Expression xExp;
        public Expression yExp;
        public Expression zExp;

        private Universe(int serialSize, int instructionsSize)
        {
            Serial = new int[serialSize];
            ForbiddenValues = new List<int>[serialSize];
            Instructions = new BinaryExpression[instructionsSize];

            wExp = Expression.Constant(0);
            xExp = Expression.Constant(0);
            yExp = Expression.Constant(0);
            zExp = Expression.Constant(0);

            for (int i = 0; i < ForbiddenValues.Length; i++)
            {
                ForbiddenValues[i] = new List<int>();
            }
        }

        public Universe(int serialSize, BinaryExpression[] instructions) : this(serialSize, instructions.Length)
        {
            Instructions = instructions;
        }

        public Universe(Universe universe, int index, int value) : this(universe.Serial.Length, universe.Instructions)
        {
            Array.Copy(universe.Serial, Serial, universe.Serial.Length);
            Array.Copy(universe.Instructions, Instructions, universe.Instructions.Length);
            Array.Copy(universe.ForbiddenValues, ForbiddenValues, universe.ForbiddenValues.Length);

            if (value < 0)
            {
                ForbiddenValues[index].Add(-value);
            }
            else
                Serial[index] = value;
        }

        public Expression GetExpressionOfMember(string varName)
        {
            return varName switch
            {
                nameof(AluData.W) => wExp,
                nameof(AluData.X) => xExp,
                nameof(AluData.Y) => yExp,
                nameof(AluData.Z) => zExp,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
