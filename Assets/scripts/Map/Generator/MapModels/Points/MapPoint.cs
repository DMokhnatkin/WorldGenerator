using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map.Generator.MapModels
{
    public class MapPoint
    {
        bool _generated = false;

        float _height = float.NaN;

        public float Height
        {
            get
            {
                if (!_generated)
                    throw new ArgumentException("Point wasn't generated");
                return _height;
            }
            set
            {
                _height = value;
                _generated = true;
            }
        }

        public MapPoint()
        {
        }

        public bool IsGenerated
        {
            get { return _generated; }
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

        public bool IsMiddlePt(Area area)
        {
            return object.ReferenceEquals(area.MiddlePt_Val, this);
        }

        public bool IsInArea(Area area)
        {
            return IsLeftTopInArea(area) || IsRightTopInArea(area) || 
                IsLeftDownInArea(area) || IsRightDownInArea(area) ||
                IsMiddlePt(area);
        }

        public bool IsRightDownInArea(Area area)
        {
            return object.ReferenceEquals(area.RightDownPoint_Val, this);
        }
    }
}
