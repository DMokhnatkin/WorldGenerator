using System;
using System.Collections.Generic;
using World.Common;

namespace World.DataStructures.ChunksGrid
{
    /// <summary>
    /// Implements access in detalization layers
    /// 0 layer - max detalization
    /// 1 layer - max / 2 detalization
    /// 2 layer - max / 4 detalization
    /// n layer - max / (2^n) detalization
    /// max(n) = lb(chunk.size - 1) + 1
    /// </summary>
    /// <typeparam name="T">Type of data which stores in collection</typeparam>
    public class DetalizationAccessor
    {
        /// <summary>
        /// Count of detalization layers. (2^detalizationLayersCount + 1 = chunkSize)
        /// </summary>
        public readonly int detalizationLayersCount;

        /// <summary>
        /// Calculate count of detalization layers for specifed chunk size
        /// </summary>
        public static int CalcLayersCount(int chunkSize)
        {
            if (Pow2.GetLog2(chunkSize - 1) == -1)
                throw new ArgumentException("Invalid chunk size (must be 2^n + 1)");
            return Pow2.GetLog2(chunkSize - 1) + 1;
        }

        public DetalizationAccessor(ChunksNavigator chunksNavigator)
        {
            detalizationLayersCount = CalcLayersCount(chunksNavigator.chunkSize);
        }

        /// <summary>
        /// Get offset of one coordinate in specifed layer
        /// </summary>
        public int GetCoordOffsetInLayer(int layerId)
        {
            return Pow2.GetPow2(detalizationLayersCount - layerId - 1);
        }

        /// <summary>
        /// Get base coordinates which contains specifed chunk in specifed layer
        /// </summary>
        /// <param name="extendChunk">Include some points around chunk (extend chunk width)</param>
        public IEnumerable<IntCoord> GetBaseCoordsInLayer(Chunk chunk, int layerId, int extendChunk = 0)
        {
            int offset = GetCoordOffsetInLayer(layerId);
            for (int y = chunk.DownBorder - offset * extendChunk; y <= chunk.TopBorder + offset * extendChunk; y += offset)
                for (int x = chunk.LeftBorder - offset * extendChunk; x <= chunk.RightBorder + offset * extendChunk; x += offset)
                {
                    yield return new IntCoord(x, y);
                }
        }

        /// <summary>
        /// Get base coord by coord in specifed layer in specifed chunk
        /// </summary>
        /// <param name="coordInChunk">Coordinate of point in chunk (0,0 - leftDownCornerOfChunk)</param>
        public IntCoord GetBaseCoord(IntCoord coordInChunk, Chunk chunk, int layerId)
        {
            IntCoord coord = new IntCoord(chunk.LeftBorder + coordInChunk.x * GetCoordOffsetInLayer(layerId),
                chunk.DownBorder + coordInChunk.y * GetCoordOffsetInLayer(layerId));
            return coord;
        }

        /// <summary>
        /// Get data by coord in specifed layer in specifed chunk
        /// </summary>
        /// <param name="coordInChunk">Coordinate of point in chunk (0,0 - leftDownCornerOfChunk)</param>
        public T GetData<T>(IntCoord coordInChunk, Chunk chunk, PointsStorage<T> pointsStorage, int layerId)
        {
            IntCoord coord = new IntCoord(chunk.LeftBorder + coordInChunk.x * GetCoordOffsetInLayer(layerId), 
                chunk.DownBorder + coordInChunk.y * GetCoordOffsetInLayer(layerId));
            return pointsStorage[GetBaseCoord(coordInChunk, chunk, layerId)];
        }

        /// <summary>
        /// Get data by coord in layer
        /// </summary>
        /// <param name="coord">Coordinate in layer</param>
        public T GetData<T>(IntCoord coordInLayer, PointsStorage<T> pointsStorage, int layerId)
        {
            IntCoord coord = new IntCoord(coordInLayer.x * GetCoordOffsetInLayer(layerId),
                coordInLayer.y * GetCoordOffsetInLayer(layerId));
            return pointsStorage[coord];
        }

        /// <summary>
        /// Get data by base coord
        /// </summary>
        public T GetData<T>(IntCoord baseCoord, PointsStorage<T> pointsStorage)
        {
            return pointsStorage[baseCoord];
        }

        /// <summary>
        /// Initialize all points which are not initialized before for specifed detaliztion layer
        /// </summary>
        public void ExtendDetalization<T>(Chunk chunk, PointsStorage<T> pointsStorage, int newLayerId)
        {
            foreach (IntCoord baseCoord in GetBaseCoordsInLayer(chunk, newLayerId))
            {
                if (!pointsStorage.Contains(baseCoord))
                    pointsStorage.Initialize(baseCoord);
            }
        }

        /// <summary>
        /// Get size of chunk in specifed detalization layer
        /// </summary>
        public int GetSizeInLayer(Chunk chunk, int layerId)
        {
            return ((chunk.Size - 1) / (GetCoordOffsetInLayer(layerId)) + 1);
        }
    }
}
