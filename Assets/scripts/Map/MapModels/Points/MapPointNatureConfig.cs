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
        /// <summary>
        /// How much water leaked
        /// </summary>
        public float flood = 0;

#if DEBUG
        // To which point flood was falled
        public HashSet<IMapPoint> falledTo = new HashSet<IMapPoint>();
#endif

        public MapPointNatureConfig()
        {
            humidity = 0;
            windy = 0;
        }
    }
}
