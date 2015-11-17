using System;
using System.Collections.Generic;
using Map.Generator.MapModels;

namespace Map.Generator
{

    public class HeightMap
    {
        static void FillArrayFromLeftTopCorner(MapVertex[,] arr, Area cur, int i0, int j0, int i1, int j1)
        {
            if (!cur.IsSubDivided)
            {
                arr[i0, j0] = cur.LeftTopPoint_Val;
                arr[i0, j1] = cur.RightTopPoint_Val;
                arr[i1, j0] = cur.LeftDownPoint_Val;
                arr[i1, j1] = cur.RightDownPoint_Val;
                return;
            }
            FillArrayFromLeftTopCorner(arr, cur.LeftTopChild, i0, j0, (i1 + i0) / 2, (j1 + j0) / 2);
            FillArrayFromLeftTopCorner(arr, cur.RightTopChild, i0, (j1 + j0) / 2, (i1 + i0) / 2, j1);
            FillArrayFromLeftTopCorner(arr, cur.LeftDownChild, (i1 + i0) / 2, j0, i1, (j1 + j0) / 2);
            FillArrayFromLeftTopCorner(arr, cur.RightDownChild, (i1 + i0) / 2, (j1 + j0) / 2, i1, j1);
        }

        private static int CalcHeight(Area cur)
        {
            if (!cur.IsSubDivided)
                return 0;
            return CalcHeight(cur.LeftDownChild) + 1;
        }

        public static MapVertex[,] AreaToArray(Area area)
        {
            // Calculate resolution of area
            int resolution = (int)Math.Pow(2, CalcHeight(area));
            MapVertex[,] res = new MapVertex[resolution + 1, resolution + 1];
            FillArrayFromLeftTopCorner(res, area, 0, 0, resolution, resolution);
            return res;
        }
    }
}
