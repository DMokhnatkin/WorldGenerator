using UnityEngine;
using System;
using System.Collections.Generic;
using Map.Generator.SubdividedPlane;
using System.IO;

namespace Map.Generator
{
    public class DiamondSquare
    {
        /// <summary>
        /// 0..1
        /// </summary>
        public float strength = 0.1f;
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

        void Square(Node<MapVertex> cur, float appl)
        {
            float maxOffset = appl * strength;

            cur.MiddlePt_Val.height =
                (cur.LeftTopPoint_Val.height + cur.RightTopPoint_Val.height +
                cur.LeftDownPoint_Val.height + cur.RightDownPoint_Val.height) / 4.0f;

            float _displacement = ((float)rand.NextDouble() * (2 * maxOffset) - maxOffset);
            if (cur.MiddlePt_Val.height + _displacement > maxHeight)
                _displacement *= -1;
            if (cur.MiddlePt_Val.height + _displacement < 0)
                _displacement *= -1;

            cur.MiddlePt_Val.height += _displacement;
        }

        void Diamond(Node<MapVertex> cur)
        {
            // Try find relative points (points which will be used to avverage middleEdge points)
            // If some points doesn't exists we will use only 2 points (points on the corners of this edge)
            MapVertex _topRelativePtVal = null;
            MapVertex _rightRelativePtVal = null;
            MapVertex _downRelativePtVal = null;
            MapVertex _leftRelativePtVal = null;
            
            if (cur.Parent != null)
            {
                if (cur.Parent.LeftTopChild == cur)
                {
                    // There is no left and top relative points
                    if (cur.Parent.RightTopChild.IsDivided)
                        _rightRelativePtVal = cur.Parent.RightTopChild.MiddlePt_Val;
                    if (cur.Parent.LeftDownChild.IsDivided)
                        _downRelativePtVal = cur.Parent.LeftDownChild.MiddlePt_Val;
                }
                if (cur.Parent.RightTopChild == cur)
                {
                    // There is no top and right relative points
                    if (cur.Parent.LeftTopChild.IsDivided)
                        _leftRelativePtVal = cur.Parent.LeftTopChild.MiddlePt_Val;
                    if (cur.Parent.RightDownChild.IsDivided)
                        _downRelativePtVal = cur.Parent.RightDownChild.MiddlePt_Val;
                }
                if (cur.Parent.LeftDownChild == cur)
                {
                    // There is no left and down relative points
                    if (cur.Parent.LeftTopChild.IsDivided)
                        _topRelativePtVal = cur.Parent.LeftTopChild.MiddlePt_Val;
                    if (cur.Parent.RightDownChild.IsDivided)
                        _rightRelativePtVal = cur.Parent.RightDownChild.MiddlePt_Val;
                }
                if (cur.Parent.RightDownChild == cur)
                {
                    // There is no down and right relative points
                    if (cur.Parent.RightTopChild.IsDivided)
                        _topRelativePtVal = cur.Parent.RightTopChild.MiddlePt_Val;
                    if (cur.Parent.LeftDownChild.IsDivided)
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

        void Pogr(Queue<Node<MapVertex>> curLayer, Queue<Node<MapVertex>> deeperLayer, int curDepth, int maxDepth)
        {
            if (curLayer.Count == 0)
                return;
            // After we square all points we should Diamond them
            Queue<Node<MapVertex>> toDiamond = new Queue<Node<MapVertex>>();
            while (curLayer.Count != 0)
            {
                Node<MapVertex> z = curLayer.Dequeue();
                toDiamond.Enqueue(z);
                Square(z, maxHeight / (float)GetPow2(curDepth + 1));

                if (curDepth == maxDepth)
                    continue;
                deeperLayer.Enqueue(z.LeftTopChild);
                deeperLayer.Enqueue(z.RightTopChild);
                deeperLayer.Enqueue(z.LeftDownChild);
                deeperLayer.Enqueue(z.RightDownChild);
            }
            while (toDiamond.Count != 0)
            {
                Node<MapVertex> z = toDiamond.Dequeue();
                Diamond(z);
            }
            curLayer = deeperLayer;
            deeperLayer = new Queue<Node<MapVertex>>();

            Pogr(curLayer, deeperLayer, curDepth + 1, maxDepth);
        }

        public void ExtendResolution(HeightMap map, int newResolution)
        {
            if (map.resolution == 1)
            {
                map.val.Root.LeftTopPoint_Val = new MapVertex()
                    { height = (float)rand.NextDouble() * maxHeight };
                map.val.Root.RightTopPoint_Val = new MapVertex()
                    { height = (float)rand.NextDouble() * maxHeight };
                map.val.Root.LeftDownPoint_Val = new MapVertex()
                    { height = (float)rand.NextDouble() * maxHeight };
                map.val.Root.RightDownPoint_Val = new MapVertex()
                    { height = (float)rand.NextDouble() * maxHeight };
            }
            Queue<Node<MapVertex>> curLayer = new Queue<Node<MapVertex>>();
            Queue<Node<MapVertex>> deeperLayer = new Queue<Node<MapVertex>>();
            // newResolution should be pow of 2
            // -1 because last depth will divide all squares and inc depth
            int maxDepth = (int)(Math.Log(newResolution, 2)) - 1;

            curLayer.Enqueue(map.val.Root);
            Pogr(curLayer, deeperLayer, 0, maxDepth);
            map.resolution = newResolution;
        }
    }
}
