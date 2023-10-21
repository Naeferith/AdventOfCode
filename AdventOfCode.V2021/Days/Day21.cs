using AdventOfCode.Core.Components;

namespace AdventOfCode.V2021.Days
{
    internal class Day21 : IDay
    {
        public int DayNumber => 21;

        private static readonly Dictionary<int, int> DiceValues = new()
        {
            [3] = 1,
            [4] = 3,
            [5] = 6,
            [6] = 7,
            [7] = 6,
            [8] = 3,
            [9] = 1
        };

        public string PuzzleName => "Dirac Dice";

        public string Solution1(string[] lines)
        {
            const int MAX = 1000;
            var p1 = new Player(int.Parse(string.Empty + lines[0].Last()));
            var p2 = new Player(int.Parse(string.Empty + lines[1].Last()));

            Dice dice = new();

            while (true)
            {
                if (Turn(p1, dice, MAX) || Turn(p2, dice, MAX))
                    break;
            }

            return (Math.Min(p1.Score, p2.Score) * dice.Rolls).ToString();
        }

        public string Solution2(string[] lines)
        {
            const int MAX = 21;
            var p1 = new Player(int.Parse(string.Empty + lines[0].Last()));
            var p2 = new Player(int.Parse(string.Empty + lines[1].Last()));

            var universes = new Dictionary<(Player, Player), long>();
            var endedUniverses = new Dictionary<(Player, Player), long>();

            universes.Add((p1, p2), 1);
            var running = universes.Where(e => e.Key.Item1.Score < MAX && e.Key.Item2.Score < MAX);

            do
            {
                var newUniverses = new Dictionary<(Player, Player), long>();
                foreach (var universe in running)
                {
                    foreach (var newU in Dirac(universe.Key.Item1, universe.Key.Item2))
                    {
                        if (newUniverses.ContainsKey(newU.Item1))
                            newUniverses[newU.Item1] += universe.Value * newU.Item2;
                        else
                            newUniverses.Add(newU.Item1, universe.Value * newU.Item2);
                    }
                }
                universes = newUniverses;
                running = universes.Where(e => e.Key.Item1.Score < MAX && e.Key.Item2.Score < MAX);

                foreach (var universe in universes.Where(e => e.Key.Item1.Score >= MAX || e.Key.Item2.Score >= MAX))
                {
                    if (endedUniverses.ContainsKey(universe.Key))
                        endedUniverses[universe.Key] += universe.Value;
                    else
                        endedUniverses.Add(universe.Key, universe.Value);
                }
            } while (running.Any());

            return Math.Max(endedUniverses.Where(e => e.Key.Item1.Score >= MAX).Sum(e => e.Value), endedUniverses.Where(e => e.Key.Item2.Score >= MAX).Sum(e => e.Value)).ToString();
        }

        private static bool Turn(Player player, Dice dice, int max)
            => Turn(player, dice.Roll(), max);

        private static bool Turn(Player player, int diceValue, int max)
        {
            player.Position = ((player.Position + diceValue - 1) % 10) + 1;
            player.Score += player.Position;
            return player.Score >= max;
        }

        private static IEnumerable<((Player, Player), int)> Dirac(Player p1, Player p2)
        {
            foreach (var p1val in DiceValues)
            {
                foreach (var p2val in DiceValues)
                {
                    var np1 = new Player(p1);
                    var np2 = new Player(p2);

                    if (Turn(np1, p1val.Key, 21))
                    {
                        yield return ((np1, np2), p1val.Value);
                        break;
                    }

                    Turn(np2, p2val.Key, 21);

                    yield return ((np1, np2), p1val.Value * p2val.Value);
                }
            }
        }

        private sealed class Dice
        {
            public int Rolls { get; private set; } = 0;
            public int Value { get; private set; } = 0;

            public int Roll()
            {
                const int ROLLS = 3;
                var s = 0;

                for (int i = 0; i < ROLLS; i++)
                {
                    Value %= 100;
                    s += ++Value;
                }
                Rolls += ROLLS;

                return s;
            }
        }

        private sealed class Player : IEquatable<Player>
        {
            public int Position { get; set; }
            public int Score { get; set; }

            public Player(int pos)
            {
                Position = pos;
                Score = 0;
            }

            public Player(Player p)
            {
                Position = p.Position;
                Score = p.Score;
            }

            public bool Equals(Player other)
            {
                return Position == other.Position && Score == other.Score;
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;
                return Equals(obj as Player);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Position, Score);
            }
        }
    }
}
