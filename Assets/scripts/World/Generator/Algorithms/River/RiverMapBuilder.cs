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

        public RiverMapBuilder(RiverSettings settings)
        {
            this.settings = settings;
        }

        Random rand = new Random();
        const int randConst = 50;
        const float eps = 0.0001f;

        public void BuildRiverMap(Chunk chunk, PointsStorage<float> heighmap, PointsStorage<RiverData> riverMap)
        {
            riverMap.Initialize(chunk, 1);

            List<IntCoord> queue = new List<IntCoord>();
            for (int y = chunk.DownBorder; y <= chunk.TopBorder; y++)
                for (int x = chunk.LeftBorder; x <= chunk.RightBorder; x++)
                {
                    queue.Add(new IntCoord(x, y));
                }
            // Descending sort
            queue.Sort((x, y) => { return heighmap[x] > heighmap[y] ? -1 : 1; });

            for (int i = queue.Count - 1; i >= 0; i--)
            {
                if (Comparer.Compare(riverMap[queue[i]].waterLevel, 0, eps) != 0)
                    continue;
                if (rand.Next(i, queue.Count + randConst) == i)
                    riverMap[queue[i]].waterLevel = (float)(settings.maxSourceDeep * rand.NextDouble());
            }

            for (int i = 0; i < queue.Count; i++)
            {
                IntCoord cur = queue[i];

                float maxWaterLevel =
                    Math.Max(Math.Max(heighmap[cur.Top] + riverMap[cur.Top].waterLevel, heighmap[cur.Right] + riverMap[cur.Right].waterLevel), 
                    Math.Max(heighmap[cur.Down] + riverMap[cur.Down].waterLevel, heighmap[cur.Left] + riverMap[cur.Left].waterLevel));
                if (maxWaterLevel > heighmap[cur])
                    riverMap[cur].waterLevel = maxWaterLevel - heighmap[cur];
            }
        }
    }
}
