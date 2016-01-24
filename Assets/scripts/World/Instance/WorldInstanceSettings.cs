using System.Collections.Generic;
using System;
using World.Model;

namespace World.Instance
{
    [Serializable]
    public class WorldInstanceSettings
    {
        /// <summary>
        /// Chunk is matrix chunkSize * chunkSize points
        /// </summary>
        public int chunkSize = 32;

        /// <summary>
        /// Global(unity) size of one model cell
        /// </summary>
        public float baseCellSize = 1f;

        /// <summary>
        /// Height of world
        /// </summary>
        public float height = 500;

        public DetalizationRadius detalization = new DetalizationRadius(6);
    }
}
