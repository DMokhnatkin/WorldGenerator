using System;
using Map.MapModels;
using Map.MapModels.Areas;
using Map.MapModels.Points;
using Map.MapModels.Extensions;
using System.Collections.Generic;
using Map.MapModels.Navigation.Points;

namespace Map.Generator.Algorithms.Erosion
{
    public class WaterErosion
    {
        public float raininess = 1.0f;
        /// <summary>
        /// If angle is less then critical, flood power will decreasing.
        /// If angle is more then critical,flood power will be increasing.
        /// </summary>
        public float criticalAngle = 0.26f;

        private void _CalcErosionStrength(MapPointInLayer pt, HashSet<MapPointInLayer> notVisited, float edgeLength)
        {
            if (!notVisited.Contains(pt))
                return;
            notVisited.Remove(pt);
            int ct = 0;
            if (pt.TopNeighborInLayer() != null &&
                pt.TopNeighborInLayer().Height < pt.Height)
                ct++;
            if (pt.RightNeighborInLayer() != null &&
                pt.RightNeighborInLayer().Height < pt.Height)
                ct++;
            if (pt.DownNeighborInLayer() != null &&
                pt.DownNeighborInLayer().Height < pt.Height)
                ct++;
            if (pt.LeftNeighborInLayer() != null &&
                pt.LeftNeighborInLayer().Height < pt.Height)
                ct++;
            
            if (pt.TopNeighborInLayer() != null &&
                pt.TopNeighborInLayer().Height < pt.Height)
            {
                // Positive
                float fallSlope = (pt.Height - pt.TopNeighborInLayer().Height) / edgeLength;
                pt.TopNeighborInLayer().NatureConf.flood += 
                    (pt.NatureConf.flood / ct + edgeLength * raininess) * (fallSlope + 1);
#if DEBUG
                pt.NatureConf.falledTo.Add(pt.TopNeighborInLayer());
#endif
                _CalcErosionStrength(pt.TopNeighborInLayer(), notVisited, edgeLength);
            }
            if (pt.RightNeighborInLayer() != null &&
                pt.RightNeighborInLayer().Height < pt.Height)
            {
                // Positive
                float fallSlope = (pt.Height - pt.TopNeighborInLayer().Height) / edgeLength;
                pt.RightNeighborInLayer().NatureConf.flood += 
                    (pt.NatureConf.flood / ct + edgeLength * raininess) * (fallSlope + 1);
#if DEBUG
                pt.NatureConf.falledTo.Add(pt.RightNeighborInLayer());
#endif
                _CalcErosionStrength(pt.RightNeighborInLayer(), notVisited, edgeLength);
            }
            if (pt.DownNeighborInLayer() != null &&
                pt.DownNeighborInLayer().Height < pt.Height)
            {
                // Positive
                float fallSlope = (pt.Height - pt.TopNeighborInLayer().Height) / edgeLength;
                pt.DownNeighborInLayer().NatureConf.flood += 
                    (pt.NatureConf.flood / ct + edgeLength * raininess) * (fallSlope + 1);
#if DEBUG
                pt.NatureConf.falledTo.Add(pt.DownNeighborInLayer());
#endif
                _CalcErosionStrength(pt.DownNeighborInLayer(), notVisited, edgeLength);
            }
            if (pt.LeftNeighborInLayer() != null && 
                pt.LeftNeighborInLayer().Height < pt.Height)
            {
                // Positive
                float fallSlope = (pt.Height - pt.TopNeighborInLayer().Height) / edgeLength;
                pt.LeftNeighborInLayer().NatureConf.flood += 
                    (pt.NatureConf.flood / ct + edgeLength * raininess) * (fallSlope + 1);
#if DEBUG
                pt.NatureConf.falledTo.Add(pt.LeftNeighborInLayer());
#endif
                _CalcErosionStrength(pt.LeftNeighborInLayer(), notVisited, edgeLength);
            }
        }

        /// <summary>
        /// Only for generated area
        /// </summary>
        /// <param name="edgeLength">Length of edge (in cur detalization depth)</param>
        public void CalcErosionStrength(Area area, float edgeLength)
        {
            MapPointInLayer[,] pts = area.UnwrapPoints();
            List<MapPointInLayer> sorted = new List<MapPointInLayer>();
            HashSet<MapPointInLayer> notVisited = new HashSet<MapPointInLayer>();
            // TODO optimize sort
            foreach (MapPointInLayer z in pts)
            {
                sorted.Add(z);
                notVisited.Add(z);
            }
            sorted.Sort((x, y) => y.Height.CompareTo(x.Height));
            for (int i = 0; i < sorted.Count; i++)
            {
                if (notVisited.Contains(sorted[i]))
                {
                    sorted[i].NatureConf.flood = 1;
                    _CalcErosionStrength(sorted[i], notVisited, edgeLength);
                }
            }
        }
    }
}
