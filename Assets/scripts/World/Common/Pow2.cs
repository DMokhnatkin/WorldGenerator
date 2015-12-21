using System.Collections.Generic;

namespace World.Common
{
    /// <summary>
    /// Int pows of 2 with cash
    /// </summary>
    public static class Pow2
    {
        static List<int> cash = new List<int>();

        static Pow2()
        {
            cash.Add(1);
        }

        /// <summary>
        /// Get 2^step
        /// </summary>
        public static int GetPow2(int step)
        {
            while (cash.Count - 1 < step)
            {
                cash.Add(cash[cash.Count - 1] * 2);
            }
            return cash[step];
        }
    }
}
