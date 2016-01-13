using System;
using World.Model.Frames;

namespace World.Model.Chunks
{
    /// <summary>
    /// Class wrapper for world model. It allows access by chunk.
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
        }

        /// <summary>
        /// Get chunk by its coordinate
        /// </summary>
        public ModelChunk GetChunk(ModelCoord chunkCoord)
        {
            SquareFrame frame = new SquareFrame(new ModelCoord(chunkCoord.x * ChunkSize, chunkCoord.y * ChunkSize), 
                ChunkSize);
            return new ModelChunk(chunkCoord, frame);
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
    }
}
