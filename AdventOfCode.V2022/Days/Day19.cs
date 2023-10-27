using AdventOfCode.Core.Components;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.V2022.Days
{
    internal class Day19 : IDay
    {
        private sealed record Blueprint(int OreOreCost, int ClayOreCost, int ObsidianOreCost, int ObsidianClayCost, int GeodeOreCost, int GeodeObsidianCost);
        private sealed record State(int Iteration, int OreCount, int OreIncr, int ClayCount, int ClayIncr, int ObsCount, int ObsIncr);

        private Blueprint[]? _blueprints;

        private readonly IDictionary<State, int> States;

        public int DayNumber => 19;

        public string PuzzleName => "Enough Minerals";

        public Day19()
        {
            States = new Dictionary<State, int>();
        }

        /// <summary>
        /// Awefully slow.
        /// Extremely memory inefficient.
        /// Still managed to run it.
        /// 
        /// 10/10 will never run it again.
        /// 
        /// </summary>
        private int Compute(Blueprint bp, State state)
        {
            if (state.Iteration == 0)
            {
                return 0;
            }

            if (state.OreCount >= bp.GeodeOreCost && state.ObsCount >= bp.GeodeObsidianCost)
            {
                return state.Iteration - 1 + Memoize(
                    bp, new(
                    state.Iteration - 1,
                    state.OreCount + state.OreIncr - bp.GeodeOreCost,
                    state.OreIncr,
                    state.ClayCount + state.ClayIncr,
                    state.ClayIncr,
                    state.ObsCount + state.ObsIncr - bp.GeodeObsidianCost,
                    state.ObsIncr));
            }

            var result = 0;

            if (state.OreCount >= bp.OreOreCost)
            {
                result = Math.Max(result, Memoize(
                    bp, new(
                    state.Iteration - 1,
                    state.OreCount + state.OreIncr - bp.OreOreCost,
                    state.OreIncr + 1,
                    state.ClayCount + state.ClayIncr,
                    state.ClayIncr,
                    state.ObsCount + state.ObsIncr,
                    state.ObsIncr)));
            }

            if (state.OreCount >= bp.ClayOreCost)
            {
                result = Math.Max(result, Memoize(
                    bp, new(
                    state.Iteration - 1,
                    state.OreCount + state.OreIncr - bp.ClayOreCost,
                    state.OreIncr,
                    state.ClayCount + state.ClayIncr,
                    state.ClayIncr + 1,
                    state.ObsCount + state.ObsIncr,
                    state.ObsIncr)));
            }

            if (state.OreCount >= bp.ObsidianOreCost && state.ClayCount >= bp.ObsidianClayCost)
            {
                result = Math.Max(result, Memoize(
                    bp, new(
                    state.Iteration - 1,
                    state.OreCount + state.OreIncr - bp.ObsidianOreCost,
                    state.OreIncr,
                    state.ClayCount + state.ClayIncr - bp.ObsidianClayCost,
                    state.ClayIncr,
                    state.ObsCount + state.ObsIncr,
                    state.ObsIncr + 1)));
            }

            return Math.Max(result, Memoize(
                    bp, new(
                    state.Iteration - 1,
                    state.OreCount + state.OreIncr,
                    state.OreIncr,
                    state.ClayCount + state.ClayIncr,
                    state.ClayIncr,
                    state.ObsCount + state.ObsIncr,
                    state.ObsIncr)));
        }

        private int Memoize(Blueprint bp, State state)
        {
            if (States.TryGetValue(state, out var res))
            {
                return res;
            }
            else
            {
                var result = Compute(bp, state);

                if (state.Iteration > 0)
                {
                    States[state] = result;
                }

                return result;
            }
        }

        [MemberNotNull(nameof(_blueprints))]
        private void Initialize(string[] lines)
        {
            _blueprints = new Blueprint[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                var ints = lines[i]
                    .Split(' ')
                    .Select(s => new { isInt = int.TryParse(s, out var i), num = i })
                    .Where(i => i.isInt)
                    .Select(i => i.num)
                    .ToArray();

                _blueprints[i] = new(ints[0], ints[1], ints[2], ints[3], ints[4], ints[5]);
            }
        }

        public string Solution1(string[] lines)
        {
            Initialize(lines);
            return _blueprints
                .Select((b, i) =>
                {
                    var result = Compute(b, new(24, 0, 1, 0, 0, 0, 0));
                    States.Clear();
                    return (i + 1) * result;
                })
                .Sum()
                .ToString();
        }

        public string Solution2(string[] lines)
        {
            Initialize(lines);
            return _blueprints
                .Take(3)
                .Select(b =>
                {
                    var result = Compute(b, new(32, 0, 1, 0, 0, 0, 0));
                    States.Clear();
                    return result;
                })
                .Aggregate(1, (a, b) => a * b)
                .ToString();
        }
    }
}
