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
        private void BuildRiverSkeleton(IntCoord coord, PointsStorage<float> heighmap, RiverMap riverMap, ChunksNavigator navigator, float energy, float waterAmount)
        {
            if (riverMap.riverSkeletons.Contains(coord))
                return; // We joined some river
            riverMap.riverSkeletons.Initialize(coord);
            riverMap.riverSkeletons[coord].energy = energy;
            riverMap.riverSkeletons[coord].waterAmount = waterAmount;
            // Find neighbor the water will flow to
            IntCoord minHeightNeighbor = coord;
            for (int x = coord.x - 1; x <= coord.x + 1; x++)
                for (int y = coord.y - 1; y <= coord.y + 1; y++)
                {
                    IntCoord cur = new IntCoord(x, y);
                    if (!cur.Equals(coord.Top) && !cur.Equals(coord.Right) &&
                        !cur.Equals(coord.Down) && !cur.Equals(coord.Left))
                        continue;
                    if (!heighmap.Contains(cur))
                        generator.GeneratePoint(cur);
                    if (heighmap[cur] < heighmap[minHeightNeighbor])
                        minHeightNeighbor = cur;
                }
            riverMap.riverSkeletons[coord].direction = minHeightNeighbor;
            if (minHeightNeighbor.Equals(coord))
                return; // No way for river
            // Make min height neighbor as new source
            BuildRiverSkeleton(minHeightNeighbor, 
                heighmap, 
                riverMap, 
                navigator, 
                energy + heighmap[coord] - heighmap[minHeightNeighbor], 
                waterAmount + settings.rainy);
        }

        public void BuildRiverMap(Chunk chunk, PointsStorage<float> heighmap, RiverMap riverMap)
        {
            // TODO: create more then 1 source per chunk
            for (int i = 0; i < settings.countToTry; i++)
            {
                IntCoord coord = new IntCoord(rand.Next(chunk.LeftBorder, chunk.RightBorder), rand.Next(chunk.DownBorder, chunk.TopBorder));
                if (riverMap.DistToNearestSource(coord) > settings.minSourceDistance)
                {
                    BuildRiverSkeleton(coord, heighmap, riverMap, chunk.chunksNavigator, settings.sourceEnergy, (float)rand.NextDouble() * settings.maxSourceWaterAmount);
                    riverMap.CreateSource(coord);
                }
            }
        }

        /// <summary>
        /// Affect chunk by skeleton point
        /// </summary>
        private void AffectChunkBySkeletonPoint(IntCoord coord, PointsStorage<float> heightmap, RiverMap riverMap)
        {
            int radius = (int)Math.Ceiling(Math.Log(riverMap.riverSkeletons[coord].waterAmount / settings.waterAmountEps, 2));
            for (int y = coord.y - radius; y <= coord.y + radius; y++)
                for (int x = coord.x - radius; x <= coord.x + radius; x++)
                {
                    IntCoord cur = new IntCoord(x, y);
                    if (!heightmap.Contains(cur))
                        generator.GeneratePoint(cur);
                    riverMap.riverData.TryInitialize(cur);
                    float waterAm = riverMap.riverSkeletons[coord].waterAmount /
                        (float)Math.Pow(2, Math.Sqrt(Math.Pow(cur.x - coord.x, 2) + Math.Pow(cur.y - coord.y, 2)));
                    if (waterAm > riverMap.riverData[cur].waterAmount)
                    {
                        heightmap[cur] -= waterAm - riverMap.riverData[cur].waterAmount;
                        riverMap.riverData[cur].waterAmount = waterAm;
                    }
                }
        }

        public void AffectChunk(Chunk chunk, PointsStorage<float> heighmap, RiverMap riverMap)
        {
            for (int y = chunk.DownBorder; y <= chunk.TopBorder; y++)
                for (int x = chunk.LeftBorder; x <= chunk.RightBorder; x++)
                {
                    IntCoord coord = new IntCoord(x, y);
                    if (!riverMap.riverSkeletons.Contains(coord))
                        continue;
                    //heighmap[coord] -= riverMap.riverData[coord].energy * settings.disolve;
                    //AffectChunkByEdge(coord, riverMap.riverSkeletons[coord].direction, heighmap, riverMap);
                    AffectChunkBySkeletonPoint(coord, heighmap, riverMap);
                }
        }
    }
}
