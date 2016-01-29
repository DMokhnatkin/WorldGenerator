using System;
using System.Collections.Generic;
using World.Common;

namespace World.DataStructures.ChunksGrid
{
    /// <summary>
    /// Implements access in detalization layers
    /// </summary>
    /// <typeparam name="T">Type of data which stores in collection</typeparam>
    public class DetalizationAccessor
    {
        /// <summary>
        /// Count of detalization layers
        /// </summary>
        public readonly int detalizationLayersCount;

        public DetalizationAccessor(int detalizationLayersCount)
        {
            this.detalizationLayersCount = detalizationLayersCount;
        }

        /// <summary>
        /// Get offset of one coordinate in specifed layer
        /// </summary>
        public int GetCoordOffsetInLayer(int layerId)
        {
            return (Pow2.GetPow2(detalizationLayersCount - layerId - 1));
        }

        /// <summary>
        /// Get base coordinates which contains specifed chunk in specifed layer
        /// </summary>
        public IEnumerable<IntCoord> GetBaseCoordsInLayer(Chunk chunk, int layerId)
        {
            int offset = GetCoordOffsetInLayer(layerId);
            for (int y = chunk.DownBorder; y <= chunk.TopBorder; y += offset)
                for (int x = chunk.LeftBorder; x <= chunk.RightBorder; x += offset)
                {
                    yield return new IntCoord(x, y);
                }
        }

        /// <summary>
        /// Get data by coord in specifed layer in specifed chunk
        /// </summary>
        /// <param name="coordInChunk">Coordinate of point in chunk (0,0 - leftDownCornerOfChunk)</param>
        public T GetData<T>(IntCoord coordInChunk, Chunk chunk, PointsStorage<T> pointsStorage, int layerIf)
        {
            IntCoord coord = new IntCoord(chunk.LeftBorder + coordInChunk.x * GetCoordOffsetInLayer(layerIf), 
                chunk.DownBorder + coordInChunk.y * GetCoordOffsetInLayer(layerIf));
            return pointsStorage[coord];
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
            return (chunk.Size / (GetCoordOffsetInLayer(layerId)));
        }
    }
}
