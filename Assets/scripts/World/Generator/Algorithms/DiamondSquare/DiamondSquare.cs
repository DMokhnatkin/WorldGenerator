﻿using System;
using World.Model;
using World.Model.Frames;
using System.Collections.Generic;
using World.Common;

namespace World.Generator.Algorithms.DiamondSquare
{
    public class DiamondSquare
    {
        DiamondSquareSettings settings = new DiamondSquareSettings();

        private Random rand = new Random();
        // Max difference between heighs of 2 neighbor points in max detalization layer of model.
        private float _maxOffsetForModelUnit;
        private WorldModel model;

        public DiamondSquare(WorldModel model)
        {
            this.model = model; 
            _maxOffsetForModelUnit = model.CoordTransformer.ModelDistToGlobal(1, model.MaxDetalizationLayer) * settings.harshness;
        }

        void Square(BinPlus1SquareFrame frame)
        {
            float maxOffset = _maxOffsetForModelUnit * Pow2.SQRT_2 * frame.Size;
            model[frame.Center].Data.height = 
                (model[frame.LeftTopCorner].Data.height +
                 model[frame.RightTopCorner].Data.height +
                 model[frame.LeftDownCorner].Data.height +
                 model[frame.RightDownCorner].Data.height) / 4.0f +
                 2*((float)rand.NextDouble() - 0.5f) * maxOffset;
        }

        void Diamond(BinPlus1SquareFrame frame)
        {
            // Diamond top edge center point
            if (model.Contains(new ModelCoord(frame.TopEdgeCenter.x, frame.TopEdgeCenter.y + frame.Size / 2)) &&
                model.Contains(frame.Center))
            {
                model[frame.TopEdgeCenter].Data.height =
                    (model[new ModelCoord(frame.TopEdgeCenter.x, frame.TopEdgeCenter.y + frame.Size / 2)].Data.height +
                     model[frame.Center].Data.height +
                     model[frame.LeftTopCorner].Data.height +
                     model[frame.RightTopCorner].Data.height) / 4.0f;
            }
            else
            {
                model[frame.TopEdgeCenter].Data.height =
                    (model[frame.LeftTopCorner].Data.height +
                     model[frame.RightTopCorner].Data.height) / 2.0f;
            }

            // Diamond right edge center point
            if (model.Contains(new ModelCoord(frame.RightEdgeCenter.x + frame.Size / 2, frame.RightEdgeCenter.y)) &&
                model.Contains(frame.Center))
            {
                model[frame.RightEdgeCenter].Data.height =
                    (model[new ModelCoord(frame.RightEdgeCenter.x + frame.Size / 2, frame.RightEdgeCenter.y)].Data.height +
                     model[frame.Center].Data.height +
                     model[frame.RightTopCorner].Data.height +
                     model[frame.RightDownCorner].Data.height) / 4.0f;
            }
            else
            {
                model[frame.RightEdgeCenter].Data.height =
                    (model[frame.RightTopCorner].Data.height +
                     model[frame.RightDownCorner].Data.height) / 2.0f;
            }

            // Diamond down edge center point
            if (model.Contains(new ModelCoord(frame.DownEdgeCenter.x, frame.DownEdgeCenter.y - frame.Size / 2)) &&
                model.Contains(frame.Center))
            {
                model[frame.DownEdgeCenter].Data.height =
                    (model[new ModelCoord(frame.DownEdgeCenter.x, frame.DownEdgeCenter.y - frame.Size / 2)].Data.height +
                     model[frame.Center].Data.height +
                     model[frame.LeftDownCorner].Data.height +
                     model[frame.RightDownCorner].Data.height) / 4.0f;
            }
            else
            {
                model[frame.DownEdgeCenter].Data.height =
                    (model[frame.LeftDownCorner].Data.height +
                     model[frame.RightDownCorner].Data.height) / 2.0f;
            }

            // Diamond left edge center point
            if (model.Contains(new ModelCoord(frame.LeftEdgeCenter.x - frame.Size / 2, frame.LeftEdgeCenter.y)) &&
                model.Contains(frame.Center))
            {
                model[frame.LeftEdgeCenter].Data.height =
                    (model[new ModelCoord(frame.LeftEdgeCenter.x - frame.Size / 2, frame.LeftEdgeCenter.y)].Data.height +
                     model[frame.Center].Data.height +
                     model[frame.LeftTopCorner].Data.height +
                     model[frame.LeftDownCorner].Data.height) / 4.0f;
            }
            else
            {
                model[frame.LeftEdgeCenter].Data.height =
                    (model[frame.LeftTopCorner].Data.height +
                     model[frame.LeftDownCorner].Data.height) / 2.0f;
            }
        }

        /// <summary>
        /// Generate in BinPlus1SquareFrame
        /// </summary>
        public void GenerateSingleFrame(BinPlus1SquareFrame frame)
        {
            // Randomize corners
            float maxOffset = _maxOffsetForModelUnit * frame.Size;
            model[frame.LeftTopCorner].Data.height = 2 * ((float)rand.NextDouble() - 0.5f) * maxOffset;
            model[frame.RightDownCorner].Data.height = model[frame.LeftTopCorner].Data.height + 2 * ((float)rand.NextDouble() - 0.5f) * maxOffset * Pow2.SQRT_2;
            model[frame.LeftDownCorner].Data.height = 
                (model[frame.LeftTopCorner].Data.height + 
                 model[frame.RightDownCorner].Data.height) / 2.0f + 
                 ((float)rand.NextDouble() - 0.5f) * maxOffset * Pow2.SQRT_2;
            model[frame.RightTopCorner].Data.height =
                (model[frame.LeftTopCorner].Data.height +
                 model[frame.RightDownCorner].Data.height) / 2.0f +
                 ((float)rand.NextDouble() - 0.5f) * maxOffset * Pow2.SQRT_2;

            Queue<BinPlus1SquareFrame> curLayerQueue = new Queue<BinPlus1SquareFrame>();
            Queue<BinPlus1SquareFrame> nextLayerQueue = new Queue<BinPlus1SquareFrame>();
            curLayerQueue.Enqueue(frame);
            while (curLayerQueue.Count != 0)
            {
                foreach (BinPlus1SquareFrame t in curLayerQueue)
                {
                    Square(t);
                }
                while (curLayerQueue.Count != 0)
                {
                    BinPlus1SquareFrame t = curLayerQueue.Dequeue();
                    Diamond(t);

                    if (t.Size == 2)
                        continue; // We can't divide deeper
                    nextLayerQueue.Enqueue(t.LeftTopQuarter);
                    nextLayerQueue.Enqueue(t.RightTopQuarter);
                    nextLayerQueue.Enqueue(t.LeftDownQuarter);
                    nextLayerQueue.Enqueue(t.RightDownQuarter);
                }
                curLayerQueue = nextLayerQueue;
                nextLayerQueue = new Queue<BinPlus1SquareFrame>();
            }
        }
    }
}
