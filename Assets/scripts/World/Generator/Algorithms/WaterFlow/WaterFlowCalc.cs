using System;
using System.Collections.Generic;
using UnityEngine;
using World.DataStructures;
using World.DataStructures.ChunksGrid;
using World.Generator.Geometry;

namespace World.Generator.Algorithms.WaterFlow
{
    public class WaterFlowCalc
    {
        private WaterFlowSettings settings;
        System.Random rand = new System.Random();

        public WaterFlowCalc(WaterFlowSettings settings)
        {
            this.settings = settings;
        }

        void ApplyToHeighMap(Chunk chunk, PointsStorage<float> heighmap, PointsStorage<WaterFlowData> waterFlowMap, float cellSize)
        {
            for (int y = chunk.DownBorder; y <= chunk.TopBorder; y++)
                for (int x = chunk.LeftBorder; x <= chunk.RightBorder; x++)
                {
                    IntCoord cur = new IntCoord(x, y);
                    heighmap[cur] -=
                        (waterFlowMap[cur].topVelocity +
                            waterFlowMap[cur].rightVelocity +
                            waterFlowMap[cur].downVelocity +
                            waterFlowMap[cur].leftVelocity).y * settings.kSlip;
                }
        }

        float CalcNewWaterSpeed(PointsStorage<float> heighmap, PointsStorage<WaterFlowData> waterFlowMap, IntCoord source, IntCoord target, float waterCt)
        {
            return
                (float)(waterCt *
                        Math.Sqrt(2 * settings.g * (heighmap[source] - heighmap[target])) +
                        waterFlowMap[target].accumulatedWater *
                        waterFlowMap[target].waterSpeed) /
                        (waterCt + waterFlowMap[target].accumulatedWater);
        }

        /// <summary>
        /// Calculate waterFlowMap for chunk
        /// <a href="http://www-evasion.imag.fr/Publications/2007/MDH07/FastErosion_PG07.pdf"/>
        /// </summary>
        public void CalcWaterFlow(Chunk chunk, PointsStorage<float> heighmap, PointsStorage<WaterFlowData> waterFlowMap, float cellSize, int iterations)
        {
            waterFlowMap.Initialize(chunk, 1);
            /*
            for (int i = 0; i < iterations; i++)
            {
                for (int y = chunk.DownBorder; y <= chunk.TopBorder; y++)
                    for (int x = chunk.LeftBorder; x <= chunk.RightBorder; x++)
                    {
                        IntCoord cur = new IntCoord(x, y);
                        if (x == chunk.LeftBorder || x == chunk.RightBorder ||
                            y == chunk.DownBorder || y == chunk.TopBorder)
                        {
                            if (waterFlowMap[cur].generated)
                                continue;
                        }

                        // Increase water level (from rain)
                        float d1 = waterFlowMap[cur].accumulatedWater + settings.rainy;

                        // Recalculate flows
                        waterFlowMap[cur].flowTop = Math.Max(
                                0, waterFlowMap[cur].flowTop + cellSize * settings.g * (heighmap[cur] + d1 - heighmap[cur.Top] - waterFlowMap[cur.Top].accumulatedWater));
                        waterFlowMap[cur].flowRight = Math.Max(
                                0, waterFlowMap[cur].flowRight + cellSize * settings.g * (heighmap[cur] + d1 - heighmap[cur.Right] - waterFlowMap[cur.Right].accumulatedWater));
                        waterFlowMap[cur].flowDown = Math.Max(
                                0, waterFlowMap[cur].flowLeft + cellSize * settings.g * (heighmap[cur] + d1 - heighmap[cur.Down] - waterFlowMap[cur.Down].accumulatedWater));
                        waterFlowMap[cur].flowLeft = Math.Max(
                                0, waterFlowMap[cur].flowDown + cellSize * settings.g * (heighmap[cur] + d1 - heighmap[cur.Left] - waterFlowMap[cur.Left].accumulatedWater));

                        // Scale flows to make sum of flows less then water height
                        float k = Math.Min(1,
                            waterFlowMap[cur].accumulatedWater * cellSize * cellSize /
                            ((waterFlowMap[cur].flowTop +
                              waterFlowMap[cur].flowRight +
                              waterFlowMap[cur].flowLeft +
                              waterFlowMap[cur].flowDown)));
                        waterFlowMap[cur].flowTop *= k;
                        waterFlowMap[cur].flowRight *= k;
                        waterFlowMap[cur].flowDown *= k;
                        waterFlowMap[cur].flowLeft *= k;

                        // How much water volume in cell was changed due to flows
                        float dv = (waterFlowMap[cur.Top].flowDown + waterFlowMap[cur.Right].flowLeft + 
                                    waterFlowMap[cur.Down].flowTop + waterFlowMap[cur.Left].flowRight -
                                    waterFlowMap[cur].flowTop - waterFlowMap[cur].flowRight -
                                    waterFlowMap[cur].flowDown - waterFlowMap[cur].flowLeft);

                        // New water level after flows
                        float d2 = d1 + dv / (cellSize * cellSize);

                        // Calculate velocity
                        waterFlowMap[cur].velocity.x = (waterFlowMap[cur.Left].flowRight - 
                            waterFlowMap[cur].flowLeft + 
                            waterFlowMap[cur].flowRight - 
                            waterFlowMap[cur.Right].flowLeft) / (cellSize * (d1 + d2));
                        waterFlowMap[cur].velocity.y = (waterFlowMap[cur.Down].flowTop -
                            waterFlowMap[cur].flowDown +
                            waterFlowMap[cur].flowTop -
                            waterFlowMap[cur.Top].flowDown) / (cellSize * (d1 + d2));

                        ///<summary>See 3.2 in <a href="http://www.cs.rpi.edu/~cutler/classes/advancedgraphics/S10/final_projects/lau.pdf"></a></summary>
                        float s = settings.k_c * waterFlowMap[cur].accumulatedWater * waterFlowMap[cur].velocity.magnitude;
                        if (s > waterFlowMap[cur].sedimentCapacity)
                        {
                            heighmap[cur] += settings.k_d * (s - waterFlowMap[cur].sedimentCapacity);
                            waterFlowMap[cur].sedimentCapacity = s - settings.k_d * (s - waterFlowMap[cur].sedimentCapacity);
                        }
                        else
                        {
                            heighmap[cur] -= settings.k_s * (waterFlowMap[cur].sedimentCapacity - s);
                            waterFlowMap[cur].sedimentCapacity = settings.k_s * (waterFlowMap[cur].sedimentCapacity - s);
                        }

                        waterFlowMap[cur].accumulatedWater = d2 * settings.k_e;

                        if (i == iterations - 1)
                        {
                            waterFlowMap[cur].generated = true;
                        }
                    }
            }*/

            
            List<IntCoord> queue = new List<IntCoord>();
            for (int y = chunk.DownBorder; y <= chunk.TopBorder; y++)
                for (int x = chunk.LeftBorder; x <= chunk.RightBorder; x++)
                {
                    queue.Add(new IntCoord(x, y));
                }
            // Descending sort
            queue.Sort((x, y) => { return heighmap[x] > heighmap[y] ? -1 : 1; });

            for (int i = 0; i < queue.Count; i++)
            {
                IntCoord cur = queue[i];

                if (waterFlowMap[cur].generated)
                    continue;

                // Increase water level (from rain)
                waterFlowMap[cur].accumulatedWater += settings.rainy;

                // Calc how much water will flow to neighbors
                waterFlowMap[cur].flowTop = Math.Max(0, heighmap[cur] - heighmap[cur.Top]);
                waterFlowMap[cur].flowRight = Math.Max(0, heighmap[cur] - heighmap[cur.Right]);
                waterFlowMap[cur].flowDown = Math.Max(0, heighmap[cur] - heighmap[cur.Down]);
                waterFlowMap[cur].flowLeft = Math.Max(0, heighmap[cur] - heighmap[cur.Left]);

                // Scale flows to make sum of flows less then accumulated water
                float k =
                    waterFlowMap[cur].accumulatedWater /
                    ((waterFlowMap[cur].flowTop +
                      waterFlowMap[cur].flowRight +
                      waterFlowMap[cur].flowLeft +
                      waterFlowMap[cur].flowDown));
                waterFlowMap[cur].flowTop *= k;
                waterFlowMap[cur].flowRight *= k;
                waterFlowMap[cur].flowDown *= k;
                waterFlowMap[cur].flowLeft *= k;

                // Recalculate flows
                if (heighmap[cur] > heighmap[cur.Top])
                    waterFlowMap[cur.Top].waterSpeed = CalcNewWaterSpeed(heighmap, waterFlowMap, cur, cur.Top, waterFlowMap[cur].flowTop);
                if (heighmap[cur] > heighmap[cur.Right])
                    waterFlowMap[cur.Right].waterSpeed = CalcNewWaterSpeed(heighmap, waterFlowMap, cur, cur.Right, waterFlowMap[cur].flowRight);
                if (heighmap[cur] > heighmap[cur.Down])
                    waterFlowMap[cur.Down].waterSpeed = CalcNewWaterSpeed(heighmap, waterFlowMap, cur, cur.Down, waterFlowMap[cur].flowDown);
                if (heighmap[cur] > heighmap[cur.Left])
                    waterFlowMap[cur.Left].waterSpeed = CalcNewWaterSpeed(heighmap, waterFlowMap, cur, cur.Left, waterFlowMap[cur].flowLeft);

                waterFlowMap[cur.Top].accumulatedWater += waterFlowMap[cur].flowTop;
                waterFlowMap[cur.Right].accumulatedWater += waterFlowMap[cur].flowRight;
                waterFlowMap[cur.Down].accumulatedWater += waterFlowMap[cur].flowDown;
                waterFlowMap[cur.Left].accumulatedWater += waterFlowMap[cur].flowLeft;

                waterFlowMap[cur].generated = true;

                /*
                // Recalculate velocities
                if (heighmap[cur.Down] > heighmap[cur])
                {
                    Vector2 tilt = new Vector2(cellSize, heighmap[cur.Down] - heighmap[cur]);
                    waterFlowMap[cur].topVelocity += new Vector2(0, settings.rainy);
                    waterFlowMap[cur].topVelocity += (Vector2.Dot(waterFlowMap[cur.Down].topVelocity, tilt) / tilt.magnitude) * settings.inertness * tilt.normalized;
                }
                if (heighmap[cur.Left] > heighmap[cur])
                {
                    Vector2 tilt = new Vector2(cellSize, heighmap[cur.Left] - heighmap[cur]);
                    waterFlowMap[cur].rightVelocity += new Vector2(0, settings.rainy);
                    waterFlowMap[cur].rightVelocity += (Vector2.Dot(waterFlowMap[cur.Left].rightVelocity, tilt) / tilt.magnitude) * settings.inertness * tilt.normalized;
                }
                if (heighmap[cur.Top] > heighmap[cur])
                {
                    Vector2 tilt = new Vector2(cellSize, heighmap[cur.Top] - heighmap[cur]);
                    waterFlowMap[cur].downVelocity += new Vector2(0, settings.rainy);
                    waterFlowMap[cur].downVelocity += (Vector2.Dot(waterFlowMap[cur.Top].downVelocity, tilt) / tilt.magnitude) * settings.inertness * tilt.normalized;
                }
                if (heighmap[cur.Right] > heighmap[cur])
                {
                    Vector2 tilt = new Vector2(cellSize, heighmap[cur.Right] - heighmap[cur]);
                    waterFlowMap[cur].leftVelocity += new Vector2(0, settings.rainy);
                    waterFlowMap[cur].leftVelocity += (Vector2.Dot(waterFlowMap[cur.Right].leftVelocity, tilt) / tilt.magnitude) * settings.inertness * tilt.normalized;
                }*/
            }

            ApplyToHeighMap(chunk, heighmap, waterFlowMap, cellSize);
        }
    }
}
