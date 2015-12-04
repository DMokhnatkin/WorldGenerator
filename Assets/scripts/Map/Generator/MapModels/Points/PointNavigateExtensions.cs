using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map.Generator.MapModels;

namespace Map.Generator.MapModels
{
    public static class PointNavigateExtensions
    {
        /// <summary>
        /// Get top neighbor for point in area depth layer in which area is
        /// </summary>
        public static MapPoint TopNeighborInLayer(this MapPoint point, Area area)
        {
            if (point.IsLeftTopInArea(area))
            {
                if (area.TopNeighbor != null)
                    return area.TopNeighbor.LeftTopPoint_Val;
                return null;
            }
            if (point.IsRightTopInArea(area))
            {
                if (area.TopNeighbor != null)
                    return area.TopNeighbor.RightTopPoint_Val;
                return null;
            }
            if (point.IsLeftDownInArea(area))
            {
                return area.LeftTopPoint_Val;
            }
            if (point.IsRightDownInArea(area))
            {
                return area.RightTopPoint_Val;
            }
            throw new ArgumentException("area doesn't contains point");
        }

        /// <summary>
        /// Get right neighbor for point in depth layer in which area is
        /// </summary>
        public static MapPoint RightNeighborInLayer(this MapPoint point, Area area)
        {
            if (point.IsLeftTopInArea(area))
            {
                return area.RightTopPoint_Val;
            }
            if (point.IsRightTopInArea(area))
            {
                if (area.RightNeighbor != null)
                    return area.RightNeighbor.RightTopPoint_Val;
                return null;
            }
            if (point.IsLeftDownInArea(area))
            {
                return area.RightDownPoint_Val;
            }
            if (point.IsRightDownInArea(area))
            {
                if (area.RightNeighbor != null)
                    return area.RightNeighbor.RightDownPoint_Val;
                return null;
            }
            throw new ArgumentException("area doesn't contains point");
        }

        /// <summary>
        /// Get down neighbor for point in cur depth layer in which area is
        /// </summary>
        public static MapPoint DownNeighborInLayer(this MapPoint point, Area area)
        {
            if (point.IsLeftTopInArea(area))
            {
                return area.LeftDownPoint_Val;
            }
            if (point.IsRightTopInArea(area))
            {
                return area.RightDownPoint_Val;
            }
            if (point.IsLeftDownInArea(area))
            {
                if (area.DownNeighbor != null)
                    return area.DownNeighbor.LeftDownPoint_Val;
                return null;
            }
            if (point.IsRightDownInArea(area))
            {
                if (area.DownNeighbor != null)
                    return area.DownNeighbor.RightDownPoint_Val;
                return null;
            }
            throw new ArgumentException("area doesn't contains point");
        }

        /// <summary>
        /// Get left neighbor for point in cur depth layer in which area is
        /// </summary>
        public static MapPoint LeftNeighborInLayer(this MapPoint point, Area area)
        {
            if (point.IsLeftTopInArea(area))
            {
                if (area.LeftNeighbor != null)
                    return area.LeftNeighbor.LeftTopPoint_Val;
                return null;
            }
            if (point.IsRightTopInArea(area))
            {
                return area.LeftTopPoint_Val;
            }
            if (point.IsLeftDownInArea(area))
            {
                if (area.LeftNeighbor != null)
                    return area.LeftNeighbor.LeftDownPoint_Val;
                return null;
            }
            if (point.IsRightDownInArea(area))
            {
                return area.LeftDownPoint_Val;
            }
            throw new ArgumentException("area doesn't contains point");
        }
    }
}
