using World.Common;

namespace World.Model
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
