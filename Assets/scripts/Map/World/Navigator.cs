using UnityEngine;
using System.Collections.Generic;
using Map.MapModels.Common;

namespace Map.World
{
    public class Navigator
    {
        Vector2 basePos;
        float baseSize;

        /// <param name="basePos">Position which is zero</param>
        /// <param name="baseSize">Size of max detalized chunk</param>
        public Navigator(Vector2 basePos, float baseSize)
        {
            this.basePos = basePos;
            this.baseSize = baseSize;
        }

        public Vector2 GetCellLeftDownPos(Coord cellCoord)
        {
            return (basePos + new Vector2((cellCoord.x + 0.5f) * baseSize, (cellCoord.y + 0.5f) * baseSize));
        }

        /// <summary>
        /// Get relative to cell coord for pos
        /// 0 - pos is in cell
        /// 1 - pos is upper cell
        /// 2 - pos if to the right of cell
        /// 3 - pos is under cell
        /// 4 - pos is to the left of cell
        /// </summary>
        public int CellContains(Vector2 pos, Coord cellCoord)
        {
            Vector2 leftDownCorner = GetCellLeftDownPos(cellCoord);
            Vector2 rightTopCorner = leftDownCorner + new Vector2(baseSize, baseSize);
            if (pos.y > rightTopCorner.y)
                return 1;
            if (pos.x > rightTopCorner.x)
                return 2;
            if (pos.y < leftDownCorner.y)
                return 3;
            if (pos.x < leftDownCorner.x)
                return 4;
            return 0;
        }
    }
}
