using Map.MapModels.Areas;

namespace Map.MapModels.Points
{
    /// <summary>
    /// Map point with specifed parent area
    /// </summary>
    public class MapPointInLayer
    {
        MapPoint mapPoint;
        Area area;

        public MapPointInLayer(MapPoint pt, Area parent)
        {
            mapPoint = pt;
            area = parent;
        }
    }
}
