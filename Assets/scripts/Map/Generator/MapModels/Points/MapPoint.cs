using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map.Generator.MapModels
{
    public class MapPoint
    {
        public float Height
        {
            get;
            set;
        }

        public MapPoint()
        {
            Height = float.NaN;
        }

        public bool IsGenerated
        {
            get { return !float.IsNaN(Height); }
        }

        public bool IsLeftTopInArea(Area area)
        {
            return object.ReferenceEquals(area.LeftTopPoint_Val, this);
        }

        public bool IsRightTopInArea(Area area)
        {
            return object.ReferenceEquals(area.RightTopPoint_Val, this);
        }

        public bool IsLeftDownInArea(Area area)
        {
            return object.ReferenceEquals(area.LeftDownPoint_Val, this);
        }

        public bool IsRightDownInArea(Area area)
        {
            return object.ReferenceEquals(area.RightDownPoint_Val, this);
        }
    }
}
