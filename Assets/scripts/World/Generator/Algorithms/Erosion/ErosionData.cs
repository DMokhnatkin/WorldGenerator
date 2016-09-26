using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace World.Generator.Algorithms.Erosion
{
    public class ErosionData
    {
        /// <summary>
        /// Height of water
        /// </summary>
        public float waterHeight1 = 0;

        /// <summary>
        /// Height of water 2
        /// </summary>
        public float waterHeight2 = 0;

        /// <summary>
        /// The outflow to top neighbor
        /// </summary>
        public float flowTop = 0;

        /// <summary>
        /// The outflow to right neighbor
        /// </summary>
        public float flowRight = 0;

        /// <summary>
        /// The outflow to down neighbor
        /// </summary>
        public float flowDown = 0;

        /// <summary>
        /// The outflow to left neighbor
        /// </summary>
        public float flowLeft = 0;

        /// <summary>
        /// X velocity
        /// </summary>
        public float xVelocity = 0;

        /// <summary>
        /// Y velocity
        /// </summary>
        public float yVelocity = 0;

        /// <summary>
        /// Suspended sediment amount
        /// </summary>
        public float suspendedSediment = 0;
    }
}
