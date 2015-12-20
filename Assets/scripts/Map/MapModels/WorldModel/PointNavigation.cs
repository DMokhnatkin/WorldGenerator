using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map.MapModels.Common;

namespace Map.MapModels.WorldModel
{
    public static class PointNavigation
    {
        /// <summary>
        /// Get top neighbor of point in specifed layer
        /// </summary>
        /// <param name="offset">Neighbor number (1-top Neighbor, 2-top Neighbor of top Neighbor, ...)</param>
        /// <returns></returns>
        public static WorldPoint TopNeighbor(WorldModel worldGrid, WorldModelLayer layer, WorldPoint point, int offset = 1)
        {
            Coord coord = new Coord(point.NormalCoord.x, point.NormalCoord.y + layer.CoordOffset * offset);
            return worldGrid.GetPoint(coord);
        }

        /// <summary>
        /// Get right neighbor of point in specifed layer
        /// </summary>
        /// <param name="offset">Neighbor number (1-right Neighbor, 2-right Neighbor of right Neighbor, ...)</param>
        /// <returns></returns>
        public static WorldPoint RightNeighbor(WorldModel worldGrid, WorldModelLayer layer, WorldPoint point, int offset = 1)
        {
            Coord coord = new Coord(point.NormalCoord.x + layer.CoordOffset * offset, point.NormalCoord.y);
            return worldGrid.GetPoint(coord);
        }

        /// <summary>
        /// Get down neighbor of point in specifed layer
        /// </summary>
        /// <param name="offset">Neighbor number (1-down Neighbor, 2-down Neighbor of down Neighbor, ...)</param>
        /// <returns></returns>
        public static WorldPoint DownNeighbor(WorldModel worldGrid, WorldModelLayer layer, WorldPoint point, int offset = 1)
        {
            Coord coord = new Coord(point.NormalCoord.x, point.NormalCoord.y - layer.CoordOffset * offset);
            return worldGrid.GetPoint(coord);
        }

        /// <summary>
        /// Get left neighbor of point in specifed layer
        /// </summary>
        /// <param name="offset">Neighbor number (1-left Neighbor, 2-left Neighbor of left Neighbor, ...)</param>
        /// <returns></returns>
        public static WorldPoint LeftNeighbor(WorldModel worldGrid, WorldModelLayer layer, WorldPoint point, int offset = 1)
        {
            Coord coord = new Coord(point.NormalCoord.x - layer.CoordOffset * offset, point.NormalCoord.y);
            return worldGrid.GetPoint(coord);
        }

        public static WorldPoint RelativeOffset(WorldModel worldModel, WorldModelLayer layer, WorldPoint point, int offsetX, int offsetY)
        {
            Coord coord = new Coord(point.NormalCoord.x + layer.CoordOffset * offsetX, point.NormalCoord.y + layer.CoordOffset * offsetY);
            return worldModel.GetPoint(coord);
        }

        /// <summary>
        /// Get points around specified point(distance is less then radius) 
        /// </summary>
        public static List<WorldPoint> GetAround(WorldModel worldModel, WorldModelLayer layer, WorldPoint point, float radius)
        {
            List<WorldPoint> res = new List<WorldPoint>();
            float rad2 = radius * radius;
            int rad = (int)radius;
            for (int x = -rad; x <= rad; x++)
                for (int y = -rad; y <= rad; y++)
                {
                    if (x * x + y * y <= rad2)
                        if (worldModel.GetPoint(new Coord(x, y), layer) != null)
                            res.Add(worldModel.GetPoint(new Coord(x, y), layer));
                }
            return res;
        }

        /// <summary>
        /// Create not existing points around specified points
        /// </summary>
        public static void CreateAround(WorldModel worldModel, WorldModelLayer layer, WorldPoint point, float radius)
        {
            int rad = (int)radius;
            float rad2 = radius * radius;
            for (int x = -rad; x <= rad; x++)
                for (int y = -rad; y <= rad; y++)
                {
                    if (x * x + y * y <= rad2)
                        if (worldModel.GetPoint(new Coord(x, y), layer) == null)
                            worldModel.CreatePoint(new Coord(x, y), layer);
                }
        }
    }
}
