using System;
using World.DataStructures;
using World.Model;

namespace World.Instance
{
    /// <summary>
    /// Coordinate changed args
    /// </summary>
    public class CoordChangedArgs : EventArgs
    {
        public IntCoord PrevVal { get; private set; }
        public IntCoord NewVal { get; private set; }

        public CoordChangedArgs(IntCoord prevVal, IntCoord newVal)
        {
            PrevVal = prevVal;
            NewVal = newVal;
        }
    }
}
