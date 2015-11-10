using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map.Generator.CommonMap
{
    public class MapChunk
    {
        public HeightMap heightMap;

        public MapChunk(HeightMap map)
        {
            heightMap = map;
        }
    }
}
