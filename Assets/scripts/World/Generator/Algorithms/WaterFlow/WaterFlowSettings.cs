using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World.Generator.Algorithms.WaterFlow
{
    [Serializable]
    public class WaterFlowSettings
    {
        public float g = 9.8f;

        public float rainy = 1.0f;

        public float eps = 0.01f;

        public float kSlip = 0.5f;

        public float inertness = 0.9f;

        /// <summary>
        /// Sediment capacity constant
        /// </summary>
        public float k_c = 1.0f;

        /// <summary>
        /// Deposition const [0..1]
        /// </summary>
        public float k_d = 1.0f;

        /// <summary>
        /// Dissolve const [0..1]
        /// </summary>
        public float k_s = 1.0f;

        /// <summary>
        /// Evaporation const [0..1]
        /// </summary>
        public float k_e = 0.9f;


        public float waterEpsFlow = 0.05f;
        public float waterMaxBlock = 0.05f;
    }
}
