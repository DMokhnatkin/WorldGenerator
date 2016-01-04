using System;
using World.Model;

namespace World.Instance
{
    /// <summary>
    /// Coordinate changed args
    /// </summary>
    public class CoordChangedArgs : EventArgs
    {
        public ModelCoord PrevVal { get; private set; }
        public ModelCoord NewVal { get; private set; }

        public CoordChangedArgs(ModelCoord prevVal, ModelCoord newVal)
        {
            PrevVal = prevVal;
            NewVal = newVal;
        }
    }
}
