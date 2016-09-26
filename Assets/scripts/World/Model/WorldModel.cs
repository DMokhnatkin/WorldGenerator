using System;
using System.Collections.Generic;
using World.Common;
using World.DataStructures.ChunksGrid;
using World.Generator.Algorithms.Erosion;
using World.Generator.Algorithms.River;
using World.Generator.Algorithms.WaterFlow;

namespace World.Model
{
    public class WorldModel
    {
        /// <summary>
        /// Grid of chunks
        /// </summary>
        public readonly ChunksNavigator chunksNavigator;

        /// <summary>
        /// Heighmap of world
        /// </summary>
        public readonly PointsStorage<float> heighmap;

        /// <summary>
        /// Map of mountains
        /// </summary>
        public readonly PointsStorage<float> mountainMap;

        /// <summary>
        /// Map of erosion
        /// </summary>
        public readonly PointsStorage<ErosionData> erosionMap;

        /// <summary>
        /// Map of water flow
        /// </summary>
        public readonly PointsStorage<WaterFlowData> waterFlowMap;

        /// <summary>
        /// Map of rivers
        /// </summary>
        public RiverMap riverMap;

        /// <summary>
        /// To access to coords in chunks by detalization layers
        /// </summary>
        public readonly DetalizationAccessor detalizationAccessor;

        /// <summary>
        /// Transform model coordinates to global(Unity) and back
        /// </summary>
        public CoordTransformer CoordTransformer { get; private set; }

        public WorldModel(float modelUnitWidth, int chunkSize)
        {
            chunksNavigator = new ChunksNavigator(chunkSize);
            heighmap = new PointsStorage<float>();
            waterFlowMap = new PointsStorage<WaterFlowData>();
            erosionMap = new PointsStorage<ErosionData>();
            mountainMap = new PointsStorage<float>();
            riverMap = new RiverMap();
            detalizationAccessor = new DetalizationAccessor(chunksNavigator);
            CoordTransformer = new CoordTransformer(this, modelUnitWidth);
        }
    }
}
