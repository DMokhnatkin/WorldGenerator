using System;
using System.Collections.Generic;
using World.Common;

namespace World.DataStructures.ChunksGrid
{
    /// <summary>
    /// Represents grid which is divided by chunks. 
    /// </summary>
    public class ChunksNavigator
    {
        /// <summary>
        /// Size of chunk, always 2^n + 1
        /// </summary>
        public readonly int chunkSize;

        public ChunksNavigator(int chunkSize)
        {
            if (Pow2.GetLog2(chunkSize - 1) == -1)
                throw new ArgumentException("Chunk size must be 2^n + 1");
            this.chunkSize = chunkSize;
        }

        /// <summary>
        /// Get chunk by it's coordinate
        /// Complexity = O(1)
        /// </summary>
        public Chunk GetChunk(IntCoord chunkCoord)
        {
            return new Chunk(this, chunkCoord, new IntCoord(chunkCoord.x * (chunkSize - 1), chunkCoord.y * (chunkSize - 1)));
        }

        /// <summary>
        /// Get chunk by coord in it
        /// </summary>
        public Chunk GetChunkByInnerCoord(IntCoord coord)
        {
            return new Chunk(this, coord, new IntCoord(coord.x / chunkSize, coord.y / chunkSize));
        }

        /// <summary>
        /// Top neighbor of chunk
        /// </summary>
        public Chunk TopNeighbor(Chunk chunk)
        {
            return GetChunk(chunk.chunkCoord.Top);
        }

        /// <summary>
        /// Right neighbor of chunk
        /// </summary>
        public Chunk RightNeighbor(Chunk chunk)
        {
            return GetChunk(chunk.chunkCoord.Right);
        }

        /// <summary>
        /// Down neighbor of chunk
        /// </summary>
        public Chunk DownNeighbor(Chunk chunk)
        {
            return GetChunk(chunk.chunkCoord.Down);
        }

        /// <summary>
        /// Left neighbor of chunk
        /// </summary>
        public Chunk LeftNeighbor(Chunk chunk)
        {
            return GetChunk(chunk.chunkCoord.Left);
        }
    }
    
}
