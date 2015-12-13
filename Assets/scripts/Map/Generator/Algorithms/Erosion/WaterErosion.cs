using System;
using Map.MapModels;
using Map.MapModels.Areas;
using Map.MapModels.Points;

namespace Map.Generator.Algorithms.Erosion
{
    public class WaterErosion
    {
        /// <summary>
        /// Only for generated area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="radius"></param>
        private static void CalcErosionStrength(Area area, int radius)
        {
            Area[,] areas = area.GetAreasAround(radius);
            foreach (Area a in areas)
            {
                if (a == null)
                    continue;
            }
        }

        public static void ExtendResolution(Area area, WaterErosionSettings sett, int scale, byte depth)
        {
            

            if (!area.IsSubDivided ||
                !area.TopEdgeMiddlePt_Val.IsGenerated ||
                !area.RightEdgeMiddlePt_Val.IsGenerated ||
                !area.DownEdgeMiddlePt_Val.IsGenerated ||
                !area.LeftEdgeMiddlePt_Val.IsGenerated ||
                !area.MiddlePt_Val.IsGenerated)
                throw new ArgumentException(
                    String.Format(
                        "You must generate heights before add water erosion in area = {0}",
                        area.ToString()));
            MapPoint[] pts = new MapPoint[5] 
            {
                area.TopEdgeMiddlePt_Val,
                area.RightEdgeMiddlePt_Val,
                area.DownEdgeMiddlePt_Val,
                area.LeftEdgeMiddlePt_Val,
                area.MiddlePt_Val
            };
            foreach (MapPoint z in pts)
            {
                
            }
        }
    }
}
