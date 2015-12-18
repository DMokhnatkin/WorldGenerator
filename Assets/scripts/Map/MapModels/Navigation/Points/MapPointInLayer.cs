using System;
using Map.MapModels.Areas;
using Map.MapModels.Points;
using System.Collections.Generic;

namespace Map.MapModels.Navigation.Points
{
    /// <summary>
    /// Map point with specifed parent area
    /// </summary>
    public class MapPointInLayer : IMapPoint, IEqualityComparer<MapPointInLayer>
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

        public MapPointNatureConfig NatureConf
        {
            get { return MapPoint.NatureConf; }
        }

        public int Id
        {
            get
            {
                if (IsLeftTopInArea)
                {
                    return Area.LeftTopPoint_Id;
                }
                if (IsRightTopInArea)
                {
                    return Area.RightTopPoint_Id;
                }
                if (IsLeftDownInArea)
                {
                    return Area.LeftDownPoint_Id;
                }
                if (IsRightDownInArea)
                {
                    return Area.RightDownPoint_Id;
                }
                throw new ArgumentException("Area is not parent of point");
            }
        }

        public float HeightAfterWaterErosion
        {
            get; set;
        }

        public bool Equals(MapPointInLayer x, MapPointInLayer y)
        {
            return x.Equals(y);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            var z = (MapPointInLayer)obj;
            if (Id != z.Id)
                return false;
            if (object.ReferenceEquals(Area, z.Area))
                return true;
            // Now we need verify areas around (one point can be in 4 neighbor areas)
            if (IsLeftTopInArea)
            {
                if (z.IsLeftDownInArea &&
                    z.Area == Area.TopNeighbor)
                    return true;
                if (z.IsRightTopInArea &&
                    z.Area == Area.LeftNeighbor)
                    return true;
                if (z.IsRightDownInArea &&
                    z.Area == Area.LeftTopNeighbor())
                    return true;
            }
            if (IsRightTopInArea)
            {
                if (z.IsRightDownInArea &&
                    z.Area == Area.TopNeighbor)
                    return true;
                if (z.IsLeftTopInArea &&
                    z.Area == Area.RightNeighbor)
                    return true;
                if (z.IsLeftDownInArea &&
                    z.Area == Area.TopRightNeighbor())
                    return true;
            }
            if (IsLeftDownInArea)
            {
                if (z.IsRightDownInArea &&
                    z.Area == Area.LeftNeighbor)
                    return true;
                if (z.IsLeftTopInArea &&
                    z.Area == Area.DownNeighbor)
                    return true;
                if (z.IsRightTopInArea &&
                    z.Area == Area.DownLeftNeighbor())
                    return true;
            }
            if (IsRightDownInArea)
            {
                if (z.IsLeftDownInArea &&
                    z.Area == Area.RightNeighbor)
                    return true;
                if (z.IsRightTopInArea &&
                    z.Area == Area.DownNeighbor)
                    return true;
                if (z.IsLeftTopInArea &&
                    z.Area == Area.RightDownNeighbor())
                    return true;
            }
            return false;
        }

        public int GetHashCode(MapPointInLayer obj)
        {
            return obj.GetHashCode();
        }
    }
}
