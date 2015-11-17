using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map.Generator.MapModels
{
    public class MapVertex
    {
        public float Height
        {
            get;
            set;
        }

        public MapVertex()
        {
            Height = float.NaN;
        }

        public bool IsGenerated
        {
            get { return !float.IsNaN(Height); }
        }
    }
}
