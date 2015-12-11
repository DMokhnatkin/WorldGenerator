using System;
using Map.MapModels.Areas;
using Map.MapModels.Points;

namespace Map.MapModels.Extensions
{
    public static class ModelUnwrap
    {
        static void FillArrayFromLeftTopCorner(MapPoint[,] arr, Area cur, int i0, int j0, int i1, int j1, int depth, int maxDepth)
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
        public static MapPoint[,] UnwrapPoints(this Area area)
        {
            int depth = area.CalcDepth();
            return UnwrapPoints(area, depth);
        }

        /// <summary>
        /// Build array from depthLayer
        /// </summary>
        public static MapPoint[,] UnwrapPoints(this Area area, int depthLayer)
        {
            // Calculate resolution of area
            int resolution = (int)Math.Pow(2, depthLayer);
            MapPoint[,] res = new MapPoint[resolution + 1, resolution + 1];
            FillArrayFromLeftTopCorner(res, area, 0, 0, resolution, resolution, 0, depthLayer);
            return res;
        }
    }
}
