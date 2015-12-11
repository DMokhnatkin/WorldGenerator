﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map.Generator.Algorithms.Erosion;

namespace Map.MapModels.Points
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

        public WaterErosionMapPointData WaterErosion { get; private set; }

        public MapPoint()
        {
            WaterErosion = new WaterErosionMapPointData();
        }

        public bool IsGenerated
        {
            get { return _generated; }
        }
    }
}
