using System;
using System.Collections.Generic;
using World.Common;
using World.DataStructures.ChunksGrid;

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
            detalizationAccessor = new DetalizationAccessor(chunksNavigator);
            CoordTransformer = new CoordTransformer(this, modelUnitWidth);
        }
    }
}
