using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using World.Common;

namespace World.Model.Frames
{
    public class BinSquareFrame
    {
        public ModelCoord LeftDown { get; private set; }
        public int Size { get; private set; }

        public BinSquareFrame(ModelCoord leftDown, int size)
        {
            if (Pow2.GetLog2(size) == -1)
                throw new ArgumentException("Size be must power of 2");
            LeftDown = leftDown;
            Size = size;
        }

        public IEnumerable<ModelCoord> GetCoords()
        {
            for (int x = LeftDown.x; x <= LeftDown.x + Size; x++)
                for (int y = LeftDown.y; y <= LeftDown.y + Size; y++)
                {
                    yield return new ModelCoord(x, y);
                }
        }
    }
}
