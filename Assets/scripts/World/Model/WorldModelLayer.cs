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
        public WorldModel WorldModel { get; private set; }

        /// <summary>
        /// Id of layer i.e.
        /// detalization of model in this layer
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Get offset of coord for curent layer.
        /// </summary>
        public int CoordOffset { get; private set; }

        /// <summary>
        /// Transform coordinates from layer to normal form
        /// </summary>
        public ModelCoord LayerToNormal(ModelCoord coord)
        {
            return new ModelCoord(coord.x * CoordOffset, coord.y * CoordOffset);
        }

        internal WorldModelLayer(WorldModel worldModel, int detalization)
        {
            WorldModel = worldModel;
            Id = detalization;
            CoordOffset = Pow2.GetPow2(WorldModel.MaxDetalizationLayerId - Id);
        }
    }
}
