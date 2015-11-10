using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map.Generator.CommonMap
{
    public class MapModel
    {
        MapChunk[,] chunks = new MapChunk[10, 10];

        public readonly int _maxChunkResolution;

        public MapModel(int maxChunkResolution)
        {
            if (Math.Log(maxChunkResolution, 2) != Math.Ceiling(Math.Log(maxChunkResolution, 2)))
                throw new ArgumentException("maxChunkResolution must be Pow of 2");
            _maxChunkResolution = maxChunkResolution;
            DiamondSquare sq = new DiamondSquare();
            HeightMap map = new HeightMap();
            sq.ExtendResolution(map, _maxChunkResolution);
            chunks[5, 5] = new MapChunk(map);
        }

        public MapChunk GetChunk(int x, int y)
        {
            int i = chunks.GetLength(0) / 2 + x;
            int j = chunks.GetLength(1) / 2 + y;
            return chunks[i, j];
        }

        /// <summary>
        /// Join top neighbor
        /// </summary>
        /// <param name="cur"></param>
        /// <param name="neighbor"></param>
        void JoinTopNeighbor(SubdividedPlane.Node<MapVertex> cur, SubdividedPlane.Node<MapVertex> neighbor)
        {
            if (!cur.IsDivided)
            {
                neighbor.LeftDownPoint_Val.Copy(cur.LeftTopPoint_Val);
                neighbor.RightDownPoint_Val.Copy(cur.RightTopPoint_Val);
                return;
            }
            neighbor.Subdivide();
            JoinTopNeighbor(cur.LeftTopChild, neighbor.LeftDownChild);
            JoinTopNeighbor(cur.RightTopChild, neighbor.RightDownChild);
        }

        public void AddTopNeighbor(int x, int y)
        {
            MapChunk _cur = GetChunk(x, y);
            MapChunk _top = new MapChunk(new HeightMap());
            chunks[x, y + 1] = _top;
            JoinTopNeighbor(_cur.heightMap.val.Root, _top.heightMap.val.Root);
            DiamondSquare sq = new DiamondSquare();
            sq.ExtendResolution(_top.heightMap, _maxChunkResolution);
        }
    }
}
