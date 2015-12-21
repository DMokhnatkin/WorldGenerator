using System;
using System.Collections.Generic;
using World.Common;

namespace World.Model
{
    public class WorldModel
    {
        /// <summary>
        /// How much times chunk can be divided 
        /// (each divide increase point count in chunk)
        /// </summary>
        public int DetalizationLayerCount { get; private set; }

        /// <summary>
        /// All points and their coordinates
        /// </summary>
        Dictionary<ModelCoord, ModelPoint> Points { get; set; }

        /// <summary>
        /// Transform model coordinates to global(Unity) and back
        /// </summary>
        public ModelCoordToGlobalTransformer CoordTransformer { get; private set; }

        public WorldModel(int detalizationLayerCount, float modelUnitWidth)
        {
            DetalizationLayerCount = detalizationLayerCount;
            Points = new Dictionary<ModelCoord, ModelPoint>();
            CoordTransformer = new ModelCoordToGlobalTransformer(this, modelUnitWidth);
        }

        /// <summary>
        /// Get point by normal coord. If point isn't exists return null
        /// </summary>
        public ModelPoint GetPoint(ModelCoord normalCoord)
        {
            if (Points.ContainsKey(normalCoord))
                return Points[normalCoord];
            return null;
        }

        /// <summary>
        /// Get point by layer and coordinates in it. If point isn't exists return null.
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public ModelPoint GetPoint(ModelCoord coord, WorldModelLayer layer)
        {
            ModelCoord normalCoord = layer.LayerToNormal(coord);
            if (Points.ContainsKey(normalCoord))
                return Points[normalCoord];
            return null;
        }

        /// <summary>
        /// Create point by normal coord
        /// </summary>
        /// <exception cref="ArgumentException">Point is already created</exception>
        public ModelPoint CreatePoint(ModelCoord normalCoord)
        {
            if (Points.ContainsKey(normalCoord))
                throw new ArgumentException(String.Format("Point with coord {0} already created", normalCoord.ToString()));
            Points.Add(normalCoord, new ModelPoint(normalCoord));
            return Points[normalCoord];
        }

        public ModelPoint CreatePoint(ModelCoord coord, WorldModelLayer layer)
        {
            ModelCoord normalCoord = layer.LayerToNormal(coord);
            if (Points.ContainsKey(normalCoord))
                throw new ArgumentException(String.Format("Point with coord(normal) {0} already created", normalCoord.ToString()));
            Points.Add(normalCoord, new ModelPoint(normalCoord));
            return Points[normalCoord];
        }

        /// <summary>
        /// If point exists return it.
        /// If point isn't exists create it and then return 
        /// </summary>
        public ModelPoint GetOrCreatePoint(ModelCoord normalCoord)
        {
            if (!Points.ContainsKey(normalCoord))
                return CreatePoint(normalCoord);
            else
                return GetPoint(normalCoord);
        }

        public WorldModelLayer GetLayer(int detalization)
        {
            return new WorldModelLayer(this, detalization);
        }

        public WorldModelLayer MaxDetalizationLayer
        {
            get { return new WorldModelLayer(this, DetalizationLayerCount); }
        }
    }
}
