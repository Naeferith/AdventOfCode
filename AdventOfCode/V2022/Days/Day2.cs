using AdventOfCode.Core.AoC;
using AdventOfCode.V2022.Core.Day2;

namespace AdventOfCode.V2022.Days
{
    internal class Day2 : IDay
    {
        public string PuzzleName => "Rock Paper Scissors";

        public string Solution1(string[] lines)
        {
            var myScore = 0;

            foreach (var round in lines)
            {
                var hands = round.Split(' ');
                var opponentHand = GetOpponentHand(hands[0]);
                var myHand = GetMyHand1(hands[1]);

                myScore += myHand.PlayerScore();
                myScore += GetRoundScore(myHand, opponentHand);
            }

            return myScore.ToString();
        }

        public string Solution2(string[] lines)
        {
            var myScore = 0;

            foreach (var round in lines)
            {
                var hands = round.Split(' ');
                var opponentHand = GetOpponentHand(hands[0]);
                var myHand = GetMyHand2(hands[1], opponentHand);

                myScore += myHand.PlayerScore();
                myScore += GetRoundScore(myHand, opponentHand);
            }

            return myScore.ToString();
        }

        private static RockPaperScissors GetOpponentHand(string code)
        {
            return code switch
            {
                "A" => RockPaperScissors.Rock,
                "B" => RockPaperScissors.Paper,
                "C" => RockPaperScissors.Scissors,
                _ => default
            };
        }

        private static RockPaperScissors GetMyHand1(string code)
        {
            return code switch
            {
                "X" => RockPaperScissors.Rock,
                "Y" => RockPaperScissors.Paper,
                "Z" => RockPaperScissors.Scissors,
                _ => default
            };
        }

        private static RockPaperScissors GetMyHand2(string code, RockPaperScissors op)
        {
            if (code == "Y")
                return op;

            RockPaperScissors myHand;

            if (code == "X")
            {
                myHand = op switch
                {
                    RockPaperScissors.Rock => RockPaperScissors.Scissors,
                    RockPaperScissors.Paper => RockPaperScissors.Rock,
                    RockPaperScissors.Scissors => RockPaperScissors.Paper,
                    _ => throw new System.NotImplementedException(),
                };
            }
            else
            {
                myHand = op switch
                {
                    RockPaperScissors.Rock => RockPaperScissors.Paper,
                    RockPaperScissors.Paper => RockPaperScissors.Scissors,
                    RockPaperScissors.Scissors => RockPaperScissors.Rock,
                    _ => throw new System.NotImplementedException(),
                };
            }

            return myHand;
        }

        private static int GetRoundScore(RockPaperScissors my, RockPaperScissors other)
        {
            if (my == other)
                return 3;

            if ((my == RockPaperScissors.Rock && other == RockPaperScissors.Scissors)
                || (my == RockPaperScissors.Paper && other == RockPaperScissors.Rock)
                || (my == RockPaperScissors.Scissors && other == RockPaperScissors.Paper))
                return 6;

            return 0;
        }
    }
}
