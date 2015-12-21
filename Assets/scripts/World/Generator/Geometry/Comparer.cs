using System;

namespace World.Generator.Geometry
{
    public static class Comparer
    {
        public static int Compare(float a, float b, float eps)
        {
            if (Math.Abs(a - b) < eps)
                return 0;
            if (a > b)
                return 1;
            else
                return -1; 
        }
    }
}
