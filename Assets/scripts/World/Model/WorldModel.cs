using System;
using System.Collections.Generic;
using World.Common;

namespace World.Model
{
    public class WorldModel
    {
        /// <summary>
        /// Layers of detalization
        /// </summary>
        WorldModelLayer[] detalizationLayers;

        /// <summary>
        /// All points and their normal coordinates
        /// </summary>
        internal Dictionary<ModelCoord, ModelPoint> Points { get; set; }

        /// <summary>
        /// Transform model coordinates to global(Unity) and back
        /// </summary>
        public CoordTransformer CoordTransformer { get; private set; }

        public WorldModel(int detalizationLayerCount, float modelUnitWidth)
        {
            detalizationLayers = new WorldModelLayer[detalizationLayerCount];
            // Reverse because we use maxDetalizationLayer to initialize other layers(we calculate lauer offset using int) 
            for (int i = detalizationLayerCount - 1; i >= 0; i--)
                detalizationLayers[i] = new WorldModelLayer(this, i);
            Points = new Dictionary<ModelCoord, ModelPoint>();
            CoordTransformer = new CoordTransformer(this, modelUnitWidth);
        }

        /// <summary>
        /// Get layer with max detalization(in this layer layerCoord = normlalCoord)
        /// </summary>
        public WorldModelLayer MaxDetalizationLayer
        {
            get { return detalizationLayers[detalizationLayers.Length - 1]; }
        }

        /// <summary>
        /// Get layer max detalization
        /// </summary>
        public int MaxDetalizationLayerId
        {
            get { return detalizationLayers.Length - 1; }
        }

        /// <summary>
        /// Get detalization layer by it's detalization coefficient
        /// </summary>
        public WorldModelLayer GetLayer(int detalizationLayer)
        {
            if (detalizationLayer >= detalizationLayers.Length)
                throw new ArgumentException("There is no detalization layer " + detalizationLayer);
            return detalizationLayers[detalizationLayer];
        }

        /// <summary>
        /// Does model contains point with specifed normal coord
        /// </summary>
        public bool Contains(ModelCoord normalCoord)
        {
            return (Points.ContainsKey(normalCoord));
        }

        /// <summary>
        /// Get of set modelPoint by normal coordinate(coordinate in max layer detalization) 
        /// </summary>
        public ModelPoint this[ModelCoord normalCoord]
        {
            get
            {
                if (Points.ContainsKey(normalCoord))
                    return Points[normalCoord];
                return null;
            }
        }

        public ModelPoint CreatePoint(ModelCoord normalCoord)
        {
            if (Points.ContainsKey(normalCoord))
                throw new ArgumentException(String.Format("Point {0} already created", normalCoord.ToString()));
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
            return this[normalCoord];
        }
    }
}
