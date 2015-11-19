using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map.Generator.MapModels
{
    /// <summary>
    /// Some extra operations for area
    /// </summary>
    public static class ExtraAreaOperations
    {
        /// <summary>
        /// Calc depth of area
        /// </summary>
        public static byte CalcDepth(this Area cur)
        {
            byte depth = 0;
            Area z = cur;
            while (z.IsSubDivided)
            {
                z = z.LeftTopChild;
                depth++;
            }
            return depth;
        }

        static void FillArrayFromLeftTopCorner(MapVertex[,] arr, Area cur, int i0, int j0, int i1, int j1, int depth, int maxDepth)
        {
            if (!cur.IsSubDivided || depth == maxDepth)
            {
                arr[i0, j0] = cur.LeftTopPoint_Val;
                arr[i0, j1] = cur.RightTopPoint_Val;
                arr[i1, j0] = cur.LeftDownPoint_Val;
                arr[i1, j1] = cur.RightDownPoint_Val;
                return;
            }
            FillArrayFromLeftTopCorner(arr, cur.LeftTopChild, i0, j0, (i1 + i0) / 2, (j1 + j0) / 2, depth + 1, maxDepth);
            FillArrayFromLeftTopCorner(arr, cur.RightTopChild, i0, (j1 + j0) / 2, (i1 + i0) / 2, j1, depth + 1, maxDepth);
            FillArrayFromLeftTopCorner(arr, cur.LeftDownChild, (i1 + i0) / 2, j0, i1, (j1 + j0) / 2, depth + 1, maxDepth);
            FillArrayFromLeftTopCorner(arr, cur.RightDownChild, (i1 + i0) / 2, (j1 + j0) / 2, i1, j1, depth + 1, maxDepth);
        }

        /// <summary>
        /// Build array from the deepest layer of area
        /// </summary>
        public static MapVertex[,] ToArray(this Area area)
        {
            int depth = area.CalcDepth();
            return ToArray(area, depth);
        }

        /// <summary>
        /// Build array from depthLayer
        /// </summary>
        public static MapVertex[,] ToArray(this Area area, int depthLayer)
        {
            // Calculate resolution of area
            int resolution = (int)Math.Pow(2, depthLayer);
            MapVertex[,] res = new MapVertex[resolution + 1, resolution + 1];
            FillArrayFromLeftTopCorner(res, area, 0, 0, resolution, resolution, 0, depthLayer);
            return res;
        }

        /// <summary>
        /// Do some action for each element near top border of area
        /// </summary>
        public static void ForEachTopBorder(this Area area, Action<Area> action)
        {
            if (area.IsSubDivided)
            {
                ForEachTopBorder(area.LeftTopChild, action);
                ForEachTopBorder(area.RightTopChild, action);
            }
            else
                action(area);
        }

        /// <summary>
        /// Do some action for each element near right border of area
        /// </summary>
        public static void ForEachRightBorder(this Area area, Action<Area> action)
        {
            if (area.IsSubDivided)
            {
                ForEachRightBorder(area.RightTopChild, action);
                ForEachRightBorder(area.RightDownChild, action);
            }
            else
                action(area);
        }

        /// <summary>
        /// Do some action for each element near down border of area
        /// </summary>
        public static void ForEachDownBorder(this Area area, Action<Area> action)
        {
            if (area.IsSubDivided)
            {
                ForEachDownBorder(area.LeftDownChild, action);
                ForEachDownBorder(area.RightDownChild, action);
            }
            else
                action(area);
        }

        /// <summary>
        /// Do some action for each element near left border of area
        /// </summary>
        public static void ForEachLeftBorder(this Area area, Action<Area> action)
        {
            if (area.IsSubDivided)
            {
                ForEachLeftBorder(area.LeftTopChild, action);
                ForEachLeftBorder(area.LeftDownChild, action);
            }
            else
                action(area);
        }
    }
}
