using System;
using System.Linq;

namespace InstagramNameChecker
{
    static class Utils
    {
        internal const string DefaultPassForNewAccounts = "acceptMe123";
        internal const string FilePath = "Wordlist.txt";

        public static string GetRandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="min">The inclusive lower bound of the random number returned.</param>
        /// <param name="max">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns> A 32-bit signed integer greater than or equal to minValue and less than maxValue;
        /// that is, the range of return values includes minValue but not maxValue. If minValue
        /// equals maxValue, minValue is returned.</returns>
        public static int GetRandomInt(int min, int max)
        {
            Random rand = new Random();
            return rand.Next(min,max);
        }
    }
}
