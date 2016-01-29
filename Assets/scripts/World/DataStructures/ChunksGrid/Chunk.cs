using System;
using System.Collections.Generic;

namespace World.DataStructures.ChunksGrid
{
    /// <summary>
    /// Represents chunk
    /// </summary>
    public struct Chunk : IEqualityComparer<Chunk>
    {
        /// <summary>
        /// Associated chunksNavigator
        /// </summary>
        public readonly ChunksNavigator chunksNavigator;

        /// <summary>
        /// Coordinate of chunk
        /// </summary>
        public readonly IntCoord chunkCoord;

        /// <summary>
        /// Left down corner of chunk
        /// </summary>
        public readonly IntCoord leftDown;

        public int TopBorder { get { return leftDown.y + Size - 1; } }

        public int RightBorder { get { return leftDown.x + Size - 1; } }

        public int DownBorder { get { return leftDown.x; } }

        public int LeftBorder { get { return leftDown.y; } }

        /// <summary>
        /// Size of chunk
        /// </summary>
        public int Size { get { return chunksNavigator.chunkSize; } }

        public Chunk(ChunksNavigator chunksGrid, IntCoord chunkCoord, IntCoord leftDown)
        {
            this.chunksNavigator = chunksGrid;
            this.chunkCoord = chunkCoord;
            this.leftDown = leftDown;
        }

        public override bool Equals(object obj)
        {
            return chunkCoord.Equals(((Chunk)obj).chunkCoord);
        }

        public bool Equals(Chunk x, Chunk y)
        {
            return x.Equals(y);
        }

        public override int GetHashCode()
        {
            return chunkCoord.GetHashCode();
        }

        public int GetHashCode(Chunk obj)
        {
            return obj.GetHashCode();
        }
    }
}
