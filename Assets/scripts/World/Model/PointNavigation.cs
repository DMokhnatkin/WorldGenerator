using System;
using World.Common;
using World.Model.PointCollections;
using World.Model.Frames;
using World.Model;

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
            return worldModel[coord];
        }

        /// <summary>
        /// Get points around specified point(distance is less then radius) 
        /// </summary>
        public static WorldPointCollection GetAround(WorldModelLayer layer, ModelPoint point, float radius)
        {
            WorldPointCollection res = new WorldPointCollection();
            float rad2 = radius * radius;
            int rad = (int)radius;
            for (int x = -rad; x <= rad; x++)
                for (int y = -rad; y <= rad; y++)
                {
                    if (x * x + y * y <= rad2)
                        if (layer[new ModelCoord(x, y)] != null)
                            res.Add(layer[new ModelCoord(x, y)]);
                }
            return res;
        }

        /// <summary>
        /// Create not existing points around specified points
        /// </summary>
        public static void CreateAround(WorldModelLayer layer, ModelPoint point, float radius)
        {
            int rad = (int)radius;
            float rad2 = radius * radius;
            for (int x = -rad; x <= rad; x++)
                for (int y = -rad; y <= rad; y++)
                {
                    if (x * x + y * y <= rad2)
                        if (!layer.Contains(new ModelCoord(x, y)))
                            layer.CreatePoint(new ModelCoord(x, y));
                }
        }

        /// <summary>
        /// Return BinSquareFrame with point in center
        /// size = 2^radius + 1
        /// </summary>
        public static BinPlus1SquareFrame GetBinPlus1SquareFrame(ModelCoord pointCoord, int radius)
        {
            if (Pow2.GetLog2(radius) == -1)
                throw new ArgumentException("Radius must 2^n");
            return new BinPlus1SquareFrame(
                new ModelCoord(pointCoord.x - radius, pointCoord.y - radius),
                2 * radius + 1);
        }

        /// <summary>
        /// Get square frame around specifed point
        /// </summary>
        public static SquareFrame GetAround(ModelCoord pointCoord, int radius)
        {
            return new SquareFrame(
                new ModelCoord(pointCoord.x - radius, pointCoord.y - radius),
                2 * radius + 1);
        }

        /// <summary>
        /// Create points in specifed frame
        /// </summary>
        public static void CreatePoints(BinPlus1SquareFrame frame, WorldModelLayer layer)
        {
            foreach (ModelCoord coord in frame.GetCoords())
                if (!layer.Contains(coord))
                    layer.CreatePoint(coord);
        }
    }
}
