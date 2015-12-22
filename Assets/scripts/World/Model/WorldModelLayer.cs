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
        /// Detalization of chunks in this layer
        /// </summary>
        public int Detalization { get; private set; }

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
            Detalization = detalization;
            CoordOffset = Pow2.GetPow2(WorldModel.MaxDetalization - Detalization);
        }

        /// <summary>
        /// Does world model contains point
        /// </summary>
        public bool Contains(ModelCoord coordInLayer)
        {
            return (WorldModel.Contains(coordInLayer));
        }

        /// <summary>
        /// Get point by coordinates in cur layer. If point isn't exists return null.
        /// </summary>
        public ModelPoint this[ModelCoord coordInLayer]
        {
            get
            {
                ModelCoord normalCoord = LayerToNormal(coordInLayer);
                return WorldModel[normalCoord];
            }
        }

        /// <summary>
        /// Create point by coordinates in cur layer.
        /// </summary>
        public ModelPoint CreatePoint(ModelCoord coordInLayer)
        {
            ModelCoord normalCoord = LayerToNormal(coordInLayer);
            return WorldModel.CreatePoint(normalCoord);
        }

        /// <summary>
        /// Get point by coordinates in cur layer. If point isn't exists create it.
        /// </summary>
        public ModelPoint GetOrCreatePoint(ModelCoord coordInLayer)
        {
            ModelCoord normalCoord = LayerToNormal(coordInLayer);
            return WorldModel.GetOrCreatePoint(normalCoord);
        }

    }
}
