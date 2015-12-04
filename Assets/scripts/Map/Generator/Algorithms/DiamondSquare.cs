using UnityEngine;
using System;
using System.Collections.Generic;
using Map.Generator.MapModels;
using System.IO;

namespace Map.Generator.Algorithms
{
    public class DiamondSquare
    {
        /// <summary>
        /// 0..1
        /// </summary>
        public float strength = 0.1f;
        public float minHeight = 0;
        public float maxHeight = 1;

        System.Random rand = new System.Random();

        // For fast pow(2, ?) operation
        List<int> pow2 = new List<int>(new int[] { 1, 2, 4, 8, 16, 32, 64 });

        /// Stores generated depth of each area
        Dictionary<Area, int> depths = new Dictionary<Area, int>();

        // To store generated areas for extend resolution
        Dictionary<Area, Queue<Area>> cash = new Dictionary<Area, Queue<Area>>();

        // To delete already generated areas from queue
        private byte _maxDepth;

        int GetPow2(int degree)
        {
            while (pow2.Count <= degree)
                pow2.Add(pow2[pow2.Count - 1] * 2);
            return pow2[degree];
        }

        public DiamondSquare(byte maxDepth)
        {
            _maxDepth = maxDepth;
        }

        void AddRandOffset(MapPoint pt, float appl)
        {
            float maxOffset = appl * strength;
            float _displacement = ((float)rand.NextDouble() * (2 * maxOffset) - maxOffset);
            if (pt.Height + _displacement > maxHeight)
                _displacement *= -0.5f;
            if (pt.Height + _displacement < minHeight)
                _displacement *= -0.5f;
            pt.Height += _displacement;
        }

        /// <summary>
        /// Try set height to point by interpolating neighbors
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="cur"></param>
        void TryInterpolate(MapPoint pt, Area cur)
        {
            float sum = 0;
            int ct = 0;

            // Try use left and right neighbor
            if (pt.LeftNeighborInLayer(cur) != null &&
                pt.RightNeighborInLayer(cur) != null)
            {
                sum += pt.LeftNeighborInLayer(cur).Height;
                sum += pt.RightNeighborInLayer(cur).Height;
                ct += 2;
            }

            // Try use top and down deighbor
            if (pt.TopNeighborInLayer(cur) != null &&
                pt.DownNeighborInLayer(cur) != null)
            {
                sum += pt.TopNeighborInLayer(cur).Height;
                sum += pt.DownNeighborInLayer(cur).Height;
                ct += 2;
            }

            if (ct != 0)
                pt.Height = sum / (float)ct;
        }

        /// <summary>
        /// Try set height to point by extrapolating neighbors
        /// </summary>
        void TryExtrapolate(MapPoint pt, Area cur)
        {
            float sum = 0;
            int ct = 0;
            // Use top neighbor and its top neighbor
            if (pt.TopNeighborInLayer(cur) != null &&
                pt.TopNeighborInLayer(cur).TopNeighborInLayer(cur) != null)
            {
                sum +=
                    (2 * pt.TopNeighborInLayer(cur).Height -
                     pt.TopNeighborInLayer(cur).TopNeighborInLayer(cur).Height);
                ct++;
            }

            // Use right neighbor and its right neighbor
            if (pt.RightNeighborInLayer(cur) != null &&
                pt.RightNeighborInLayer(cur).RightNeighborInLayer(cur) != null)
            {
                sum +=
                    (2 * pt.RightNeighborInLayer(cur).Height -
                     pt.RightNeighborInLayer(cur).RightNeighborInLayer(cur).Height);
                ct++;
            }

            // Use down neighbor and its down neighbor
            if (pt.DownNeighborInLayer(cur) != null &&
                pt.DownNeighborInLayer(cur).DownNeighborInLayer(cur) != null)
            {
                sum +=
                    (2 * pt.DownNeighborInLayer(cur).Height -
                     pt.DownNeighborInLayer(cur).DownNeighborInLayer(cur).Height);
                ct++;
            }

            // Use left neighbor and its left neighbor
            if (pt.LeftNeighborInLayer(cur) != null &&
                pt.LeftNeighborInLayer(cur).LeftNeighborInLayer(cur) != null)
            {
                sum +=
                    (2 * pt.LeftNeighborInLayer(cur).Height -
                     pt.LeftNeighborInLayer(cur).LeftNeighborInLayer(cur).Height);
                ct++;
            }

            if (ct != 0)
                pt.Height = sum / (float)ct;
        }

        /// <summary>
        /// Avverage point
        /// </summary>
        /// <returns>If height was setted true</returns>
        void AveragePoint(MapPoint pt, Area cur)
        {
            /* Use interpolating */

            // Try use top, right, down and left neighbors
            if (pt.TopNeighborInLayer(cur) != null &&
                pt.RightNeighborInLayer(cur) != null &&
                pt.DownNeighborInLayer(cur) != null &&
                pt.LeftNeighborInLayer(cur) != null)
            {
                pt.Height = (pt.TopNeighborInLayer(cur).Height +
                    pt.RightNeighborInLayer(cur).Height +
                    pt.DownNeighborInLayer(cur).Height +
                    pt.LeftNeighborInLayer(cur).Height) / 4.0f;
                return;
            }

            // Try use left and right neighbor
            if (pt.LeftNeighborInLayer(cur) != null &&
                pt.RightNeighborInLayer(cur) != null)
            {
                pt.Height = (pt.LeftNeighborInLayer(cur).Height +
                    pt.RightNeighborInLayer(cur).Height) / 2.0f;
                return;
            }

            // Try use top and down deighbor
            if (pt.TopNeighborInLayer(cur) != null &&
                pt.DownNeighborInLayer(cur) != null)
            {
                pt.Height = (pt.TopNeighborInLayer(cur).Height +
                    pt.DownNeighborInLayer(cur).Height) / 2.0f;
                return;
            }

            /* Use extrapolating */
            TryExtrapolate(pt, cur);

            return;
        }

        void CalcCorners(Area cur, float appl)
        {
            if (!cur.LeftTopPoint_Val.IsGenerated)
            {
                AveragePoint(cur.LeftTopPoint_Val, cur);
                if (!cur.LeftTopPoint_Val.IsGenerated)
                    cur.LeftTopPoint_Val.Height = (float)rand.NextDouble() * appl;
                AddRandOffset(cur.LeftTopPoint_Val, appl);
            }

            if (!cur.RightTopPoint_Val.IsGenerated)
            {
                AveragePoint(cur.RightTopPoint_Val, cur);
                if (!cur.RightTopPoint_Val.IsGenerated)
                    cur.RightTopPoint_Val.Height = (float)rand.NextDouble() * appl;
                AddRandOffset(cur.RightTopPoint_Val, appl);
            }

            if (!cur.LeftDownPoint_Val.IsGenerated)
            {
                AveragePoint(cur.LeftDownPoint_Val, cur);
                if (!cur.LeftDownPoint_Val.IsGenerated)
                    cur.LeftDownPoint_Val.Height = (float)rand.NextDouble() * appl;
                AddRandOffset(cur.LeftDownPoint_Val, appl);
            }

            if (!cur.RightDownPoint_Val.IsGenerated)
            {
                AveragePoint(cur.RightDownPoint_Val, cur);
                if (!cur.RightDownPoint_Val.IsGenerated)
                    cur.RightDownPoint_Val.Height = (float)rand.NextDouble() * appl;
                AddRandOffset(cur.RightDownPoint_Val, appl);
            }
        }

        void Square(Area cur, float appl)
        {
            // We have 4 corners, generate middle pt
            cur.MiddlePt_Val.Height =
            (cur.LeftTopPoint_Val.Height + cur.RightTopPoint_Val.Height +
            cur.LeftDownPoint_Val.Height + cur.RightDownPoint_Val.Height) / 4.0f;

            AddRandOffset(cur.MiddlePt_Val, appl);
        }

        Queue<Area> curLayer = new Queue<Area>();

        void Pogr(int curDepth, int depth)
        {
            if (curDepth == 0)
            {
                foreach (Area z in curLayer)
                {
                    CalcCorners(z, (maxHeight - minHeight) / 2.0f);
                }
            }
            if (curDepth >= depth)
                return;
            float appl = (maxHeight - minHeight) / (float)GetPow2(curDepth + 1);
            Queue<Area> nextLayer = new Queue<Area>();
            // Square current layer and collect areas to nextLayer
            foreach (Area z in curLayer)
            {
                if (!z.IsSubDivided)
                    z.Subdivide();
                if (!z.MiddlePt_Val.IsGenerated)
                    Square(z, appl);

                // If it is max depth, ignore childs
                if (curDepth + 1 >= _maxDepth)
                    continue;
                nextLayer.Enqueue(z.LeftTopChild);
                nextLayer.Enqueue(z.RightTopChild);
                nextLayer.Enqueue(z.LeftDownChild);
                nextLayer.Enqueue(z.RightDownChild);
            }
            // Diamond current layer
            foreach (Area z in curLayer)
            {
                AveragePoint(z.LeftTopChild.RightTopPoint_Val, z.LeftTopChild);
                AveragePoint(z.RightTopChild.RightDownPoint_Val, z.RightTopChild);
                AveragePoint(z.RightDownChild.LeftDownPoint_Val, z.RightDownChild);
                AveragePoint(z.LeftDownChild.LeftTopPoint_Val, z.LeftDownChild);
                //Diamond(z);
            }

            curLayer = nextLayer;
            nextLayer = new Queue<Area>();
            if (curDepth + 1 < depth)
                Pogr(curDepth + 1, depth);
        }

        public void ExtendResolution(Area area, byte depth)
        {
            if (depth > _maxDepth)
                throw new ArgumentException("depth > _maxDepth");

            // Get already generated depth (if exists)
            int curDepth = 0;
            if (depths.ContainsKey(area))
                curDepth = depths[area];

            if (depth > curDepth)
            {
                // Get founded before layer (if exists)
                if (cash.ContainsKey(area))
                    curLayer = cash[area];
                else
                    curLayer = new Queue<Area>(new Area[] { area });

                Pogr(curDepth, depth);

                if (depth == _maxDepth)
                    // We extend this area to maxDepth
                    // We can remove this area from cash, 
                    // because we won't extend this area in future
                    cash.Remove(area);
                else
                {
                    if (cash.ContainsKey(area))
                        cash[area] = curLayer;
                    else
                        cash.Add(area, curLayer);
                }

                // Save generated depth
                if (depths.ContainsKey(area))
                    depths[area] = depth;
                else
                    depths.Add(area, depth);
            }
        }
    }
}
