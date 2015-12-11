using System;
using Map.MapModels.Areas;
using Map.MapModels.Points;

namespace Map.MapModels.Navigation.Points
{
    /// <summary>
    /// Map point with specifed parent area
    /// </summary>
    public class MapPointInLayer : IMapPoint
    {
        public MapPoint MapPoint { get; private set; }
        public Area Area { get; private set; }

        public MapPointInLayer(MapPoint pt, Area parent)
        {
            MapPoint = pt;
            Area = parent;
        }

        public bool IsLeftTopInArea
        {
            get { return object.ReferenceEquals(Area.LeftTopPoint_Val, MapPoint); }
        }

        public bool IsRightTopInArea
        {
            get { return object.ReferenceEquals(Area.RightTopPoint_Val, MapPoint); }
        }

        public bool IsLeftDownInArea
        {
            get { return object.ReferenceEquals(Area.LeftDownPoint_Val, MapPoint); }
        }

        public bool IsRightDownInArea
        {
            get { return object.ReferenceEquals(Area.RightDownPoint_Val, MapPoint); }
        }

        public bool IsMiddlePt
        {
            get { return object.ReferenceEquals(Area.MiddlePt_Val, MapPoint); }
        }

        public bool IsTopEdgeMiddlePt
        {
            get { return object.ReferenceEquals(Area.TopEdgeMiddlePt_Val, MapPoint); }
        }

        public bool IsRightEdgeMiddlePt
        {
            get { return object.ReferenceEquals(Area.RightEdgeMiddlePt_Val, MapPoint); }
        }

        public bool IsDownEdgeMiddlePt
        {
            get { return object.ReferenceEquals(Area.DownEdgeMiddlePt_Val, MapPoint); }
        }

        public bool IsLeftEdgeMiddlePt
        {
            get { return object.ReferenceEquals(Area.LeftEdgeMiddlePt_Val, MapPoint); }
        }

        public float Height
        {
            get { return MapPoint.Height; }
            set { MapPoint.Height = value; }
        }

        public bool IsGenerated
        {
            get { return MapPoint.IsGenerated; }
        }
    }
}
