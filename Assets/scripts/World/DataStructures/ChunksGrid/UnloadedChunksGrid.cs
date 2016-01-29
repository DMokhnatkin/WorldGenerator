using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World.DataStructures.ChunksGrid
{
    /// <summary>
    /// Chunks from this grid can be unloaded
    /// </summary>
    public class UnloadedChunksGrid
    {
        /// <summary>
        /// Status of chunk
        /// </summary>
        public enum ChunkState { Unloaded, Ready, Noninitialized };

        /// <summary>
        /// Chunks which was initialized
        /// </summary>
        internal Dictionary<IntCoord, ChunkState> chunksStatus = new Dictionary<IntCoord, ChunkState>();

        /// <summary>
        /// Get state of chunk
        /// </summary>
        public ChunkState GetChunkState(IntCoord coord)
        {
            if (chunksStatus.ContainsKey(coord))
                return chunksStatus[coord];
            else
                return ChunkState.Noninitialized;
        }
    }
}
