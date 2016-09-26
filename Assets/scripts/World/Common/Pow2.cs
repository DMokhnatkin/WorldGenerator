using System;
using System.Collections.Generic;

namespace World.Common
{
    /// <summary>
    /// Int pows of 2 with cash
    /// </summary>
    public static class Pow2
    {
        public const float SQRT_2 = 1.414213562373095f;

        static List<int> cashPow2 = new List<int>();

        static Dictionary<int, int> cashLog2 = new Dictionary<int, int>();

        static Pow2()
        {
            cashPow2.Add(1);
            cashLog2.Add(1, 0);
        }

        static void CalcNext()
        {
            cashPow2.Add(cashPow2[cashPow2.Count - 1] * 2);
            cashLog2.Add(cashPow2[cashPow2.Count - 1], cashPow2.Count - 1);
        }

        /// <summary>
        /// Get 2^step
        /// </summary>
        public static int GetPow2(int step)
        {
            while (cashPow2.Count - 1 < step)
            {
                CalcNext();
            }
            return cashPow2[step];
        }

        /// <summary>
        /// Get log with base = 2. If val is not power of 2, will be returned -1
        /// </summary>
        public static int GetLog2(int val)
        {
            while (cashPow2[cashPow2.Count - 1] < val)
            {
                CalcNext();
            }
            if (cashLog2.ContainsKey(val))
                return cashLog2[val];
            else
                return -1;
        }

        /// <summary>
        /// Ceil float to nearest bigger step of 2
        /// </summary>
        public static int CeilToPow2(float val)
        {
            return GetPow2((int)Math.Ceiling(Math.Log(val, 2)));
        }
    }
}
