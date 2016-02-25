using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using World.DataStructures;
using World.DataStructures.ChunksGrid;

namespace World.Generator.Algorithms.River
{
    public class RiverMap
    {
        /// <summary>
        /// Map of river flows
        /// </summary>
        public readonly PointsStorage<RiverData> riverData = new PointsStorage<RiverData>();

        public readonly PointsStorage<RiverSkeletonData> riverSkeletons = new PointsStorage<RiverSkeletonData>();

        /// <summary>
        /// List of river sources
        /// TODO: optimize
        /// </summary>
        public readonly List<IntCoord> riverSources = new List<IntCoord>();

        /// <summary>
        /// Find nearest source
        /// </summary>
        public IntCoord FindNearestSource(IntCoord coord)
        {
            float minDist = float.MaxValue;
            IntCoord res = new IntCoord();
            foreach (IntCoord z in riverSources)
            {
                if (Math.Sqrt(Math.Pow(z.x - coord.x, 2) + Math.Pow(z.y - coord.y, 2)) < minDist)
                {
                    minDist = (float)Math.Sqrt(Math.Pow(z.x - coord.x, 2) + Math.Pow(z.y - coord.y, 2));
                    res = z;
                }
            }
            return res;
        }

        /// <summary>
        /// Distance to nearest source
        /// </summary>
        public float DistToNearestSource(IntCoord coord)
        {
            IntCoord res = FindNearestSource(coord);
            return (float)Math.Sqrt(Math.Pow(res.x - coord.x, 2) + Math.Pow(res.y - coord.y, 2));
        }

        /// <summary>
        /// Create river source
        /// </summary>
        public void CreateSource(IntCoord coord)
        {
            riverSources.Add(coord);
        }
    }
}
