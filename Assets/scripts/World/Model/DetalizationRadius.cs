using System;
using System.Collections.Generic;
using World.DataStructures;
using World.DataStructures.ChunksGrid;

namespace World.Model
{
    public struct ChunkDetalization
    {
        public readonly IntCoord chunkCoord;
        public readonly int detalization;

        public ChunkDetalization(IntCoord chunkCoord, int detalization)
        {
            this.chunkCoord = chunkCoord;
            this.detalization = detalization;
        }
    }

    /// <summary>
    /// Addiction between distance to center chunk and detalization
    /// </summary>
    [Serializable]
    public class DetalizationRadius
    {
        /// <summary>
        /// Element with i index - distance when start use max - i detalization.
        /// I.e. if radius of current chunk is less then data[0], we will set it's detalization = maxDetalization
        /// If radius between data[0]..data[1] we will set its's detalization = maxDetalization - 1
        /// If data[i] == -1, this detalization will be skipped
        /// </summary>
        public int[] data;

        int FindDetalization(int distance)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == -1)
                    continue;
                if (data[i] > distance)
                    return i;
            }
            return data.Length;
        }

        public int GetDetalization(int distance)
        {
            return data.Length - FindDetalization(distance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detalizationLayersCount">Must be layers count - 1</param>
        public DetalizationRadius(int detalizationLayersCount)
        {
            data = new int[detalizationLayersCount];
        }

        /// <summary>
        /// Get detalization for chunks around specifed chunk
        /// </summary>
        public IEnumerable<ChunkDetalization> GetDetalizations(IntCoord curChunkCoord)
        {
            for (int x = curChunkCoord.x - data[data.Length - 1]; x <= curChunkCoord.x + data[data.Length - 1]; x++)
                for (int y = curChunkCoord.y - data[data.Length - 1]; y <= curChunkCoord.y + data[data.Length - 1]; y++)
                {
                    yield return new ChunkDetalization(new IntCoord(x, y),
                        GetDetalization((int)Math.Sqrt(Math.Pow(x - curChunkCoord.x, 2) +
                            Math.Pow(y - curChunkCoord.y, 2))));
                }
        }

        /// <summary>
        /// Get only chunks which detalization must be increased.
        /// This method is created for optimization.
        /// </summary>
        /// <param name="prevChunkCoord">Previous chunk coordinate when detalizations was updated</param>
        /// <param name="curChunkCoord">Current chunk coordinate</param>
        public IEnumerable<ChunkDetalization> GetChunksWithIncreasedDetalization(IntCoord prevChunkCoord, IntCoord curChunkCoord)
        {
            if (curChunkCoord.Equals(prevChunkCoord.Top))
            {
                // Return only rows which are upper by one then borders in detalization for last chunk coord
                for (int i = 0; i < data.Length; i++)
                {
                    for (int x = curChunkCoord.x - data[i]; x <= curChunkCoord.x + data[i]; x++)
                    {
                        yield return new ChunkDetalization(new IntCoord(x, curChunkCoord.y + data[i]), data.Length - i);
                    }
                }
                yield break;
            }
            if (curChunkCoord.Equals(prevChunkCoord.Right))
            {
                // Return only rows which are to the right by one then borders in detalization for last chunk coord
                for (int i = 0; i < data.Length; i++)
                {
                    for (int y = curChunkCoord.y - data[i]; y <= curChunkCoord.y + data[i]; y++)
                    {
                        yield return new ChunkDetalization(new IntCoord(curChunkCoord.x + data[i], y), data.Length - i);
                    }
                }
                yield break;
            }
            if (curChunkCoord.Equals(prevChunkCoord.Down))
            {
                // Return only rows which are to the bottom by one then borders in detalization for last chunk coord
                for (int i = 0; i < data.Length; i++)
                {
                    for (int x = curChunkCoord.x - data[i]; x <= curChunkCoord.x + data[i]; x++)
                    {
                        yield return new ChunkDetalization(new IntCoord(x, curChunkCoord.y - data[i]), data.Length - i);
                    }
                }
                yield break;
            }
            if (curChunkCoord.Equals(prevChunkCoord.Left))
            {
                // Return only rows which are to the left by one then borders in detalization for last chunk coord
                for (int i = 0; i < data.Length; i++)
                {
                    for (int y = curChunkCoord.y - data[i]; y <= curChunkCoord.y + data[i]; y++)
                    {
                        yield return new ChunkDetalization(new IntCoord(curChunkCoord.x - data[i], y), data.Length - i);
                    }
                }
                yield break;
            }
            throw new NotImplementedException("prevChunkCoord must be neighbor of curChunkCoord in DetalizationRadius.GetChunksWithExtendedDetalization()");
        }
    }
}
