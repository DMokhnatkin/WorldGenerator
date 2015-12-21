using System;
using World.Common;
using World.Model.PointCollections;

namespace World.Model
{
    /// <summary>
    /// Class for simple navigation relative to specifed point
    /// </summary>
    public static class PointNavigation
    {
        /// <summary>
        /// Get point by relative offset in layer
        /// </summary>
        public static ModelPoint RelativeOffset(WorldModel worldModel, WorldModelLayer layer, ModelPoint point, int offsetX, int offsetY)
        {
            ModelCoord coord = new ModelCoord(point.NormalCoord.x + layer.CoordOffset * offsetX, point.NormalCoord.y + layer.CoordOffset * offsetY);
            return worldModel.GetPoint(coord);
        }

        /// <summary>
        /// Get points around specified point(distance is less then radius) 
        /// </summary>
        public static WorldPointCollection GetAround(WorldModel worldModel, WorldModelLayer layer, ModelPoint point, float radius)
        {
            WorldPointCollection res = new WorldPointCollection();
            float rad2 = radius * radius;
            int rad = (int)radius;
            for (int x = -rad; x <= rad; x++)
                for (int y = -rad; y <= rad; y++)
                {
                    if (x * x + y * y <= rad2)
                        if (worldModel.GetPoint(new ModelCoord(x, y), layer) != null)
                            res.Add(worldModel.GetPoint(new ModelCoord(x, y), layer));
                }
            return res;
        }

        /// <summary>
        /// Create not existing points around specified points
        /// </summary>
        public static void CreateAround(WorldModel worldModel, WorldModelLayer layer, ModelPoint point, float radius)
        {
            int rad = (int)radius;
            float rad2 = radius * radius;
            for (int x = -rad; x <= rad; x++)
                for (int y = -rad; y <= rad; y++)
                {
                    if (x * x + y * y <= rad2)
                        if (worldModel.GetPoint(new ModelCoord(x, y), layer) == null)
                            worldModel.CreatePoint(new ModelCoord(x, y), layer);
                }
        }
    }
}
