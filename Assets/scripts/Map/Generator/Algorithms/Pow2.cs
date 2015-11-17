using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map.Generator.MapModels;

namespace Map.Generator.Algorithms
{
    public class Pow2
    {
        public static void Relax(Area a)
        {
            List<Area> res = AreaBypass.DeepestAreas(a);
            foreach (Area t in res)
            {
                t.LeftTopPoint_Val.Height *= t.LeftTopPoint_Val.Height;
            }
        }
    }
}
