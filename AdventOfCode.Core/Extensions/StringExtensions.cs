namespace AdventOfCode.Core.Extensions
{
    /// <summary>
    /// Provides extensions methods for <see cref="string"/>
    /// </summary>
    public static class StringExtensions
    {

        /// <summary>
        /// Format a string to center it in a bugger space
        /// </summary>
        /// <param name="str">String to be centered</param>
        /// <param name="length">Maximum length</param>
        /// <returns>The string formated.</returns>
        public static string PadCenter(this string str, int length)
        {
            int spaces = length - str.Length;
            int padLeft = spaces / 2 + str.Length;
            return str.PadLeft(padLeft).PadRight(length);
        }
    }
}
