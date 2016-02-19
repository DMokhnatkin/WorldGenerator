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

        public float maxSourceDeep = 0.2f;
    }
}
