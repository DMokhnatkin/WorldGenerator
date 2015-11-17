using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map.Generator.MapModels;

namespace Map.Generator.MapModels
{
    public class AreaBypass
    {
        static void _DeepestBypass(List<Area> res, Area cur)
        {
            if (!cur.IsSubDivided)
            {
                res.Add(cur);
                return;
            }
            else
            {
                _DeepestBypass(res, cur.LeftTopChild);
                _DeepestBypass(res, cur.RightTopChild);
                _DeepestBypass(res, cur.LeftDownChild);
                _DeepestBypass(res, cur.RightDownChild);
            }
        }

        public static List<Area> DeepestAreas(Area ar)
        {
            List<Area> res = new List<Area>();
            _DeepestBypass(res, ar);
            return res;
        }
    }
}
