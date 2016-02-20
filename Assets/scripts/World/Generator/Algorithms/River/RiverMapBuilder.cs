using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using World.DataStructures;
using World.DataStructures.ChunksGrid;
using World.Generator.Geometry;

namespace World.Generator.Algorithms.River
{
    public class RiverMapBuilder
    {
        RiverSettings settings;
        WorldGenerator generator;

        public RiverMapBuilder(RiverSettings settings, WorldGenerator generator)
        {
            this.settings = settings;
            this.generator = generator;
        }

        Random rand = new Random();

        /// <summary>
        /// Build river source and recursively build river way
        /// </summary>
        private void BuildSource(IntCoord coord, PointsStorage<float> heighmap, PointsStorage<RiverData> riverMap, Dictionary<Chunk, int> riverDensity, ChunksNavigator navigator, float sourceEnergy)
        {
            if (riverMap.Contains(coord))
                return; // We joined some river
            riverMap.Initialize(coord);
            riverMap[coord].energy = sourceEnergy;
            // Increase density for chunk(s) which contain this point
            foreach (Chunk chunk in navigator.GetChunksByInnerCoord(coord))
            {
                if (!riverDensity.ContainsKey(chunk))
                    riverDensity.Add(chunk, 1);
                else
                    riverDensity[chunk]++;
            }
            // Find neighbor the water will flow to
            IntCoord minHeightNeighbor = coord;
            for (int x = coord.x - 1; x <= coord.x + 1; x++)
                for (int y = coord.y - 1; y <= coord.y + 1; y++)
                {
                    IntCoord cur = new IntCoord(x, y);
                    if (!heighmap.Contains(cur))
                        generator.GeneratePoint(cur);
                    if (heighmap[cur] < heighmap[minHeightNeighbor])
                        minHeightNeighbor = cur;
                }
            if (minHeightNeighbor.Equals(coord))
                return; // No way for river
            // Make min height neighbor as new source
            BuildSource(minHeightNeighbor, heighmap, riverMap, riverDensity, navigator, sourceEnergy + heighmap[coord] - heighmap[minHeightNeighbor]);
        }

        public void BuildRiverMap(Chunk chunk, PointsStorage<float> heighmap, PointsStorage<RiverData> riverMap, Dictionary<Chunk, int> riverDensity)
        {
            if (!riverDensity.ContainsKey(chunk))
                riverDensity.Add(chunk, 0);
            while (riverDensity[chunk] / (float)(chunk.Size * chunk.Size) < settings.riverChunkDensity)
            {
                IntCoord coord = new IntCoord(rand.Next(chunk.LeftBorder, chunk.RightBorder), rand.Next(chunk.DownBorder, chunk.TopBorder));
                BuildSource(coord, heighmap, riverMap, riverDensity, chunk.chunksNavigator, settings.sourceEnergy);
            }
        }
    }
}
