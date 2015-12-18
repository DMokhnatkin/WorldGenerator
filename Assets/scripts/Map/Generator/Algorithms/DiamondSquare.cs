using System;
using System.Collections.Generic;
using Map.MapModels;
using Map.Generator.Geometry;
using Map.MapModels.Areas;
using Map.MapModels.Points;
using Map.MapModels.Navigation.Points;

namespace Map.Generator.Algorithms
{
    public class DiamondSquare
    {
        /// <summary>
        /// 0..1
        /// </summary>
        // Max height change(calc from maxHeight) in one chunk (0..1)
        public float harshness = 0.05f;
        public float maxHeight = 1;
        public float chunkSize;

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

        public DiamondSquare(byte maxDepth, float chunkSize)
        {
            _maxDepth = maxDepth;
            this.chunkSize = chunkSize;
        }

        void AddRandOffset(MapPoint pt, float appl)
        {
            float _displacement = ((float)rand.NextDouble() * (2 * appl) - appl);
            if (pt.Height + _displacement > maxHeight)
                _displacement *= -0.5f;
            if (pt.Height + _displacement < 0)
                _displacement *= -0.5f;
            pt.Height += _displacement;
        }

        /// <summary>
        /// Avverage point
        /// </summary>
        /// <returns>If height was setted true</returns>
        bool AveragePoint(MapPointInLayer pt)
        {
            if (!TryInterpolateCubic(pt))
                if (!TryInterpolateAverage(pt))
                    if (!TryExtrapolate(pt))
                    {
                        return false;
                    }
            return true;
        }

        void TrimCornerHeight(MapPointInLayer pt)
        {
            float maxFarHeight = 0;
            // Find the biggest difference between neighbors
            if (pt.TopNeighborInLayer() != null &&
                pt.TopNeighborInLayer().IsGenerated &&
                Math.Abs(pt.Height - pt.TopNeighborInLayer().Height) > Math.Abs(maxFarHeight))
            {
                maxFarHeight = pt.Height - pt.TopNeighborInLayer().Height;
            }
            if (pt.RightNeighborInLayer() != null &&
                pt.RightNeighborInLayer().IsGenerated &&
                Math.Abs(pt.Height - pt.RightNeighborInLayer().Height) > Math.Abs(maxFarHeight))
            {
                maxFarHeight = pt.Height - pt.RightNeighborInLayer().Height;
            }
            if (pt.DownNeighborInLayer() != null &&
                pt.DownNeighborInLayer().IsGenerated &&
                Math.Abs(pt.Height - pt.DownNeighborInLayer().Height) > Math.Abs(maxFarHeight))
            {
                maxFarHeight = pt.Height - pt.DownNeighborInLayer().Height;
            }
            if (pt.LeftNeighborInLayer() != null &&
                pt.LeftNeighborInLayer().IsGenerated &&
                Math.Abs(pt.Height - pt.LeftNeighborInLayer().Height) > Math.Abs(maxFarHeight))
            {
                maxFarHeight = pt.Height - pt.LeftNeighborInLayer().Height;
            }

            // If difference is too big, trim height
            if (Math.Abs(maxFarHeight) > harshness * maxHeight)
            {
                if (maxFarHeight > 0)
                    pt.Height -= maxFarHeight - harshness * maxHeight;
                else
                    pt.Height += Math.Abs(maxFarHeight) - harshness * maxHeight;
            }
        }

        void InitializeCorners(Area cur)
        {
            // Try average each corner. If we can't, set random value

            AveragePoint(new MapPointInLayer(cur.LeftTopPoint_Val, cur));
            if (!cur.LeftTopPoint_Val.IsGenerated)
                cur.LeftTopPoint_Val.Height = (float)rand.NextDouble() * harshness * maxHeight;
            else
                AddRandOffset(cur.LeftTopPoint_Val, harshness * maxHeight);
            TrimCornerHeight(new MapPointInLayer(cur.LeftTopPoint_Val, cur));

            AveragePoint(new MapPointInLayer(cur.RightTopPoint_Val, cur));
            if (!cur.RightTopPoint_Val.IsGenerated)
                cur.RightTopPoint_Val.Height = (float)rand.NextDouble() * harshness * maxHeight;
            else
                AddRandOffset(cur.RightTopPoint_Val, harshness * maxHeight);
            TrimCornerHeight(new MapPointInLayer(cur.RightTopPoint_Val, cur));

            AveragePoint(new MapPointInLayer(cur.LeftDownPoint_Val, cur));
            if (!cur.LeftDownPoint_Val.IsGenerated)
                cur.LeftDownPoint_Val.Height = (float)rand.NextDouble() * harshness * maxHeight;
            else
                AddRandOffset(cur.LeftDownPoint_Val, harshness * maxHeight);
            TrimCornerHeight(new MapPointInLayer(cur.LeftDownPoint_Val, cur));

            AveragePoint(new MapPointInLayer(cur.RightDownPoint_Val, cur));
            if (!cur.RightDownPoint_Val.IsGenerated)
                cur.RightDownPoint_Val.Height = (float)rand.NextDouble() * harshness * maxHeight;
            else
                AddRandOffset(cur.RightDownPoint_Val, harshness * maxHeight);
            TrimCornerHeight(new MapPointInLayer(cur.RightDownPoint_Val, cur));
        }

        /// <summary>
        /// Try set height to point by interpolating neighbors
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="cur"></param>
        bool TryInterpolateAverage(MapPointInLayer pt)
        {
            float sum = 0;
            int ct = 0;
            MapPointInLayer _leftNeighbor = pt.LeftNeighborInLayer();
            MapPointInLayer _rightNeighbor = pt.RightNeighborInLayer();
            if (_leftNeighbor != null &&
                _leftNeighbor.IsGenerated &&
                _rightNeighbor != null &&
                _rightNeighbor.IsGenerated)
            {
                sum += _leftNeighbor.Height;
                sum += _rightNeighbor.Height;
                ct += 2;
            }

            MapPointInLayer _topNeighbor = pt.TopNeighborInLayer();
            MapPointInLayer _downNeighbor = pt.DownNeighborInLayer();
            // Try use top and down deighbor
            if (_topNeighbor != null &&
                _topNeighbor.IsGenerated &&
                _downNeighbor != null &&
                _downNeighbor.IsGenerated)
            {
                sum += _topNeighbor.Height;
                sum += _downNeighbor.Height;
                ct += 2;
            }

            if (ct != 0)
            {
                pt.Height = sum / (float)ct;
                return true;
            }
            return false;
        }

        bool TryInterpolateCubic(MapPointInLayer pt)
        {
            float sum = 0;
            int ct = 0;

            MapPointInLayer _leftNeighbor = pt.LeftNeighborInLayer();
            MapPointInLayer _leftLeftNeighbor = pt.LeftNeighborInLayer(2); // Left neighbor of left neighbor
            MapPointInLayer _rightNeighbor = pt.RightNeighborInLayer();
            MapPointInLayer _rightRightNeighbor = pt.RightNeighborInLayer(2); // Right neighbor of right neighbor
            // Horizontal
            if (_leftNeighbor != null &&
                _leftNeighbor.IsGenerated &&
                _leftLeftNeighbor != null &&
                _leftLeftNeighbor.IsGenerated &&
                _rightNeighbor != null &&
                _rightNeighbor.IsGenerated &&
                _rightRightNeighbor != null &&
                _rightRightNeighbor.MapPoint.IsGenerated)
            {
                sum += Interpolation.Cubic4Points(
                    _leftLeftNeighbor.Height,
                    _leftNeighbor.Height,
                    _rightNeighbor.Height,
                    _rightRightNeighbor.Height);
                ct += 1;
            }

            MapPointInLayer _topNeighbor = pt.TopNeighborInLayer();
            MapPointInLayer _topTopNeighbor = pt.TopNeighborInLayer(2);
            MapPointInLayer _downNeighbor = pt.DownNeighborInLayer();
            MapPointInLayer _downDownNeighbor = pt.DownNeighborInLayer(2);
            // Vertical
            if (_topNeighbor != null &&
                _topNeighbor.IsGenerated &&
                _topTopNeighbor != null &&
                _topTopNeighbor.IsGenerated &&
                _downNeighbor != null &&
                _downNeighbor.IsGenerated &&
                _downDownNeighbor != null &&
                _downDownNeighbor.IsGenerated)
            {
                sum += Interpolation.Cubic4Points(
                    _topTopNeighbor.Height,
                    _topNeighbor.Height,
                    _downNeighbor.Height,
                    _downDownNeighbor.Height);
                ct += 1;
            }
            if (ct != 0)
            {
                pt.Height = sum / (float)ct;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Try set height to point by extrapolating neighbors
        /// </summary>
        bool TryExtrapolate(MapPointInLayer pt)
        {
            float sum = 0;
            int ct = 0;

            MapPointInLayer _topNeighbor = pt.TopNeighborInLayer();
            MapPointInLayer _topTopNeighbor = pt.TopNeighborInLayer(2); // Top neighbor of top neighbor
            // Use top neighbor and its top neighbor
            if (_topNeighbor != null &&
                _topNeighbor.IsGenerated &&
                _topTopNeighbor != null &&
                _topTopNeighbor.IsGenerated)
            {
                sum +=
                    (2 * _topNeighbor.Height -
                     _topTopNeighbor.Height);
                ct++;
            }

            MapPointInLayer _rightNeighbor = pt.RightNeighborInLayer();
            MapPointInLayer _rightRightNeighbor = pt.RightNeighborInLayer(2);
            // Use right neighbor and its right neighbor
            if (_rightNeighbor != null &&
                _rightNeighbor.IsGenerated &&
                _rightRightNeighbor != null &&
                _rightRightNeighbor.IsGenerated)
            {
                sum +=
                    (2 * _rightNeighbor.Height -
                     _rightRightNeighbor.Height);
                ct++;
            }

            MapPointInLayer _downNeighbor = pt.DownNeighborInLayer();
            MapPointInLayer _downDownNeighbor = pt.DownNeighborInLayer(2);
            // Use down neighbor and its down neighbor
            if (_downNeighbor != null &&
                _downNeighbor.IsGenerated &&
                _downDownNeighbor != null &&
                _downDownNeighbor.IsGenerated)
            {
                sum +=
                    (2 * _downNeighbor.Height -
                     _downDownNeighbor.Height);
                ct++;
            }

            MapPointInLayer _leftNeighbor = pt.LeftNeighborInLayer();
            MapPointInLayer _leftLeftNeighbor = pt.LeftNeighborInLayer(2);
            // Use left neighbor and its left neighbor
            if (_leftNeighbor != null &&
                _leftNeighbor.IsGenerated &&
                _leftLeftNeighbor != null &&
                _leftLeftNeighbor.IsGenerated)
            {
                sum +=
                    (2 * _leftNeighbor.Height -
                     _leftLeftNeighbor.Height);
                ct++;
            }

            if (ct != 0)
            {
                pt.Height = sum / (float)ct;
                return true;
            }
            return false;
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
                    InitializeCorners(z);
                }
            }
            if (curDepth >= depth)
                return;
            float appl = harshness * maxHeight / (float)GetPow2(curDepth + 1);
            Queue<Area> nextLayer = new Queue<Area>();
            // Square current layer and collect areas to nextLayer
            foreach (Area z in curLayer)
            {
                if (!z.IsSubDivided)
                    z.Divide();
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
                if (!AveragePoint(new MapPointInLayer(z.LeftTopChild.RightTopPoint_Val, z.LeftTopChild)))
                    throw new ArgumentException();
                if (!AveragePoint(new MapPointInLayer(z.RightTopChild.RightDownPoint_Val, z.RightTopChild)))
                    throw new ArgumentException();
                if (!AveragePoint(new MapPointInLayer(z.RightDownChild.LeftDownPoint_Val, z.RightDownChild)))
                    throw new ArgumentException();
                if (!AveragePoint(new MapPointInLayer(z.LeftDownChild.LeftTopPoint_Val, z.LeftDownChild)))
                    throw new ArgumentException();
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
