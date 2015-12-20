using System;
using System.Collections.Generic;
using Map.MapModels.Common;

namespace Map.MapModels.WorldModel
{
    public class WorldModel
    {
        /// <summary>
        /// How much times chunk can be divided 
        /// (each divide increase point count in chunk)
        /// </summary>
        public int MaxChunkDetalization { get; private set; }

        /// <summary>
        /// All points and their coordinates
        /// </summary>
        Dictionary<Coord, WorldPoint> Points { get; set; }

        /// <summary>
        /// Get point by normal coord. If point isn't exists return null
        /// </summary>
        public WorldPoint GetPoint(Coord normalCoord)
        {
            if (Points.ContainsKey(normalCoord))
                return Points[normalCoord];
            return null;
        }

        /// <summary>
        /// Get point by layer and coordinates in it. If point isn't exists return null.
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public WorldPoint GetPoint(Coord coord, WorldModelLayer layer)
        {
            Coord normalCoord = layer.LayerToNormal(coord);
            if (Points.ContainsKey(normalCoord))
                return Points[normalCoord];
            return null;
        }

        /// <summary>
        /// Create point by normal coord
        /// </summary>
        /// <exception cref="ArgumentException">Point is already created</exception>
        public WorldPoint CreatePoint(Coord normalCoord)
        {
            if (Points.ContainsKey(normalCoord))
                throw new ArgumentException(String.Format("Point with coord {0} already created", normalCoord.ToString()));
            Points.Add(normalCoord, new WorldPoint(normalCoord));
            return Points[normalCoord];
        }

        public WorldPoint CreatePoint(Coord coord, WorldModelLayer layer)
        {
            Coord normalCoord = layer.LayerToNormal(coord);
            if (Points.ContainsKey(normalCoord))
                throw new ArgumentException(String.Format("Point with coord(normal) {0} already created", normalCoord.ToString()));
            Points.Add(normalCoord, new WorldPoint(normalCoord));
            return Points[normalCoord];
        }

        /// <summary>
        /// If point exists return it.
        /// If point isn't exists create it and then return 
        /// </summary>
        public WorldPoint GetOrCreatePoint(Coord normalCoord)
        {
            if (!Points.ContainsKey(normalCoord))
                return CreatePoint(normalCoord);
            else
                return GetPoint(normalCoord);
        }

        public WorldModel(int maxChunkDetalization)
        {
            MaxChunkDetalization = maxChunkDetalization;
            Points = new Dictionary<Coord, WorldPoint>();
        }

        public WorldModelLayer GetLayer(int detalization)
        {
            return new WorldModelLayer(this, detalization);
        }

        public WorldModelLayer GetMaxDetalizationLayer()
        {
            return new WorldModelLayer(this, MaxChunkDetalization);
        }
    }
}
