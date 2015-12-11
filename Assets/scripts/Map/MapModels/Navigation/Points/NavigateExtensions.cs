using System;
using Map.MapModels.Areas;
using Map.MapModels.Points;

namespace Map.MapModels.Navigation.Points
{
    public static class NavigateExtensions
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
        public static MapPointInLayer TopNeighborInLayer(this MapPointInLayer point, int index)
        {
            MapPointInLayer curPt = point;
            for (int i = 0; i < index; i++)
            {
                curPt = curPt.TopNeighborInLayer();
                if (curPt == null)
                    return null;
            }
            return curPt;
        }

        /// <summary>
        /// Get right neighbor for point in area depth layer in which area is
        /// </summary>
        /// <param name="index">How much points skip</param>
        /// <returns></returns>
        public static MapPointInLayer RightNeighborInLayer(this MapPointInLayer pt, int index)
        {
            MapPointInLayer curPt = pt;
            for (int i = 0; i < index; i++)
            {
                curPt = curPt.RightNeighborInLayer();
                if (curPt == null)
                    return null;
            }
            return curPt;
        }

        /// <summary>
        /// Get down neighbor for point in area depth layer in which area is
        /// </summary>
        /// <param name="index">How much points skip</param>
        /// <returns></returns>
        public static MapPointInLayer DownNeighborInLayer(this MapPointInLayer pt, int index)
        {
            MapPointInLayer curPt = pt;
            for (int i = 0; i < index; i++)
            {
                curPt = curPt.DownNeighborInLayer();
                if (curPt == null)
                    return null;
            }
            return curPt;
        }

        /// <summary>
        /// Get left neighbor for point in area depth layer in which area is
        /// </summary>
        /// <param name="index">How much points skip</param>
        /// <returns></returns>
        public static MapPointInLayer LeftNeighborInLayer(this MapPointInLayer pt, int index)
        {
            MapPointInLayer curPt = pt;
            for (int i = 0; i < index; i++)
            {
                curPt = curPt.LeftNeighborInLayer();
                if (curPt == null)
                    return null;
            }
            return curPt;
        }

        /// <summary>
        /// Get top neighbor for point in area depth layer in which area is
        /// </summary>
        public static MapPointInLayer TopNeighborInLayer(this MapPointInLayer pt)
        {
            Area area = pt.Area;
            MapPoint point = pt.MapPoint;
            /* Corners */
            if (pt.IsLeftTopInArea)
            {
                if (area.TopNeighbor != null)
                    return new MapPointInLayer(area.TopNeighbor.LeftTopPoint_Val, area.TopNeighbor);
                return null;
            }
            if (pt.IsRightTopInArea)
            {
                if (area.TopNeighbor != null)
                    return new MapPointInLayer(area.TopNeighbor.RightTopPoint_Val, area.TopNeighbor);
                return null;
            }
            if (pt.IsLeftDownInArea)
            {
                return new MapPointInLayer(area.LeftTopPoint_Val, area);
            }
            if (pt.IsRightDownInArea)
            {
                return new MapPointInLayer(area.RightTopPoint_Val, area);
            }

            /* Edges */
            if (pt.IsTopEdgeMiddlePt)
            {
                if (area.TopNeighbor != null)
                    return new MapPointInLayer(area.TopNeighbor.TopEdgeMiddlePt_Val, area.TopNeighbor);
                return null;
            }
            if (pt.IsRightEdgeMiddlePt)
            {
                if (area.TopNeighbor != null)
                    return new MapPointInLayer(area.TopNeighbor.RightEdgeMiddlePt_Val, area.TopNeighbor);
                return null;
            }
            if (pt.IsDownEdgeMiddlePt)
            {
                return new MapPointInLayer(area.TopEdgeMiddlePt_Val, area);
            }
            if (pt.IsLeftEdgeMiddlePt)
            {
                if (area.TopNeighbor != null)
                    return new MapPointInLayer(area.TopNeighbor.LeftEdgeMiddlePt_Val, area.TopNeighbor);
                return null;
            }
            /* Middle point */
            if (pt.IsMiddlePt)
            {
                if (area.TopNeighbor != null)
                    return new MapPointInLayer(area.TopNeighbor.MiddlePt_Val, area.TopNeighbor);
                return null;
            }
            throw new ArgumentException("area doesn't contains point");
        }

        /// <summary>
        /// Get right neighbor for point in depth layer in which area is
        /// </summary>
        public static MapPointInLayer RightNeighborInLayer(this MapPointInLayer pt)
        {
            MapPoint point = pt.MapPoint;
            Area area = pt.Area;
            /* Corners */
            if (pt.IsLeftTopInArea)
            {
                return new MapPointInLayer(area.RightTopPoint_Val, area);
            }
            if (pt.IsRightTopInArea)
            {
                if (area.RightNeighbor != null)
                    return new MapPointInLayer(area.RightNeighbor.RightTopPoint_Val, area.RightNeighbor);
                return null;
            }
            if (pt.IsLeftDownInArea)
            {
                return new MapPointInLayer(area.RightDownPoint_Val, area);
            }
            if (pt.IsRightDownInArea)
            {
                if (area.RightNeighbor != null)
                    return new MapPointInLayer(area.RightNeighbor.RightDownPoint_Val, area.RightNeighbor);
                return null;
            }

            /* Edges */
            if (pt.IsTopEdgeMiddlePt)
            {
                if (area.RightNeighbor != null)
                    return new MapPointInLayer(area.RightNeighbor.TopEdgeMiddlePt_Val, area.RightNeighbor);
                return null;
            }
            if (pt.IsRightEdgeMiddlePt)
            {
                if (area.RightNeighbor != null)
                    return new MapPointInLayer(area.RightNeighbor.RightEdgeMiddlePt_Val, area.RightNeighbor);
                return null;
            }
            if (pt.IsDownEdgeMiddlePt)
            {
                if (area.RightNeighbor != null)
                    return new MapPointInLayer(area.RightNeighbor.DownEdgeMiddlePt_Val, area.RightNeighbor);
                return null;
            }
            if (pt.IsLeftEdgeMiddlePt)
            {
                return new MapPointInLayer(area.RightEdgeMiddlePt_Val, area);
            }

            /* Middle point */
            if (pt.IsMiddlePt)
            {
                if (area.RightNeighbor != null)
                    return new MapPointInLayer(area.RightNeighbor.MiddlePt_Val, area.RightNeighbor);
                return null;
            }
            throw new ArgumentException("area doesn't contains point");
        }

        /// <summary>
        /// Get down neighbor for point in cur depth layer in which area is
        /// </summary>
        public static MapPointInLayer DownNeighborInLayer(this MapPointInLayer pt)
        {
            Area area = pt.Area;
            MapPoint point = pt.MapPoint;
            /* Corners */
            if (pt.IsLeftTopInArea)
            {
                return new MapPointInLayer(area.LeftDownPoint_Val, area);
            }
            if (pt.IsRightTopInArea)
            {
                return new MapPointInLayer(area.RightDownPoint_Val, area);
            }
            if (pt.IsLeftDownInArea)
            {
                if (area.DownNeighbor != null)
                    return new MapPointInLayer(area.DownNeighbor.LeftDownPoint_Val, area.DownNeighbor);
                return null;
            }
            if (pt.IsRightDownInArea)
            {
                if (area.DownNeighbor != null)
                    return new MapPointInLayer(area.DownNeighbor.RightDownPoint_Val, area.DownNeighbor);
                return null;
            }

            /* Edges */
            if (pt.IsTopEdgeMiddlePt)
            {
                return new MapPointInLayer(area.DownEdgeMiddlePt_Val, area);
            }
            if (pt.IsRightEdgeMiddlePt)
            {
                if (area.DownNeighbor != null)
                    return new MapPointInLayer(area.DownNeighbor.RightEdgeMiddlePt_Val, area.DownNeighbor);
                return null;
            }
            if (pt.IsDownEdgeMiddlePt)
            {
                if (area.DownNeighbor != null)
                    return new MapPointInLayer(area.DownNeighbor.DownEdgeMiddlePt_Val, area.DownNeighbor);
                return null;
            }
            if (pt.IsLeftEdgeMiddlePt)
            {
                if (area.DownNeighbor != null)
                    return new MapPointInLayer(area.DownNeighbor.LeftEdgeMiddlePt_Val, area.DownNeighbor);
                return null;
            }

            /* Middle point */
            if (pt.IsMiddlePt)
            {
                if (area.DownNeighbor != null)
                    return new MapPointInLayer(area.DownNeighbor.MiddlePt_Val, area.DownNeighbor);
                return null;
            }

            throw new ArgumentException("area doesn't contains point");
        }

        /// <summary>
        /// Get left neighbor for point in cur depth layer in which area is
        /// </summary>
        public static MapPointInLayer LeftNeighborInLayer(this MapPointInLayer pt)
        {
            MapPoint point = pt.MapPoint;
            Area area = pt.Area;
            /* Corners */
            if (pt.IsLeftTopInArea)
            {
                if (area.LeftNeighbor != null)
                    return new MapPointInLayer(area.LeftNeighbor.LeftTopPoint_Val, area.LeftNeighbor);
                return null;
            }
            if (pt.IsRightTopInArea)
            {
                return new MapPointInLayer(area.LeftTopPoint_Val, area);
            }
            if (pt.IsLeftDownInArea)
            {
                if (area.LeftNeighbor != null)
                    return new MapPointInLayer(area.LeftNeighbor.LeftDownPoint_Val, area.LeftNeighbor);
                return null;
            }
            if (pt.IsRightDownInArea)
            {
                return new MapPointInLayer(area.LeftDownPoint_Val, area);
            }

            /* Edges */
            if (pt.IsTopEdgeMiddlePt)
            {
                if (area.LeftNeighbor != null)
                    return new MapPointInLayer(area.LeftNeighbor.TopEdgeMiddlePt_Val, area.LeftNeighbor);
                return null;
            }
            if (pt.IsRightEdgeMiddlePt)
            {
                return new MapPointInLayer(area.LeftEdgeMiddlePt_Val, area);
            }
            if (pt.IsDownEdgeMiddlePt)
            {
                if (area.LeftNeighbor != null)
                    return new MapPointInLayer(area.LeftNeighbor.DownEdgeMiddlePt_Val, area.LeftNeighbor);
                return null;
            }
            if (pt.IsLeftEdgeMiddlePt)
            {
                if (area.LeftNeighbor != null)
                    return new MapPointInLayer(area.LeftNeighbor.LeftEdgeMiddlePt_Val, area.LeftNeighbor);
                return null;
            }

            /* Middle point */
            if (pt.IsMiddlePt)
            {
                if (area.LeftNeighbor != null)
                    return new MapPointInLayer(area.LeftNeighbor.MiddlePt_Val, area.LeftNeighbor);
                return null;
            }
            throw new ArgumentException("area doesn't contains point");
        }
    }
}
