using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using World.Model.Frames;

namespace World.Model.Chunks
{
    public class ModelChunk
    {
        /// <summary>
        /// Coordinate of chunk
        /// </summary>
        public ModelCoord Coord { get; private set; }

        /// <summary>
        /// Frame which matches current chunk
        /// </summary>
        public SquareFrame Frame { get; private set; }

        public ModelChunk(ModelCoord coord, SquareFrame frame)
        {
            Coord = coord;
            Frame = frame;
        }

        public override int GetHashCode()
        {
            return Coord.GetHashCode() * Frame.GetHashCode();
        }
    }
}
