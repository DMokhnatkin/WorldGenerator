using Map.MapModels.Common;

namespace Map.MapModels.WorldModel
{
    public class WorldPoint
    {
        /// <summary>
        /// Coord of point in model
        /// </summary>
        public Coord NormalCoord { get; private set; }

        public WorldPointData Data { get; private set; }

        public WorldPoint(Coord coord)
        {
            this.NormalCoord = coord;
            Data = new WorldPointData();
        }
    }
}
