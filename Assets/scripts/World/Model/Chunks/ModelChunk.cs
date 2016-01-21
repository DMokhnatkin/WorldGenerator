﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using World.Model.Frames;
using World.Common;
using World.Model;

namespace World.Model.Chunks
{
    public class ModelChunk : IEqualityComparer<ModelChunk>
    {
        public WorldModel Model { get; private set; }

        /// <summary>
        /// Coordinate of chunk
        /// </summary>
        public ModelCoord Coord { get; private set; }

        /// <summary>
        /// Frame which matches current chunk
        /// </summary>
        public SquareFrame Frame { get; private set; }

        /// <summary>
        /// Size in normal model coords of one chunk
        /// </summary>
        public int Size { get { return Model.ChunksGrid.ChunkSize; } }

        public ModelChunk(ModelCoord coord, SquareFrame frame, WorldModel model)
        {
            Coord = coord;
            Frame = frame;
            Model = model;
        }

        public override bool Equals(object obj)
        {
            return ((ModelChunk)obj).Coord.Equals(this.Coord);
        }

        public bool Equals(ModelChunk x, ModelChunk y)
        {
            return x.Equals(y);
        }

        public override int GetHashCode()
        {
            return Coord.GetHashCode() * Frame.GetHashCode();
        }

        public int GetHashCode(ModelChunk obj)
        {
            return obj.GetHashCode();
        }

        /// <summary>
        /// Get size in specifed detalization layer
        /// </summary>
        public int GetSizeInLayer(int layerId)
        {
            return Size / (Pow2.GetPow2((Model.MaxDetalizationLayerId - layerId)));
        }

        /// <summary>
        /// Get point in specifed detalization layer
        /// </summary>
        public ModelPoint GetPointInLayer(ModelCoord coord, int layerId)
        {
            ModelCoord normalCoord = new ModelCoord(
                Frame.LeftBorder + coord.x * Model.GetLayer(layerId).CoordOffset,
                Frame.DownBorder + coord.y * Model.GetLayer(layerId).CoordOffset);
            return Model[normalCoord];
        }
    }
}
