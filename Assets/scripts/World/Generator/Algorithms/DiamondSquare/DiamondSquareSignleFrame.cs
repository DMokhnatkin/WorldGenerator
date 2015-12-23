using System;
using World.Model;
using World.Model.Frames;
using System.Collections.Generic;

namespace World.Generator.Algorithms.DiamondSquare
{
    public class DiamondSquareSignleFrame
    {
        static void Square(BinSquareFrame frame, WorldModel model)
        {
            model[Center(frame)].Data.height = 1;
        }

        /// <summary>
        /// Get center coord of frame
        /// </summary>
        static ModelCoord Center(BinSquareFrame frame)
        {
            return (
                new ModelCoord(
                    frame.LeftDown.x + frame.Size / 2, 
                    frame.LeftDown.y + frame.Size / 2));
        }

        /// <summary>
        /// Get left top quarter of frame
        /// </summary>
        static BinSquareFrame LeftTopQuarter(BinSquareFrame frame)
        {
            return new BinSquareFrame(
                new ModelCoord(frame.LeftDown.x, frame.LeftDown.y + frame.Size / 2),
                frame.Size / 2);
        }

        /// <summary>
        /// Get right top quarter of frame
        /// </summary>
        static BinSquareFrame RightTopQuarter(BinSquareFrame frame)
        {
            return new BinSquareFrame(
                new ModelCoord(frame.LeftDown.x + frame.Size / 2, frame.LeftDown.y + frame.Size / 2),
                frame.Size / 2);
        }

        /// <summary>
        /// Get left down quarter of frame
        /// </summary>
        static BinSquareFrame LeftDownQuarter(BinSquareFrame frame)
        {
            return new BinSquareFrame(frame.LeftDown, frame.Size / 2);
        }

        /// <summary>
        /// Get right down quarter of frame
        /// </summary>
        static BinSquareFrame RightDownQuarter(BinSquareFrame frame)
        {
            return new BinSquareFrame(
                new ModelCoord(frame.LeftDown.x + frame.Size / 2, frame.LeftDown.y + frame.Size / 2),
                frame.Size / 2);
        }

        /// <summary>
        /// Generate in BinSquareFrame
        /// </summary>
        public static void Generate(BinSquareFrame frame, WorldModel model)
        {
            int curLayer = 0;
            Queue<BinSquareFrame> curLayerQueue = new Queue<BinSquareFrame>();
            Queue<BinSquareFrame> nextLayerQueue = new Queue<BinSquareFrame>();
            curLayerQueue.Enqueue(frame);
            while (curLayer <= model.MaxDetalizationLayerId)
            {
                while (curLayerQueue.Count != 0)
                {
                    BinSquareFrame t = curLayerQueue.Dequeue();
                    Square(t, model);
                    if (curLayer == model.MaxDetalizationLayerId)
                        continue;
                    nextLayerQueue.Enqueue(LeftTopQuarter(t));
                    nextLayerQueue.Enqueue(RightTopQuarter(t));
                    nextLayerQueue.Enqueue(LeftDownQuarter(t));
                    nextLayerQueue.Enqueue(RightDownQuarter(t));
                }
                curLayer++;
            }
        }
    }
}
