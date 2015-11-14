using System;
using System.Collections.Generic;
using Map.Generator.MapModels;

namespace Map.Generator
{

    public class HeightMap
    {
        static void FillArray(MapVertex[,] arr, Area cur, int i0, int j0, int i1, int j1)
        {
            if (!cur.IsSubDivided)
            {
                arr[i0, j0] = cur.LeftTopPoint_Val;
                arr[i0, j1] = cur.RightTopPoint_Val;
                arr[i1, j0] = cur.LeftDownPoint_Val;
                arr[i1, j1] = cur.RightDownPoint_Val;
                return;
            }
            FillArray(arr, cur.LeftTopChild, i0, j0, (i1 + i0) / 2, (j1 + j0) / 2);
            FillArray(arr, cur.RightTopChild, i0, (j1 + j0) / 2, (i1 + i0) / 2, j1);
            FillArray(arr, cur.LeftDownChild, (i1 + i0) / 2, j0, i1, (j1 + j0) / 2);
            FillArray(arr, cur.RightDownChild, (i1 + i0) / 2, (j1 + j0) / 2, i1, j1);
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
            FillArray(res, area, 0, 0, resolution, resolution);
            return res;
        }

        /*
        /// <summary>
        /// Current resolution (Count of vertices in width - 1)
        /// </summary>
        public int resolution = 1;

        public SubdividedPlane<MapVertex> val = new SubdividedPlane<MapVertex>(new MapVertex() { height = 0 });

        void FillArray(MapVertex[,] arr, Node<MapVertex> cur, int i0, int j0, int i1, int j1)
        {
            if (!cur.IsSubDivided)
            {
                arr[i0, j0] = cur.LeftTopPoint_Val;
                arr[i0, j1] = cur.RightTopPoint_Val;
                arr[i1, j0] = cur.LeftDownPoint_Val;
                arr[i1, j1] = cur.RightDownPoint_Val;
                return;
            }
            FillArray(arr, cur.LeftTopChild, i0, j0, (i1 + i0) / 2, (j1 + j0) /2);
            FillArray(arr, cur.RightTopChild, i0, (j1 + j0) / 2, (i1 + i0) / 2, j1);
            FillArray(arr, cur.LeftDownChild, (i1 + i0) / 2, j0, i1, (j1 + j0) / 2);
            FillArray(arr, cur.RightDownChild, (i1 + i0) / 2, (j1 + j0) / 2, i1, j1);
        }

        void CollectTopEdgePoints(LinkedList<MapVertex> res, Node<MapVertex> cur)
        {
            if (cur.IsSubDivided)
            {
                CollectTopEdgePoints(res, cur.LeftTopChild);
                CollectTopEdgePoints(res, cur.RightTopChild);
            }
            else
                res.AddLast(cur.LeftTopPoint_Val);
        }

        public IEnumerator<MapVertex> TopEdgePoints()
        {
            LinkedList<MapVertex> res = new LinkedList<MapVertex>();
            CollectTopEdgePoints(res, val.Root);
            // We ignored one point, lets add it
            res.AddLast(val.Root.RightTopPoint_Val);
            return res.GetEnumerator();
        }

        void CollectDownEdgePoints(LinkedList<MapVertex> res, Node<MapVertex> cur)
        {
            if (cur.IsSubDivided)
            {
                CollectTopEdgePoints(res, cur.LeftDownChild);
                CollectTopEdgePoints(res, cur.RightDownChild);
            }
            else
                res.AddLast(cur.LeftDownPoint_Val);
        }

        public LinkedList<MapVertex> DownEdgePoints()
        {
            LinkedList<MapVertex> res = new LinkedList<MapVertex>();
            CollectDownEdgePoints(res, val.Root);
            // We ignored one point, lets add it
            res.AddLast(val.Root.RightDownPoint_Val);
            return res;
        }

        public MapVertex[,] ToArray()
        {
            MapVertex[,] res = new MapVertex[resolution + 1, resolution + 1];
            FillArray(res, val.Root, 0, 0, resolution, resolution);
            return res;
        }*/
    }
}
