using System;
using World.Model.Frames;
using World.Common;

namespace World.Model.Chunks
{
    /// <summary>
    /// Class wrapper for world model. It allows access by chunk.
    /// Chunk edges are stitching
    /// </summary>
    public class ChunkGrid
    {
        public WorldModel Model { get; private set; }

        /// <summary>
        /// Size of one chunk
        /// </summary>
        public int ChunkSize { get; private set; }

        public ChunkGrid(WorldModel model, int chunkSize)
        {
            Model = model;
            ChunkSize = chunkSize;
            if (Pow2.GetLog2(ChunkSize - 1) == -1)
                throw new ArgumentException("Chunk size must be 2^k + 1");
        }

        /// <summary>
        /// Get chunk by its coordinate
        /// </summary>
        public ModelChunk GetChunk(ModelCoord chunkCoord)
        {
            SquareFrame frame = new SquareFrame(new ModelCoord(chunkCoord.x * (ChunkSize - 1), chunkCoord.y * (ChunkSize - 1)), 
                ChunkSize);
            return new ModelChunk(chunkCoord, frame, Model);
        }

        /// <summary>
        /// Get chunk by any point coord which is in chunk
        /// </summary>
        public ModelChunk GetChunkByInnerCoord(ModelCoord pointCoordInChunk)
        {
            ModelCoord chunkCoord = new ModelCoord(pointCoordInChunk.x / ChunkSize,
                pointCoordInChunk.y / ChunkSize);
            return GetChunk(chunkCoord);
        }

        /// <summary>
        /// Create new empty points to extend detalizayion of chunk
        /// </summary>
        public void ExtendDetalization(ModelChunk chunk, int newDetalization)
        {
            WorldModelLayer newLayer = Model.GetLayer(newDetalization);
            for (int x = chunk.Frame.LeftBorder; x <= chunk.Frame.RightBorder; x += newLayer.CoordOffset)
                for (int y = chunk.Frame.DownBorder; y <= chunk.Frame.TopBorder; y += newLayer.CoordOffset)
                {
                    ModelCoord coord = new ModelCoord(x, y);
                    if (!Model.Contains(coord))
                        Model.CreatePoint(coord);
                }
        }
    }
}
