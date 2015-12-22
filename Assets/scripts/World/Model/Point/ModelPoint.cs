using World.Common;

namespace World.Model
{
    public class ModelPoint
    {
        public ModelCoord NormalCoord { get; internal set; }

        public ModelPointData Data { get; private set; }

        internal ModelPoint(ModelCoord normalCoord)
        {
            Data = new ModelPointData();
            NormalCoord = normalCoord;
        }
    }
}
