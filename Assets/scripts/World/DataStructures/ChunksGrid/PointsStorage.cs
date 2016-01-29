using System;
using System.Collections.Generic;

namespace World.DataStructures.ChunksGrid
{
    /// <typeparam name="T">Data which stores</typeparam>
    public class PointsStorage<T>
    {
        /// <summary>
        /// Store values and it's coords
        /// </summary>
        internal Dictionary<IntCoord, T> data = new Dictionary<IntCoord, T>();

        public PointsStorage()
        { }

        /// <summary>
        /// Initialize chunk
        /// Complexity = O(m * m), where m - size of chunk 
        /// </summary>
        public void Initialize(Chunk chunk)
        {
            for (int y = chunk.DownBorder; y <= chunk.TopBorder; y++)
                for (int x = chunk.LeftBorder; x <= chunk.RightBorder; x++)
                {
                    IntCoord baseCoord = new IntCoord(x, y);
                    if (!data.ContainsKey(baseCoord))
                        data.Add(baseCoord, default(T));
                }
        }

        /// <summary>
        /// Initialize one point
        /// </summary>
        public void Initialize(IntCoord baseCoord)
        {
            if (Contains(baseCoord))
                throw new ArgumentException("Point already is initialized");
            data.Add(baseCoord, default(T));
        }

        /// <summary>
        /// Does grid contains point with specifed base coord
        /// </summary>
        public bool Contains(IntCoord baseCoord)
        {
            return (data.ContainsKey(baseCoord));
        }

        /// <summary>
        /// Get data by base coord 
        /// </summary>
        public T this[IntCoord baseCoord]
        {
            get
            {
                if (!Contains(baseCoord))
                    throw new ArgumentException(String.Format("No data with {0} coordinate", baseCoord));
                return data[baseCoord];
            }
            set
            {
                if (!Contains(baseCoord))
                    throw new IndexOutOfRangeException();
                data[baseCoord] = value;
            }
        }
    }
}
