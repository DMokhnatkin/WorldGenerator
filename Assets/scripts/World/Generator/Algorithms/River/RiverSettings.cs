using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World.Generator.Algorithms.River
{
    [Serializable]
    public class RiverSettings
    {
        public int maxSourcesPerChunk = 2;

        public float sourceEnergy = 0.2f;

        /// <summary>
        /// How much percentages of chunk rivers should be
        /// </summary>
        public float riverChunkDensity = 0.1f;
    }
}
