using World.Common;

namespace World.Model
{
    public class ModelPoint
    {
        /// <summary>
        /// Coord of point in model
        /// </summary>
        public ModelCoord NormalCoord { get; private set; }

        public ModelPointData Data { get; private set; }

        public ModelPoint(ModelCoord coord)
        {
            this.NormalCoord = coord;
            Data = new ModelPointData();
        }
    }
}
