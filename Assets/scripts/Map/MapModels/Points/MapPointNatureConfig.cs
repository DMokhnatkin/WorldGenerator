using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map.MapModels.Points
{
    /// <summary>
    /// Configuration of natural conditions in point
    /// </summary>
    public class MapPointNatureConfig
    {
        /// <summary>
        /// Humidity of this point
        /// 0 - normal
        /// less - aridity point
        /// more - wet point
        /// </summary>
        public short humidity;
        /// <summary>
        /// Max slope between this point and it's neighbor points in radians
        /// </summary>
        public float slope;
        public short windy;

        public MapPointNatureConfig()
        {
            humidity = 0;
            windy = 0;
        }
    }
}
