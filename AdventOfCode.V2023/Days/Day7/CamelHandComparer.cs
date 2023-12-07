namespace AdventOfCode.V2023.Days.Day7
{
    public class CamelHandComparer : IComparer<string>
    {
        private static readonly List<string> HAND_VALS = ["11111", "2111", "221", "311", "32", "41", "5"];
        private const string CARD_VALS = "23456789TJQKA";
        private const char JOKER_TOKEN = 'J';
        private readonly bool _handleJoker;

        public CamelHandComparer(bool handleJoker)
        {
            _handleJoker = handleJoker;
        }

        public int Compare(string? x, string? y)
        {
            var cX = ToHandValue(x!);
            var cY = ToHandValue(y!);

            if (HAND_VALS.IndexOf(cX) < HAND_VALS.IndexOf(cY))
            {
                return -1;
            }

            if (HAND_VALS.IndexOf(cX) > HAND_VALS.IndexOf(cY))
            {
                return 1;
            }

            for (int i = 0; i < 5; i++)
            {
                if (x![i] != y![i])
                {
                    var vX = ToCardValue(x![i]);
                    var vY = ToCardValue(y![i]);

                    return vX.CompareTo(vY);
                }
            }

            return 0;
        }

        private int ToCardValue(char c)
        {
            return _handleJoker && c == JOKER_TOKEN
                ? -1
                : CARD_VALS.IndexOf(c);
        }

        private string ToHandValue(string hand)
        {
            var groups = hand
                .GroupBy(c => c)
                .OrderByDescending(g => g.Count())
                .ToDictionary(g => g.Key, g => g.Count());

            if (_handleJoker && hand.Contains(JOKER_TOKEN) && groups.Count > 1)
            {
                groups[groups.First(kvp => kvp.Key != JOKER_TOKEN).Key] += groups[JOKER_TOKEN];
                groups.Remove(JOKER_TOKEN);
            }
            return string.Concat(groups.Select(kvp => kvp.Value.ToString()));
        }
    }
}
