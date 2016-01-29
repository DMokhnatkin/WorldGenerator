using System;
using System.Collections.Generic;

namespace World.DataStructures
{
    /// <summary>
    /// Represents coordinate(x,y) where x and y are integers
    /// </summary>
    public struct IntCoord : IEqualityComparer<IntCoord>
    {
        public readonly int x;
        public readonly int y;

        public IntCoord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            return x == ((IntCoord)obj).x && y == ((IntCoord)obj).y;
        }

        public override int GetHashCode()
        {
            return x * y + x;
        }

        public bool Equals(IntCoord x, IntCoord y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(IntCoord obj)
        {
            return obj.GetHashCode();
        }

        /// <summary>
        /// Top neighbor
        /// </summary>
        public IntCoord Top
        {
            get { return new IntCoord(x, y + 1); }
        }

        /// <summary>
        /// Right neighbor
        /// </summary>
        public IntCoord Right
        {
            get { return new IntCoord(x + 1, y); }
        }

        /// <summary>
        /// Down neighbor
        /// </summary>
        public IntCoord Down
        {
            get { return new IntCoord(x, y - 1); }
        }

        /// <summary>
        /// Left neighbor
        /// </summary>
        public IntCoord Left
        {
            get { return new IntCoord(x - 1, y); }
        }

        public override string ToString()
        {
            return x + " " + y;
        }
    }
}
