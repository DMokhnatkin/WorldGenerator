using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map.Generator.Geometry
{
    public static class Slope
    {
        public static float CalcMaxSlope(float cur, float? top, float? right, float? down, float? left)
        {
            if (top == null)
                top = cur;
            if (right == null)
                right = cur;
            if (down == null)
                down = cur;
            if (left == null)
                left = cur;
            return Math.Max(
                Math.Max(Math.Abs(top.Value - cur), Math.Abs(right.Value - cur)),
                Math.Max(Math.Abs(down.Value - cur), Math.Abs(left.Value - cur)));
        }
    }
}
