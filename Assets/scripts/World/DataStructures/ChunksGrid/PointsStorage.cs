using System;
using System.Collections.Generic;

namespace World.DataStructures.ChunksGrid
{
    /// <typeparam name="T">Data which stores</typeparam>
    public class PointsStorage<T> : IPointsStorage
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
        /// <param name="aroundChunk">Initialize some points around chunk. This parameter is radius</param>
        public void Initialize(Chunk chunk, int aroundChunk = 0)
        {
            for (int y = chunk.DownBorder - aroundChunk; y <= chunk.TopBorder + aroundChunk; y++)
                for (int x = chunk.LeftBorder - aroundChunk; x <= chunk.RightBorder + aroundChunk; x++)
                {
                    IntCoord baseCoord = new IntCoord(x, y);
                    if (!data.ContainsKey(baseCoord))
                        data.Add(baseCoord, System.Activator.CreateInstance<T>());
                }
        }

        /// <summary>
        /// Initialize one point
        /// </summary>
        public void Initialize(IntCoord baseCoord)
        {
            if (Contains(baseCoord))
                throw new ArgumentException("Point already is initialized");
            data.Add(baseCoord, System.Activator.CreateInstance<T>());
        }

        /// <summary>
        /// Initialize some points
        /// </summary>
        public void Initialize(params IntCoord[] baseCoords)
        {
            foreach (IntCoord z in baseCoords)
                Initialize(z);
        }

        /// <summary>
        /// Initialize points (if some error occured false will be returned)
        /// </summary>
        public bool TryInitialize(params IntCoord[] baseCoords)
        {
            bool res = true;
            foreach (IntCoord z in baseCoords)
            {
                if (!Contains(z))
                    Initialize(z);
                else
                    res = false;
            }
            return res;
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

        object IPointsStorage.this[IntCoord baseCoord]
        {
            get { return this[baseCoord]; }
            set { this[baseCoord] = (T)value; }
        }
    }
}
