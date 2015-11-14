using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map.Generator.Algorithms
{
    public class PerlinNoise
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static float Dot(float[] a, float[] b)
        {
            return a[0] * b[0] + a[1] * b[1];
        }
    }
}
