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

        int GetPow2(int degree)
        {
            while (pow2.Count <= degree)
                pow2.Add(pow2[pow2.Count - 1] * 2);
            return pow2[degree];
        }

        void Square(Area cur, float appl)
        {
            float maxOffset = appl * strength;

            cur.MiddlePt_Val.height =
                (cur.LeftTopPoint_Val.height + cur.RightTopPoint_Val.height +
                cur.LeftDownPoint_Val.height + cur.RightDownPoint_Val.height) / 4.0f;

            float _displacement = ((float)rand.NextDouble() * (2 * maxOffset) - maxOffset);
            if (cur.MiddlePt_Val.height + _displacement > maxHeight)
                _displacement *= -1;
            if (cur.MiddlePt_Val.height + _displacement < minHeight)
                _displacement *= -1;

            cur.MiddlePt_Val.height += _displacement;
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

            if (cur.Parent != null)
            {
                if (cur.IsLeftTopChild)
                {
                    // There is no left and top relative points
                    if (cur.Parent.RightTopChild.IsSubDivided)
                        _rightRelativePtVal = cur.Parent.RightTopChild.MiddlePt_Val;
                    if (cur.Parent.LeftDownChild.IsSubDivided)
                        _downRelativePtVal = cur.Parent.LeftDownChild.MiddlePt_Val;
                }
                if (cur.IsRightTopChild)
                {
                    // There is no top and right relative points
                    if (cur.Parent.LeftTopChild.IsSubDivided)
                        _leftRelativePtVal = cur.Parent.LeftTopChild.MiddlePt_Val;
                    if (cur.Parent.RightDownChild.IsSubDivided)
                        _downRelativePtVal = cur.Parent.RightDownChild.MiddlePt_Val;
                }
                if (cur.IsLeftDownChild)
                {
                    // There is no left and down relative points
                    if (cur.Parent.LeftTopChild.IsSubDivided)
                        _topRelativePtVal = cur.Parent.LeftTopChild.MiddlePt_Val;
                    if (cur.Parent.RightDownChild.IsSubDivided)
                        _rightRelativePtVal = cur.Parent.RightDownChild.MiddlePt_Val;
                }
                if (cur.IsRightDownChild)
                {
                    // There is no down and right relative points
                    if (cur.Parent.RightTopChild.IsSubDivided)
                        _topRelativePtVal = cur.Parent.RightTopChild.MiddlePt_Val;
                    if (cur.Parent.LeftDownChild.IsSubDivided)
                        _leftRelativePtVal = cur.Parent.LeftDownChild.MiddlePt_Val;
                }
            }
              
            if (_topRelativePtVal != null)
                cur.TopEdgeMiddlePt_Val.height =
                    (cur.LeftTopPoint_Val.height + _topRelativePtVal.height + 
                    cur.RightTopPoint_Val.height + cur.MiddlePt_Val.height) / 4.0f;
            else
                cur.TopEdgeMiddlePt_Val.height =
                    (cur.LeftTopPoint_Val.height + cur.RightTopPoint_Val.height) / 2.0f;

            if (_rightRelativePtVal != null)
                cur.RightEdgeMiddlePt_Val.height =
                    (cur.RightTopPoint_Val.height + _rightRelativePtVal.height +
                    cur.RightDownPoint_Val.height + cur.MiddlePt_Val.height) / 4.0f;
            else
                cur.RightEdgeMiddlePt_Val.height =
                    (cur.RightTopPoint_Val.height + cur.RightDownPoint_Val.height) / 2.0f;

            if (_downRelativePtVal != null)
                cur.DownEdgeMiddlePt_Val.height =
                    (cur.MiddlePt_Val.height + cur.RightDownPoint_Val.height +
                    _downRelativePtVal.height + cur.LeftDownPoint_Val.height) / 4.0f;
            else
                cur.DownEdgeMiddlePt_Val.height =
                    (cur.LeftDownPoint_Val.height + cur.RightDownPoint_Val.height) / 2.0f;

            if (_leftRelativePtVal != null)
                cur.LeftEdgeMiddlePt_Val.height =
                    (cur.LeftTopPoint_Val.height + cur.MiddlePt_Val.height + 
                    cur.LeftDownPoint_Val.height + _leftRelativePtVal.height) / 4.0f;
            else
                cur.LeftEdgeMiddlePt_Val.height =
                    (cur.LeftTopPoint_Val.height + cur.LeftDownPoint_Val.height) / 2.0f;
        }

        void Pogr(Queue<Area> curLayer, Queue<Area> nextLayer, int curDepth, int maxDepth)
        {
            if (curLayer.Count == 0)
                return;

            // Square current layer and collect areas to nextLayer
            foreach (Area z in curLayer)
            {
                if (!z.IsSubDivided)
                {
                    z.Subdivide();
                    Square(z, (maxHeight - minHeight) / (float)GetPow2(curDepth + 1));
                }

                if (curDepth == maxDepth)
                    continue;
                nextLayer.Enqueue(z.LeftTopChild);
                nextLayer.Enqueue(z.RightTopChild);
                nextLayer.Enqueue(z.LeftDownChild);
                nextLayer.Enqueue(z.RightDownChild);
            }
            // Diamond current layer
            foreach (Area z in curLayer)
            {
                Diamond(z);
            }

            curLayer = nextLayer;
            nextLayer = new Queue<Area>();
            Pogr(curLayer, nextLayer, curDepth + 1, maxDepth);
        }

        public void ExtendResolution(Area map, int newResolution)
        {
            Queue<Area> curLayer = new Queue<Area>();
            Queue<Area> deeperLayer = new Queue<Area>();
            // newResolution should be pow of 2
            // -1 because last depth will divide all squares and inc depth
            int maxDepth = (int)(Math.Log(newResolution, 2)) - 1;

            curLayer.Enqueue(map);
            Pogr(curLayer, deeperLayer, 0, maxDepth);
        }
    }
}
