using System;
using System.Collections.Generic;
using Map.Generator.SubdividedPlane;

namespace Map.Generator
{
    public class MapVertex : ICloneable
    {
        public float height;

        public object Clone()
        {
            MapVertex res = new MapVertex();
            res.height = height;
            return res;
        }

        /// <summary>
        /// Copy all values
        /// </summary>
        /// <param name="other"></param>
        public void Copy(MapVertex other)
        {
            this.height = other.height;
        }
    }

    public class HeightMap
    {
        /// <summary>
        /// Current resolution (Count of vertices in width - 1)
        /// </summary>
        public int resolution = 1;

        public SubdividedPlane<MapVertex> val = new SubdividedPlane<MapVertex>(new MapVertex() { height = 0 });

        void FillArray(MapVertex[,] arr, Node<MapVertex> cur, int i0, int j0, int i1, int j1)
        {
            if (!cur.IsDivided)
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
            if (cur.IsDivided)
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
            if (cur.IsDivided)
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
        }
    }
}
