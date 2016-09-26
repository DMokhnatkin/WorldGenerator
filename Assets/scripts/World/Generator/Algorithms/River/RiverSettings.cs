using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World.Generator.Algorithms.River
{
    [Serializable]
    public class RiverSettings
    {
        public float disolve = 0.05f;

        public float sourceEnergy = 0.2f;

        public float maxSourceWaterAmount = 1.0f;

        public float rainy = 0.05f;

        /// <summary>
        /// In model coords
        /// </summary>
        public float minSourceDistance = 200f;

        /// <summary>
        /// How much times try to create source
        /// </summary>
        public int countToTry = 10;

        public float waterAmountEps = 0.001f;
    }
}
