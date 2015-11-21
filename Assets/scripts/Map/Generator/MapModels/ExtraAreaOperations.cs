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

        /// <summary>
        /// Return (if not exists will be created) top neighborg
        /// </summary>
        public static Area GetOrCreateTopNeighbor(this Area area)
        {
            if (area.TopNeighbor == null)
                area.CreateTopNeighbor();
            return area.TopNeighbor;
        }

        /// <summary>
        /// Return (if not exists will be created) right neighborg
        /// </summary>
        public static Area GetOrCreateRightNeighbor(this Area area)
        {
            if (area.RightNeighbor == null)
                area.CreateRightNeighbor();
            return area.RightNeighbor;
        }

        /// <summary>
        /// Return (if not exists will be created) down neighborg
        /// </summary>
        public static Area GetOrCreateDownNeighbor(this Area area)
        {
            if (area.DownNeighbor == null)
                area.CreateDownNeighbor();
            return area.DownNeighbor;
        }

        /// <summary>
        /// Return (if not exists will be created) left neighborg
        /// </summary>
        public static Area GetOrCreateLeftNeighbor(this Area area)
        {
            if (area.LeftNeighbor == null)
                area.CreateLeftNeighbor();
            return area.LeftNeighbor;
        }

        /// <summary>
        /// Create areas around cur
        /// Will be created (2 * radius + 1) * (2 * radius + 1) areas
        /// </summary>
        public static void CreateAreasAround(this Area area, int radius)
        {
            Area cur = area;
            int i = 0;
            // Move cur to the left
            for (i = 0; i >= -radius; i--)
                cur = cur.GetOrCreateLeftNeighbor();
            for (; i <= radius; i++)
            {
                // Create top areas
                Area vertCur = cur;
                for (int j = 0; j <= radius; j++)
                    vertCur = vertCur.GetOrCreateTopNeighbor();
                // Create down areas
                vertCur = cur;
                for (int j = 0; j >= -radius; j--)
                    vertCur = vertCur.GetOrCreateDownNeighbor();
                cur = cur.GetOrCreateRightNeighbor();
            }
        }

        /// <summary>
        /// Get areas around
        /// Returns matrix (2 * radius + 1) * (2 * radius + 1) of areas around current
        /// All areas must be created before!
        /// </summary>
        /// <param name="radius">Radius to get areas</param>
        /// <param name="createIfNotExists">Should areas be created if not exists</param>
        public static Area[,] GetAreasAround(this Area area, int radius)
        {
            Area[,] res = new Area[2 * radius + 1, 2 * radius + 1];
            Area cur = area;
            int j = 0;
            // Move cur to the left
            for (j = 0; j > -radius; j--)
            {
                if (cur == null)
                    throw new ArgumentException("All areas around in GetAreasAround must be created before");
                cur = cur.LeftNeighbor;
            }
            for (j = -radius; j <= radius; j++)
            {
                // Create top areas
                Area vertCur = cur;
                for (int i = 0; i <= radius; i++)
                {
                    if (vertCur == null)
                        throw new ArgumentException("All areas around in GetAreasAround must be created before");
                    res[radius - i, j + radius] = vertCur;
                    vertCur = vertCur.TopNeighbor;
                }
                // Create down areas
                vertCur = cur;
                for (int i = 0; i >= -radius; i--)
                {
                    if (vertCur == null)
                        throw new ArgumentException("All areas around in GetAreasAround must be created before");
                    res[radius - i, j + radius] = vertCur;
                    vertCur = vertCur.DownNeighbor;
                }
                cur = cur.RightNeighbor;
                if (cur == null)
                    throw new ArgumentException("All areas around in GetAreasAround must be created before");
            }
            return res;
        }
    }
}
