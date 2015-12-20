using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map.MapModels.Common;

namespace Map.MapModels.WorldModel
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
            get { return Pow2.GetPow2(WorldGrid.MaxChunkDetalization - Detalization); }
        }

        /// <summary>
        /// Transform coordinates from layer to normal form
        /// </summary>
        public Coord LayerToNormal(Coord coord)
        {
            return new Coord(coord.x * CoordOffset, coord.y * CoordOffset);
        }

        internal WorldModelLayer(WorldModel worldGrid, int detalization = 0)
        {
            WorldGrid = worldGrid;
            Detalization = detalization;
        }
    }
}
