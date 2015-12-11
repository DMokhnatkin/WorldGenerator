using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map.MapModels.Areas
{
    /// <summary>
    /// Some extra operations for area
    /// </summary>
    public static class AreaExtensions
    {
        /// <summary>
        /// Calc depth of area
        /// </summary>
        public static byte CalcDepth(this Area cur)
        {
            byte depth = 0;
            Area z = cur;
            while (z.IsSubDivided)
            {
                z = z.LeftTopChild;
                depth++;
            }
            return depth;
        }
        

        /// <summary>
        /// Do some action for each element near top border of area
        /// </summary>
        public static void ForEachTopBorder(this Area area, Action<Area> action)
        {
            if (area.IsSubDivided)
            {
                ForEachTopBorder(area.LeftTopChild, action);
                ForEachTopBorder(area.RightTopChild, action);
            }
            else
                action(area);
        }

        /// <summary>
        /// Do some action for each element near right border of area
        /// </summary>
        public static void ForEachRightBorder(this Area area, Action<Area> action)
        {
            if (area.IsSubDivided)
            {
                ForEachRightBorder(area.RightTopChild, action);
                ForEachRightBorder(area.RightDownChild, action);
            }
            else
                action(area);
        }

        /// <summary>
        /// Do some action for each element near down border of area
        /// </summary>
        public static void ForEachDownBorder(this Area area, Action<Area> action)
        {
            if (area.IsSubDivided)
            {
                ForEachDownBorder(area.LeftDownChild, action);
                ForEachDownBorder(area.RightDownChild, action);
            }
            else
                action(area);
        }

        /// <summary>
        /// Do some action for each element near left border of area
        /// </summary>
        public static void ForEachLeftBorder(this Area area, Action<Area> action)
        {
            if (area.IsSubDivided)
            {
                ForEachLeftBorder(area.LeftTopChild, action);
                ForEachLeftBorder(area.LeftDownChild, action);
            }
            else
                action(area);
        }
    }
}
