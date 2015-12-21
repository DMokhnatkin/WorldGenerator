using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using World.Common;

namespace World.Model
{
    public class WorldModelLayer
    {
        /// <summary>
        /// For which worldGrid this layer is
        /// </summary>
        public WorldModel WorldGrid { get; private set; }

        /// <summary>
        /// Detalization of chunks in this layer
        /// </summary>
        public int Detalization { get; private set; }

        /// <summary>
        /// Get offset of coord for curent layer.
        /// </summary>
        public int CoordOffset
        {
            get { return Pow2.GetPow2(WorldGrid.DetalizationLayerCount - Detalization); }
        }

        /// <summary>
        /// Transform coordinates from layer to normal form
        /// </summary>
        public ModelCoord LayerToNormal(ModelCoord coord)
        {
            return new ModelCoord(coord.x * CoordOffset, coord.y * CoordOffset);
        }

        internal WorldModelLayer(WorldModel worldGrid, int detalization = 0)
        {
            WorldGrid = worldGrid;
            Detalization = detalization;
        }
    }
}
