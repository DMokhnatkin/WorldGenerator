using System;
using World.Common;

namespace World.Model.Frames
{
    /// <summary>
    /// Square frame which size is power of 2
    /// </summary>
    public class BinSquareFrame : SquareFrame
    {
        public BinSquareFrame(ModelCoord leftDown, int size) : base(leftDown, size)
        {
            if (Pow2.GetLog2(size) == -1)
                throw new ArgumentException("Size be must power of 2");
        }
    }
}
