using System;
using System.Collections.Generic;

namespace World.Model.Frames
{
    /// <summary>
    /// Square frame
    /// </summary>
    public class SquareFrame : IFrame, IEqualityComparer<SquareFrame>
    {
        /// <summary>
        /// Left down corner of frame
        /// </summary>
        public ModelCoord LeftDown { get; protected set; }
        /// <summary>
        /// Size of frame
        /// </summary>
        public int Size { get; protected set; }

        /// <summary>
        /// Get center of frame. If center is float, will be rounded.
        /// </summary>
        public ModelCoord Center
        {
            get { return new ModelCoord(LeftDown.x + Size / 2, LeftDown.y + Size / 2); }
        }

        public int LeftBorder
        {
            get { return LeftDown.x; }
        }

        public int RightBorder
        {
            get { return LeftDown.x + Size - 1; }
        }

        public int TopBorder
        {
            get { return LeftDown.y + Size - 1; }
        }

        public int DownBorder
        {
            get { return LeftDown.y; }
        }

        public SquareFrame(ModelCoord leftDown, int size)
        {
            LeftDown = leftDown;
            Size = size;
        }

        /// <summary>
        /// Get coordinates which this frame contains
        /// </summary>
        public IEnumerable<ModelCoord> GetCoords()
        {
            int rightX = LeftDown.x + Size;
            int topY = LeftDown.y + Size;
            for (int x = LeftDown.x; x < rightX; x++)
                for (int y = LeftDown.y; y < topY; y++)
                {
                    yield return new ModelCoord(x, y);
                }
        }

        public override int GetHashCode()
        {
            return (LeftDown.x * LeftDown.y + LeftDown.x) * Size;
        }

        public override string ToString()
        {
            return String.Format("SquareFrame leftDown = {0} size = {1}", LeftDown, Size);
        }

        public bool Equals(SquareFrame x, SquareFrame y)
        {
            return (x.GetHashCode() == y.GetHashCode()) &&
                (x.LeftDown == y.LeftDown) &&
                (x.Size == y.Size);
        }

        public int GetHashCode(SquareFrame obj)
        {
            return GetHashCode();
        }

        public bool Contains(ModelCoord coord)
        {
            return (coord.x >= LeftDown.x &&
                coord.x < LeftDown.x + Size &&
                coord.y >= LeftDown.y &&
                coord.y < LeftDown.y + Size);
        }
    }
}
