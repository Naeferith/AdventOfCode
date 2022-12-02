using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.V2022.Core.Day2
{
    internal enum RockPaperScissors
    {
        Rock,
        Paper,
        Scissors
    }

    internal static class RockPaperScissorExtensions
    {
        public static int PlayerScore(this RockPaperScissors p)
        {
            return p switch
            {
                RockPaperScissors.Rock => 1,
                RockPaperScissors.Paper => 2,
                RockPaperScissors.Scissors => 3,
                _ => default,
            };
        }
    }
}
