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

        /// <summary>
        /// Stores generated depth of each area
        /// </summary>
        Dictionary<Area, int> depths = new Dictionary<Area, int>();

        System.Random rand = new System.Random();

        // For fast pow(2, ?) operation
        List<int> pow2 = new List<int>(new int[] { 1, 2, 4, 8, 16, 32, 64 });

        int GetPow2(int degree)
        {
            while (pow2.Count <= degree)
                pow2.Add(pow2[pow2.Count - 1] * 2);
            return pow2[degree];
        }

        /// <summary>
        /// Calculate or get random leftTop point val
        /// </summary>
        /// <param name="cur"></param>
        void CalculateLeftTopValue(Area cur)
        {
            if (cur.LeftTopPoint_Val.IsGenerated)
                return;
            // Try calculate leftTopPoint val
            // There are 4 cases
            // ?    point which value we want to calculate
            // +    point which value we will use (to calculate ? point)
            // |,-  border of curent area
            if (cur.LeftDownPoint_Val.IsGenerated &&
                cur.LeftEdgeMiddlePt_Val.IsGenerated)
            {
                if (cur.MiddlePt_Val.IsGenerated &&
                    cur.LeftNeighbor != null &&
                    cur.LeftNeighbor.MiddlePt_Val != null &&
                    cur.LeftNeighbor.MiddlePt_Val.IsGenerated)
                {
                    // #2
                    // *|?**
                    // +|++*
                    // *|+**
                    cur.LeftTopPoint_Val.Height =
                        4 * cur.LeftEdgeMiddlePt_Val.Height -
                        cur.MiddlePt_Val.Height -
                        cur.LeftDownPoint_Val.Height -
                        cur.LeftNeighbor.MiddlePt_Val.Height;
                    return;
                }
                else
                {
                    // #1
                    // ?**
                    // +**
                    // +**
                    cur.LeftTopPoint_Val.Height =
                        2 * cur.LeftEdgeMiddlePt_Val.Height -
                        cur.LeftDownPoint_Val.Height;
                }
            }
            else
            {
                if (cur.RightTopPoint_Val.IsGenerated &&
                    cur.TopEdgeMiddlePt_Val.IsGenerated)
                {
                    if (cur.MiddlePt_Val.IsGenerated &&
                        cur.TopNeighbor != null &&
                        cur.TopNeighbor.MiddlePt_Val != null &&
                        cur.TopNeighbor.MiddlePt_Val.IsGenerated)
                    {
                        // #4
                        // *+*
                        // ---
                        // ?++
                        // *+*
                        // ***
                        cur.LeftTopPoint_Val.Height =
                            4 * cur.TopEdgeMiddlePt_Val.Height -
                            cur.TopNeighbor.MiddlePt_Val.Height -
                            cur.RightTopPoint_Val.Height -
                            cur.MiddlePt_Val.Height;
                    }
                    else
                    {
                        // #3
                        // ?++
                        // ***
                        // ***
                        cur.LeftTopPoint_Val.Height =
                            2 * cur.TopEdgeMiddlePt_Val.Height -
                            cur.RightTopPoint_Val.Height;
                    }
                }
            }
            if (!cur.LeftTopPoint_Val.IsGenerated)
                // We can't calculate value
                // Generate random value
                cur.LeftTopPoint_Val.Height = (float)rand.NextDouble() * maxHeight;
        }

        /// <summary>
        /// Calculate or get random rightTop point val
        /// </summary>
        void CalculateRightTopValue(Area cur)
        {
            if (cur.RightTopPoint_Val.IsGenerated)
                return;
            // Try calculate rightTopPoint val
            // There are 4 cases
            // ?    point which value we want to calculate
            // +    point which value we will use (to calculate ? point)
            // |,-  border of curent area
            if (cur.RightDownPoint_Val.IsGenerated &&
                cur.RightEdgeMiddlePt_Val.IsGenerated)
            {
                if (cur.MiddlePt_Val.IsGenerated &&
                    cur.RightNeighbor != null &&
                    cur.RightNeighbor.MiddlePt_Val != null &&
                    cur.RightNeighbor.MiddlePt_Val.IsGenerated)
                {
                    // #2
                    // **?|*
                    // *++|+
                    // **+|*
                    cur.RightTopPoint_Val.Height =
                        4 * cur.RightEdgeMiddlePt_Val.Height -
                        cur.MiddlePt_Val.Height -
                        cur.RightDownPoint_Val.Height -
                        cur.RightNeighbor.MiddlePt_Val.Height;
                }
                else
                {
                    // #1
                    // **?
                    // **+
                    // **+
                    cur.RightTopPoint_Val.Height =
                        2 * cur.RightEdgeMiddlePt_Val.Height -
                        cur.RightDownPoint_Val.Height;
                }
            }
            else
            {
                if (cur.LeftTopPoint_Val.IsGenerated &&
                    cur.TopEdgeMiddlePt_Val.IsGenerated)
                {
                    if (cur.MiddlePt_Val.IsGenerated &&
                        cur.TopNeighbor != null &&
                        cur.TopNeighbor.MiddlePt_Val != null &&
                        cur.TopNeighbor.MiddlePt_Val.IsGenerated)
                    {
                        // #4
                        // *+*
                        // ---
                        // ++?
                        // *+*
                        // ***
                        cur.RightTopPoint_Val.Height =
                            4 * cur.TopEdgeMiddlePt_Val.Height -
                            cur.TopNeighbor.MiddlePt_Val.Height -
                            cur.LeftTopPoint_Val.Height -
                            cur.MiddlePt_Val.Height;
                    }
                    else
                    {
                        // #3
                        // ++?
                        // ***
                        // ***
                        cur.RightTopPoint_Val.Height =
                            2 * cur.TopEdgeMiddlePt_Val.Height -
                            cur.LeftTopPoint_Val.Height;
                    }
                }
            }
            if (!cur.RightTopPoint_Val.IsGenerated)
                // We can't calculate value
                // Generate random value
                cur.RightTopPoint_Val.Height = (float)rand.NextDouble() * maxHeight;
        }

        /// <summary>
        /// Calculate or get random leftDown point value
        /// </summary>
        void CalculateLeftDownValue(Area cur)
        {
            if (cur.LeftDownPoint_Val.IsGenerated)
                return;
            // Try calculate leftTopPoint val
            // There are 4 cases
            // ?    point which value we want to calculate
            // +    point which value we will use (to calculate ? point)
            // |,-  border of curent area
            if (cur.LeftTopPoint_Val.IsGenerated &&
                cur.LeftEdgeMiddlePt_Val.IsGenerated)
            {
                if (cur.MiddlePt_Val.IsGenerated &&
                    cur.LeftNeighbor != null &&
                    cur.LeftNeighbor.MiddlePt_Val != null &&
                    cur.LeftNeighbor.MiddlePt_Val.IsGenerated)
                {
                    // #2
                    // *|+**
                    // +|++*
                    // *|?**
                    cur.LeftDownPoint_Val.Height =
                        4 * cur.LeftEdgeMiddlePt_Val.Height -
                        cur.MiddlePt_Val.Height -
                        cur.LeftTopPoint_Val.Height -
                        cur.LeftNeighbor.MiddlePt_Val.Height;
                }
                else
                {
                    // #1
                    // +**
                    // +**
                    // ?**
                    cur.LeftDownPoint_Val.Height =
                        2 * cur.LeftEdgeMiddlePt_Val.Height -
                        cur.LeftTopPoint_Val.Height;
                }
            }
            else
            {
                if (cur.RightDownPoint_Val.IsGenerated &&
                    cur.DownEdgeMiddlePt_Val.IsGenerated)
                {
                    if (cur.MiddlePt_Val.IsGenerated &&
                        cur.DownNeighbor != null &&
                        cur.DownNeighbor.MiddlePt_Val != null &&
                        cur.DownNeighbor.MiddlePt_Val.IsGenerated)
                    {
                        // #4
                        // ***
                        // *+*
                        // ?++
                        // ---
                        // *+*
                        cur.LeftDownPoint_Val.Height =
                            4 * cur.DownEdgeMiddlePt_Val.Height -
                            cur.DownNeighbor.MiddlePt_Val.Height -
                            cur.RightDownPoint_Val.Height -
                            cur.MiddlePt_Val.Height;
                    }
                    else
                    {
                        // #3
                        // ***
                        // ***
                        // ?++
                        cur.LeftDownPoint_Val.Height =
                            2 * cur.DownEdgeMiddlePt_Val.Height -
                            cur.RightDownPoint_Val.Height;
                    }
                }
            }
            if (!cur.LeftDownPoint_Val.IsGenerated)
                // We can't calculate value
                // Generate random value
                cur.LeftDownPoint_Val.Height = (float)rand.NextDouble() * maxHeight;
        }

        /// <summary>
        /// Calculate or get random rightDown point value
        /// </summary>
        void CalculateRightDownValue(Area cur)
        {
            if (cur.RightDownPoint_Val.IsGenerated)
                return;
            // Try calculate rightTopPoint val
            // There are 4 cases
            // ?    point which value we want to calculate
            // +    point which value we will use (to calculate ? point)
            // |,-  border of curent area
            if (cur.RightTopPoint_Val.IsGenerated &&
                cur.RightEdgeMiddlePt_Val.IsGenerated)
            {
                if (cur.MiddlePt_Val.IsGenerated &&
                    cur.RightNeighbor != null &&
                    cur.RightNeighbor.MiddlePt_Val != null &&
                    cur.RightNeighbor.MiddlePt_Val.IsGenerated)
                {
                    // #2
                    // **+|*
                    // *++|+
                    // **?|*
                    cur.RightDownPoint_Val.Height =
                        4 * cur.RightEdgeMiddlePt_Val.Height -
                        cur.MiddlePt_Val.Height -
                        cur.RightTopPoint_Val.Height -
                        cur.RightNeighbor.MiddlePt_Val.Height;
                }
                else
                {
                    // #1
                    // **+
                    // **+
                    // **?
                    cur.RightDownPoint_Val.Height =
                        2 * cur.RightEdgeMiddlePt_Val.Height -
                        cur.RightTopPoint_Val.Height;
                }
            }
            else
            {
                if (cur.LeftDownPoint_Val.IsGenerated &&
                    cur.DownEdgeMiddlePt_Val.IsGenerated)
                {
                    if (cur.MiddlePt_Val.IsGenerated &&
                        cur.DownNeighbor != null &&
                        cur.DownNeighbor.MiddlePt_Val != null &&
                        cur.DownNeighbor.MiddlePt_Val.IsGenerated)
                    {
                        // #4
                        // ***
                        // *+*
                        // ++?
                        // ---
                        // *+*
                        cur.RightDownPoint_Val.Height =
                            4 * cur.DownEdgeMiddlePt_Val.Height -
                            cur.DownNeighbor.MiddlePt_Val.Height -
                            cur.LeftDownPoint_Val.Height -
                            cur.MiddlePt_Val.Height;
                    }
                    else
                    {
                        // #3
                        // ***
                        // ***
                        // ++?
                        cur.RightDownPoint_Val.Height =
                            2 * cur.DownEdgeMiddlePt_Val.Height -
                            cur.LeftDownPoint_Val.Height;
                    }
                }
            }
            if (!cur.RightDownPoint_Val.IsGenerated)
                // We can't calculate value
                // Generate random value
                cur.RightDownPoint_Val.Height = (float)rand.NextDouble() * maxHeight;
        }

        void ExtendArea(Area cur)
        {
            // x0  x1  x2  x3
            // x4  x5  x6  x7
            // x8  x9  x10 x11
            // x12 x13 x14 x15
            //
            // Current area (already created) is
            // x5 x6
            // x9 x10
            // 
            // (from square operation)
            // x2 + x0 + x8 = 4*x5 - x10,
            // x3 + x1 + x11 = 4 * x6 - x9,
            // x4 + x12 + x14 = 4 * x9 - x6,
            // x13 + x7 + x15 = 4 * x10 - x5,
            // (from diamond for each cur area corner)
            // x1 + x4 = 4 * x5 - x9 - x6,
            // x2 + x7 = 4 * x6 - x5 - x10,
            // x8 + x13 = 4 * x9 - x5 - x10,
            // x11 + x14 = 4 * x10 - x6 - x9,
            // x0 + x3 + x12 + x15 = x5 + x6 + x9 + x10,
            // (from diamond for borders operation)
            // x0 + x2 = 2 * x1,
            // x1 + x3 = 2 * x2,
            // x3 + x11 = 2 * x7,
            // x7 + x15 = 2 * x11,
            // x15 + x13 = 2 * x14,
            // x14 + x12 = 2 * x13,
            // x4 + x12 = 2 * x8,
            // x0 + x8 = 2 * x4
            //
            // Answer is:
            // x0 -> x10 + 4 x5 - 2 x6 - 2 x9, 
            // x1-> 2 x5 - x9, 
            // x2-> - x10 + 2 x6, 
            // x3-> - 2 x10 - 2 x5 + 4 x6 + x9, 
            // x4-> 2 x5 - x6, 
            // x7-> - x5 + 2 x6, 
            // x8-> - x10 + 2 x9, 
            // x11-> 2 x10 - x9, 
            // x12-> - 2 x10 - 2 x5 + x6 + 4 x9, 
            // x13-> - x5 + 2 x9, 
            // x14-> 2 x10 - x6, 
            // x15-> 4 x10 + x5 - 2 x6 - 2 x9
            if (cur.TopNeighbor == null)
                cur.CreateTopNeighbor();
            if (cur.RightNeighbor == null)
                cur.CreateRightNeighbor();
            if (cur.DownNeighbor == null)
                cur.CreateDownNeighbor();
            if (cur.LeftNeighbor == null)
                cur.CreateLeftNeighbor();

            if (cur.LeftNeighbor.TopNeighbor == null)
                cur.LeftNeighbor.CreateTopNeighbor();
            if (cur.LeftNeighbor.DownNeighbor == null)
                cur.LeftNeighbor.CreateDownNeighbor();
            if (cur.RightNeighbor.TopNeighbor == null)
                cur.RightNeighbor.CreateTopNeighbor();
            if (cur.RightNeighbor.DownNeighbor == null)
                cur.RightNeighbor.CreateDownNeighbor();
        }

        void Square(Area cur, float appl)
        {
            float maxOffset = appl * strength;

            if (!cur.LeftTopPoint_Val.IsGenerated)
                CalculateLeftTopValue(cur);
            if (!cur.RightTopPoint_Val.IsGenerated)
                CalculateRightTopValue(cur);
            if (!cur.LeftDownPoint_Val.IsGenerated)
                CalculateLeftDownValue(cur);
            if (!cur.RightDownPoint_Val.IsGenerated)
                CalculateRightDownValue(cur);

            if (!cur.MiddlePt_Val.IsGenerated)
            {
                // We have 4 corners, generate middle pt
                cur.MiddlePt_Val.Height =
                (cur.LeftTopPoint_Val.Height + cur.RightTopPoint_Val.Height +
                cur.LeftDownPoint_Val.Height + cur.RightDownPoint_Val.Height) / 4.0f;

                float _displacement = ((float)rand.NextDouble() * (2 * maxOffset) - maxOffset);
                if (cur.MiddlePt_Val.Height + _displacement > maxHeight)
                    _displacement *= -0.5f;
                if (cur.MiddlePt_Val.Height + _displacement < minHeight)
                    _displacement *= -0.5f;

                cur.MiddlePt_Val.Height += _displacement;
            }
        }

        void Diamond(Area cur)
        {
            // Try find relative points (points which will be used to avverage middleEdge points)
            // If some points doesn't exists we will use only 2 points (points on the corners of this edge)
            MapVertex _topRelativePtVal = null;
            MapVertex _rightRelativePtVal = null;
            MapVertex _downRelativePtVal = null;
            MapVertex _leftRelativePtVal = null;

            if (!cur.IsSubDivided)
                cur.Subdivide();

            // Try get relative points from neighbors. 
            // If neighbor is not divided MiddlePt_Val will return null
            if (cur.TopNeighbor != null)
                _topRelativePtVal = cur.TopNeighbor.MiddlePt_Val;

            if (cur.RightNeighbor != null)
                _rightRelativePtVal = cur.RightNeighbor.MiddlePt_Val;

            if (cur.DownNeighbor != null)
                _downRelativePtVal = cur.DownNeighbor.MiddlePt_Val;

            if (cur.LeftNeighbor != null)
                _leftRelativePtVal = cur.LeftNeighbor.MiddlePt_Val;

              
            if (!cur.TopEdgeMiddlePt_Val.IsGenerated)
            {
                if (_topRelativePtVal != null)
                    cur.TopEdgeMiddlePt_Val.Height =
                        (cur.LeftTopPoint_Val.Height + _topRelativePtVal.Height + 
                        cur.RightTopPoint_Val.Height + cur.MiddlePt_Val.Height) / 4.0f;
                else
                    cur.TopEdgeMiddlePt_Val.Height =
                        (cur.LeftTopPoint_Val.Height + cur.RightTopPoint_Val.Height) / 2.0f;
            }
                

            if (!cur.RightEdgeMiddlePt_Val.IsGenerated)
            {
                if (_rightRelativePtVal != null)
                    cur.RightEdgeMiddlePt_Val.Height =
                        (cur.RightTopPoint_Val.Height + _rightRelativePtVal.Height +
                        cur.RightDownPoint_Val.Height + cur.MiddlePt_Val.Height) / 4.0f;
                else
                    cur.RightEdgeMiddlePt_Val.Height =
                        (cur.RightTopPoint_Val.Height + cur.RightDownPoint_Val.Height) / 2.0f;
            }

            if (!cur.DownEdgeMiddlePt_Val.IsGenerated)
            {
                if (_downRelativePtVal != null)
                    cur.DownEdgeMiddlePt_Val.Height =
                        (cur.MiddlePt_Val.Height + cur.RightDownPoint_Val.Height +
                        _downRelativePtVal.Height + cur.LeftDownPoint_Val.Height) / 4.0f;
                else
                    cur.DownEdgeMiddlePt_Val.Height =
                        (cur.LeftDownPoint_Val.Height + cur.RightDownPoint_Val.Height) / 2.0f;
            }
            
            if (!cur.LeftEdgeMiddlePt_Val.IsGenerated)
            {
                if (_leftRelativePtVal != null)
                    cur.LeftEdgeMiddlePt_Val.Height =
                        (cur.LeftTopPoint_Val.Height + cur.MiddlePt_Val.Height +
                        cur.LeftDownPoint_Val.Height + _leftRelativePtVal.Height) / 4.0f;
                else
                    cur.LeftEdgeMiddlePt_Val.Height =
                        (cur.LeftTopPoint_Val.Height + cur.LeftDownPoint_Val.Height) / 2.0f;
            }
        }

        void Pogr(Queue<Area> curLayer, Queue<Area> nextLayer, int curDepth, int maxDepth, int generatedDepth)
        {
            if (curLayer.Count == 0)
                return;

            // Square current layer and collect areas to nextLayer
            foreach (Area z in curLayer)
            {
                if (curDepth > generatedDepth)
                {
                    if (!z.IsSubDivided)
                        z.Subdivide();
                    if (!z.MiddlePt_Val.IsGenerated)
                        Square(z, (maxHeight - minHeight) / (float)GetPow2(curDepth + 1));
                }

                if (curDepth == maxDepth)
                    continue;
                nextLayer.Enqueue(z.LeftTopChild);
                nextLayer.Enqueue(z.RightTopChild);
                nextLayer.Enqueue(z.LeftDownChild);
                nextLayer.Enqueue(z.RightDownChild);
            }
            if (curDepth > generatedDepth)
                // Diamond current layer
                foreach (Area z in curLayer)
                {
                    Diamond(z);
                }

            curLayer = nextLayer;
            nextLayer = new Queue<Area>();
            Pogr(curLayer, nextLayer, curDepth + 1, maxDepth, generatedDepth);
        }

        public void ExtendResolution(Area map, byte depth)
        {
            Queue<Area> curLayer = new Queue<Area>();
            Queue<Area> deeperLayer = new Queue<Area>();

            curLayer.Enqueue(map);
            int generatedDepth = -1;
            if (depths.ContainsKey(map))
                generatedDepth = depths[map];
            else
                depths.Add(map, -1);
            Pogr(curLayer, deeperLayer, 1, depth, generatedDepth);
            depths[map] = depth;
        }
    }
}
