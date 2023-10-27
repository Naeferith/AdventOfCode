namespace Nae.Utils.Extensions
{
    public static class ArrayExtensions
    {
        public static bool IsPointWithinMatrix(this Array array, params int[] indicies)
        {
            if (array.Rank != indicies.Length)
            {
                throw new RankException(nameof(indicies));
            }

            for (int i = 0; i < array.Rank; i++)
            {
                if (indicies[i] < array.GetLowerBound(i) || indicies[i] > array.GetUpperBound(i))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
