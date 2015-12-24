using System;
using System.Collections.Generic;
using World.Common;

namespace World.Model.Frames
{
    /// <summary>
    /// Square frame with size = 2^n + 1
    /// </summary>
    public class BinPlus1SquareFrame
    {
        public ModelCoord LeftDown { get; private set; }
        public int Size { get; private set; }

        public BinPlus1SquareFrame(ModelCoord leftDown, int size)
        {
            if (Pow2.GetLog2(size - 1) == -1)
                throw new ArgumentException("Size must be 2^n + 1");
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

        /// <summary>
        /// Get center coord of frame
        /// </summary>
        public ModelCoord Center { get { return (new ModelCoord(LeftDown.x + Size / 2, LeftDown.y + Size / 2)); } }

        /// <summary>
        /// Get center point of top edge
        /// </summary>
        public ModelCoord TopEdgeCenter { get { return new ModelCoord(LeftDown.x + Size / 2, LeftDown.y + Size - 1); } }

        /// <summary>
        /// Get center point of right edge
        /// </summary>
        public ModelCoord RightEdgeCenter { get { return new ModelCoord(LeftDown.x + Size - 1, LeftDown.y + Size / 2); } }

        /// <summary>
        /// Get center point of down edge
        /// </summary>
        public ModelCoord DownEdgeCenter { get { return new ModelCoord(LeftDown.x + Size / 2, LeftDown.y); } }

        /// <summary>
        /// Get center point for left edge
        /// </summary>
        public ModelCoord LeftEdgeCenter { get { return new ModelCoord(LeftDown.x, LeftDown.y + Size / 2); } }

        /// <summary>
        /// Left top corner of frame
        /// </summary>
        public ModelCoord LeftTopCorner { get { return new ModelCoord(LeftDown.x, LeftDown.y + Size - 1); } }

        /// <summary>
        /// Right top corner of frame
        /// </summary>
        public ModelCoord RightTopCorner { get { return new ModelCoord(LeftDown.x + Size - 1, LeftDown.y + Size - 1); } }

        /// <summary>
        /// Get left down corner
        /// </summary>
        public ModelCoord LeftDownCorner { get { return new ModelCoord(LeftDown.x, LeftDown.y); } }

        /// <summary>
        /// Get right down corner
        /// </summary>
        public ModelCoord RightDownCorner { get { return new ModelCoord(LeftDown.x + Size - 1, LeftDown.y); } }

        /// <summary>
        /// Get left top quarter of frame
        /// </summary>
        public BinPlus1SquareFrame LeftTopQuarter { get { return new BinPlus1SquareFrame(LeftEdgeCenter, Size / 2 + 1); } }

        /// <summary>
        /// Get right top quarter of frame
        /// </summary>
        public BinPlus1SquareFrame RightTopQuarter { get { return new BinPlus1SquareFrame(Center, Size / 2 + 1); } }

        /// <summary>
        /// Get left down quarter of frame
        /// </summary>
        public BinPlus1SquareFrame LeftDownQuarter { get { return new BinPlus1SquareFrame(LeftDown, Size / 2 + 1); } }

        /// <summary>
        /// Get right down quarter of frame
        /// </summary>
        public BinPlus1SquareFrame RightDownQuarter { get { return new BinPlus1SquareFrame(DownEdgeCenter, Size / 2 + 1); } }

        public override string ToString()
        {
            return LeftDown.ToString() + " size = " + Size;
        }
    }
}
