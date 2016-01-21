using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World.Model.Chunks
{
    public static class ChunksNavigator
    {
        public static ModelChunk TopNeighbor(ModelChunk chunk)
        {
            ModelCoord topChunkCoord = new ModelCoord(chunk.Coord.x, chunk.Coord.y + 1);
            return chunk.Model.ChunksGrid.GetChunk(topChunkCoord);
        }

        public static ModelChunk RightNeighbor(ModelChunk chunk)
        {
            ModelCoord rightChunkCoord = new ModelCoord(chunk.Coord.x + 1, chunk.Coord.y);
            return chunk.Model.ChunksGrid.GetChunk(rightChunkCoord);
        }

        public static ModelChunk DownNeighbor(ModelChunk chunk)
        {
            ModelCoord downChunkCoord = new ModelCoord(chunk.Coord.x, chunk.Coord.y - 1);
            return chunk.Model.ChunksGrid.GetChunk(downChunkCoord);
        }

        public static ModelChunk LeftNeighbor(ModelChunk chunk)
        {
            ModelCoord leftChunkCoord = new ModelCoord(chunk.Coord.x - 1, chunk.Coord.y);
            return chunk.Model.ChunksGrid.GetChunk(leftChunkCoord);
        }
    }
}
