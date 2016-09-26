using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World.Generator.Algorithms.Erosion
{
    [Serializable]
    public class ErosionSettings
    {
        /// <summary>
        /// Gravity const
        /// </summary>
        public float gravity = 9.8f;

        /// <summary>
        /// Rainy conts
        /// </summary>
        public float rainy = 1.0f;

        /// <summary>
        /// Sediment capacity constant
        /// </summary>
        public float k = 1.0f;

        /// <summary>
        /// Dissolving constant
        /// </summary>
        public float ks = 1.0f;

        /// <summary>
        /// Deposition constant
        /// </summary>
        public float kd = 1.0f;
    }
}
