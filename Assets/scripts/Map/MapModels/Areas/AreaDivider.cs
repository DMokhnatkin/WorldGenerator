using System;
using System.Collections.Generic;
using Map.MapModels.Areas;

namespace Map.MapModels.Areas
{
    public static class AreaDivider
    {
        /// <summary>
        /// Divide into 4 nodes
        /// </summary>
        public static void Divide(this Area area)
        {
            if (area.LeftTopChild == null)
                area.CreateLeftTopChild();
            if (area.RightTopChild == null)
                area.CreateRightTopChild();
            if (area.LeftDownChild == null)
                area.CreateLeftDownChild();
            if (area.RightDownChild == null)
                area.CreateRightDownChild();
        }

        private static void _Divide(this Area cur, int curDepth, byte maxDepth)
        {
            cur.Divide();
            if (curDepth < maxDepth)
            {
                _Divide(cur.LeftTopChild, curDepth + 1, maxDepth);
                _Divide(cur.RightTopChild, curDepth + 1, maxDepth);
                _Divide(cur.LeftDownChild, curDepth + 1, maxDepth);
                _Divide(cur.RightDownChild, curDepth + 1, maxDepth);
            }
        }

        /// <summary>
        /// Divide several times
        /// </summary>
        public static void Divide(this Area area, byte depth)
        {
            _Divide(area, 0, depth);
        }
    }
}
