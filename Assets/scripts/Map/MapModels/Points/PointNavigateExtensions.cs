using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map.MapModels;
using Map.MapModels.Areas;

namespace Map.MapModels.Points
{
    public static class PointNavigateExtensions
    {
        public static bool IsLeftTopInArea(this MapPoint pt, Area area)
        {
            return object.ReferenceEquals(area.LeftTopPoint_Val, pt);
        }

        public static bool IsRightTopInArea(this MapPoint pt, Area area)
        {
            return object.ReferenceEquals(area.RightTopPoint_Val, pt);
        }

        public static bool IsLeftDownInArea(this MapPoint pt, Area area)
        {
            return object.ReferenceEquals(area.LeftDownPoint_Val, pt);
        }

        public static bool IsRightDownInArea(this MapPoint pt, Area area)
        {
            return object.ReferenceEquals(area.RightDownPoint_Val, pt);
        }

        public static bool IsMiddlePt(this MapPoint pt, Area area)
        {
            return object.ReferenceEquals(area.MiddlePt_Val, pt);
        }

        public static bool IsTopEdgeMiddlePt(this MapPoint pt, Area area)
        {
            return object.ReferenceEquals(area.TopEdgeMiddlePt_Val, pt);
        }

        public static bool IsRightEdgeMiddlePt(this MapPoint pt, Area area)
        {
            return object.ReferenceEquals(area.RightEdgeMiddlePt_Val, pt);
        }

        public static bool IsDownEdgeMiddlePt(this MapPoint pt, Area area)
        {
            return object.ReferenceEquals(area.DownEdgeMiddlePt_Val, pt);
        }

        public static bool IsLeftEdgeMiddlePt(this MapPoint pt, Area area)
        {
            return object.ReferenceEquals(area.LeftEdgeMiddlePt_Val, pt);
        }

        public static bool IsInArea(this MapPoint pt, Area area)
        {
            return pt.IsLeftTopInArea(area) || pt.IsRightTopInArea(area) ||
                pt.IsLeftDownInArea(area) || pt.IsRightDownInArea(area) ||
                pt.IsMiddlePt(area);
        }

        /// <summary>
        /// Get top neighbor for point in area depth layer in which area is
        /// </summary>
        /// <param name="index">How much points skip</param>
        /// <returns></returns>
        public static MapPoint TopNeighborInLayer(this MapPoint point, Area area, int index)
        {
            Area curArea = area;
            MapPoint curPt = point;
            for (int i = 0; i < index; i++)
            {
                curPt = curPt.TopNeighborInLayer(curArea);
                if (curPt == null)
                    return null;
                if (!curPt.IsInArea(curArea))
                    curArea = curArea.TopNeighbor;
                if (curArea == null)
                    return null;
            }
            return curPt;
        }

        /// <summary>
        /// Get right neighbor for point in area depth layer in which area is
        /// </summary>
        /// <param name="index">How much points skip</param>
        /// <returns></returns>
        public static MapPoint RightNeighborInLayer(this MapPoint point, Area area, int index)
        {
            Area curArea = area;
            MapPoint curPt = point;
            for (int i = 0; i < index; i++)
            {
                curPt = curPt.RightNeighborInLayer(curArea);
                if (curPt == null)
                    return null;
                if (!curPt.IsInArea(curArea))
                    curArea = curArea.RightNeighbor;
                if (curArea == null)
                    return null;
            }
            return curPt;
        }

        /// <summary>
        /// Get down neighbor for point in area depth layer in which area is
        /// </summary>
        /// <param name="index">How much points skip</param>
        /// <returns></returns>
        public static MapPoint DownNeighborInLayer(this MapPoint point, Area area, int index)
        {
            Area curArea = area;
            MapPoint curPt = point;
            for (int i = 0; i < index; i++)
            {
                curPt = curPt.DownNeighborInLayer(curArea);
                if (curPt == null)
                    return null;
                if (!curPt.IsInArea(curArea))
                    curArea = curArea.DownNeighbor;
                if (curArea == null)
                    return null;
            }
            return curPt;
        }

        /// <summary>
        /// Get left neighbor for point in area depth layer in which area is
        /// </summary>
        /// <param name="index">How much points skip</param>
        /// <returns></returns>
        public static MapPoint LeftNeighborInLayer(this MapPoint point, Area area, int index)
        {
            Area curArea = area;
            MapPoint curPt = point;
            for (int i = 0; i < index; i++)
            {
                curPt = curPt.LeftNeighborInLayer(curArea);
                if (curPt == null)
                    return null;
                if (!curPt.IsInArea(curArea))
                    curArea = curArea.LeftNeighbor;
                if (curArea == null)
                    return null;
            }
            return curPt;
        }

        /// <summary>
        /// Get top neighbor for point in area depth layer in which area is
        /// </summary>
        public static MapPoint TopNeighborInLayer(this MapPoint point, Area area)
        {
            /* Corners */
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

            /* Edges */
            if (point.IsTopEdgeMiddlePt(area))
            {
                if (area.TopNeighbor != null)
                    return area.TopNeighbor.TopEdgeMiddlePt_Val;
                return null;
            }
            if (point.IsRightEdgeMiddlePt(area))
            {
                if (area.TopNeighbor != null)
                    return area.TopNeighbor.RightEdgeMiddlePt_Val;
                return null;
            }
            if (point.IsDownEdgeMiddlePt(area))
            {
                return area.TopEdgeMiddlePt_Val;
            }
            if (point.IsLeftEdgeMiddlePt(area))
            {
                if (area.TopNeighbor != null)
                    return area.TopNeighbor.LeftEdgeMiddlePt_Val;
                return null;
            }
            /* Middle point */
            if (point.IsMiddlePt(area))
            {
                if (area.TopNeighbor != null)
                    return area.TopNeighbor.MiddlePt_Val;
                return null;
            }
            throw new ArgumentException("area doesn't contains point");
        }

        /// <summary>
        /// Get right neighbor for point in depth layer in which area is
        /// </summary>
        public static MapPoint RightNeighborInLayer(this MapPoint point, Area area)
        {
            /* Corners */
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

            /* Edges */
            if (point.IsTopEdgeMiddlePt(area))
            {
                if (area.RightNeighbor != null)
                    return area.RightNeighbor.TopEdgeMiddlePt_Val;
                return null;
            }
            if (point.IsRightEdgeMiddlePt(area))
            {
                if (area.RightNeighbor != null)
                    return area.RightNeighbor.RightEdgeMiddlePt_Val;
                return null;
            }
            if (point.IsDownEdgeMiddlePt(area))
            {
                if (area.RightNeighbor != null)
                    return area.RightNeighbor.DownEdgeMiddlePt_Val;
                return null;
            }
            if (point.IsLeftEdgeMiddlePt(area))
            {
                return area.RightEdgeMiddlePt_Val;
            }

            /* Middle point */
            if (point.IsMiddlePt(area))
            {
                if (area.RightNeighbor != null)
                    return area.RightNeighbor.MiddlePt_Val;
                return null;
            }
            throw new ArgumentException("area doesn't contains point");
        }

        /// <summary>
        /// Get down neighbor for point in cur depth layer in which area is
        /// </summary>
        public static MapPoint DownNeighborInLayer(this MapPoint point, Area area)
        {
            /* Corners */
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

            /* Edges */
            if (point.IsTopEdgeMiddlePt(area))
            {
                return area.DownEdgeMiddlePt_Val;
            }
            if (point.IsRightEdgeMiddlePt(area))
            {
                if (area.DownNeighbor != null)
                    return area.DownNeighbor.RightEdgeMiddlePt_Val;
                return null;
            }
            if (point.IsDownEdgeMiddlePt(area))
            {
                if (area.DownNeighbor != null)
                    return area.DownNeighbor.DownEdgeMiddlePt_Val;
                return null;
            }
            if (point.IsLeftEdgeMiddlePt(area))
            {
                if (area.DownNeighbor != null)
                    return area.DownNeighbor.LeftEdgeMiddlePt_Val;
                return null;
            }

            /* Middle point */
            if (point.IsMiddlePt(area))
            {
                if (area.DownNeighbor != null)
                    return area.DownNeighbor.MiddlePt_Val;
                return null;
            }

            throw new ArgumentException("area doesn't contains point");
        }

        /// <summary>
        /// Get left neighbor for point in cur depth layer in which area is
        /// </summary>
        public static MapPoint LeftNeighborInLayer(this MapPoint point, Area area)
        {
            /* Corners */
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

            /* Edges */
            if (point.IsTopEdgeMiddlePt(area))
            {
                if (area.LeftNeighbor != null)
                    return area.LeftNeighbor.TopEdgeMiddlePt_Val;
                return null;
            }
            if (point.IsRightEdgeMiddlePt(area))
            {
                return area.LeftEdgeMiddlePt_Val;
            }
            if (point.IsDownEdgeMiddlePt(area))
            {
                if (area.LeftNeighbor != null)
                    return area.LeftNeighbor.DownEdgeMiddlePt_Val;
                return null;
            }
            if (point.IsLeftEdgeMiddlePt(area))
            {
                if (area.LeftNeighbor != null)
                    return area.LeftNeighbor.LeftEdgeMiddlePt_Val;
                return null;
            }

            /* Middle point */
            if (point.IsMiddlePt(area))
            {
                if (area.LeftNeighbor != null)
                    return area.LeftNeighbor.MiddlePt_Val;
                return null;
            }
            throw new ArgumentException("area doesn't contains point");
        }
    }
}
