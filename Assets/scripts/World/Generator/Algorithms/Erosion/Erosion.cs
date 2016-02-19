using System;
using System.Collections.Generic;
using World.DataStructures.ChunksGrid;
using World.Model;
using World.DataStructures;

namespace World.Generator.Algorithms.Erosion
{
    /// <summary>
    /// http://www-evasion.imag.fr/Publications/2007/MDH07/FastErosion_PG07.pdf
    /// </summary>
    public class Erosion
    {
        public readonly ErosionSettings settings;

        private Random rand = new Random();


        public Erosion(ErosionSettings settings)
        {
            this.settings = settings;
        }

        /// <summary>
        /// Calculate height difference (including water height) 
        /// </summary>
        private float CalcHeightDifference(WorldModel model, IntCoord a, IntCoord b)
        {
            return model.heighmap[a] + model.erosionMap[a].waterHeight1 -
                model.heighmap[b] - model.erosionMap[b].waterHeight1;
        }

        /// <summary>
        /// Calc flows for points
        /// </summary>
        private void CalcFlows(WorldModel model, IntCoord cur, float dt, float cellSize)
        {
            if (model.erosionMap.Contains(cur.Left))
            {
                model.erosionMap[cur].flowLeft = Math.Max(
                    0, model.erosionMap[cur].flowLeft +
                       dt * cellSize *
                       settings.gravity *
                       CalcHeightDifference(model, cur, cur.Left));
            }
            if (model.erosionMap.Contains(cur.Top))
            {
                model.erosionMap[cur].flowTop = Math.Max(
                    0, model.erosionMap[cur].flowTop +
                       dt * cellSize *
                       settings.gravity *
                       CalcHeightDifference(model, cur, cur.Top));
            }
            if (model.erosionMap.Contains(cur.Right))
            {
                model.erosionMap[cur].flowRight = Math.Max(
                    0, model.erosionMap[cur].flowRight +
                       dt * cellSize *
                       settings.gravity *
                       CalcHeightDifference(model, cur, cur.Right));
            }
            if (model.erosionMap.Contains(cur.Down))
            {
                model.erosionMap[cur].flowDown = Math.Max(
                    0, model.erosionMap[cur].flowDown +
                       dt * cellSize *
                       settings.gravity *
                       CalcHeightDifference(model, cur, cur.Down));
            }
            // Scale flows to make sum of flows less then water height
            float k = Math.Min(1,
                model.erosionMap[cur].waterHeight1 * cellSize * cellSize /
                ((model.erosionMap[cur].flowTop +
                  model.erosionMap[cur].flowRight +
                  model.erosionMap[cur].flowLeft +
                  model.erosionMap[cur].flowDown) * dt));
            model.erosionMap[cur].flowTop *= k;
            model.erosionMap[cur].flowRight *= k;
            model.erosionMap[cur].flowDown *= k;
            model.erosionMap[cur].flowLeft *= k;
        }

        /// <summary>
        /// Recalculate water height (add correction by flows)
        /// </summary>
        private void CalcWaterHeight2(WorldModel model, IntCoord cur, float dt, float cellSize)
        {
            // Let's find flows from neighbors
            float fromTop = model.erosionMap.Contains(cur.Top) ?
                model.erosionMap[cur.Top].flowDown :
                0;
            float fromRight = model.erosionMap.Contains(cur.Right) ?
                model.erosionMap[cur.Right].flowLeft :
                0;
            float fromDown = model.erosionMap.Contains(cur.Down) ?
                model.erosionMap[cur.Down].flowTop :
                0;
            float fromLeft = model.erosionMap.Contains(cur.Left) ?
                model.erosionMap[cur.Left].flowRight :
                0;

            // Delta v (after calculate of outflow and inflow)
            float dv = dt * (fromTop + fromRight + fromDown + fromLeft -
                model.erosionMap[cur].flowTop -
                model.erosionMap[cur].flowRight -
                model.erosionMap[cur].flowDown -
                model.erosionMap[cur].flowLeft);

            // Change water height
            model.erosionMap[cur].waterHeight2 = model.erosionMap[cur].waterHeight1 +
                dv / (cellSize * cellSize);
        }

        /// <summary>
        /// Calculate velocity
        /// </summary>
        private void CalcVelocity(WorldModel model, IntCoord cur, float cellSize)
        {
            // X velocity
            float fromLeft = model.erosionMap.Contains(cur.Left) ?
                model.erosionMap[cur.Left].flowRight :
                0;
            float fromRight = model.erosionMap.Contains(cur.Right) ?
                model.erosionMap[cur.Right].flowLeft :
                0;
            model.erosionMap[cur].xVelocity =
                (fromLeft - model.erosionMap[cur].flowLeft +
                 model.erosionMap[cur].flowRight - fromRight) / 
                 (2.0f * cellSize * (0.5f * (model.erosionMap[cur].waterHeight1 + model.erosionMap[cur].waterHeight2)));
            // Y velocity
            float fromTop = model.erosionMap.Contains(cur.Top) ?
                model.erosionMap[cur.Top].flowDown :
                0;
            float fromDown = model.erosionMap.Contains(cur.Down) ?
                model.erosionMap[cur.Down].flowTop :
                0;
            model.erosionMap[cur].yVelocity =
                (fromTop - model.erosionMap[cur].flowTop +
                 model.erosionMap[cur].flowDown - fromDown) /
                 (2.0f * cellSize * (0.5f * (model.erosionMap[cur].waterHeight1 + model.erosionMap[cur].waterHeight2)));
        }

        /// <summary>
        /// Calculate cos of slope
        /// http://math.stackexchange.com/questions/1044044/local-tilt-angle-based-on-height-field
        /// </summary>
        private float CalcSlopeCos(WorldModel model, IntCoord cur, float cellSize)
        {
            // dh/dx
            float dh_x;
            if (model.heighmap.Contains(cur.Left) &&
                model.heighmap.Contains(cur.Right))
            {
                dh_x = (model.heighmap[cur.Left] - model.heighmap[cur.Right]) / cellSize;
            }
            else
            {
                if (model.heighmap.Contains(cur.Left))
                {
                    dh_x = (model.heighmap[cur.Left] - model.heighmap[cur]) / cellSize;
                }
                else
                {
                    dh_x = (model.heighmap[cur] - model.heighmap[cur.Right]) / cellSize;
                }
            }

            // dh/dy
            float dh_y;
            if (model.heighmap.Contains(cur.Top) &&
                model.heighmap.Contains(cur.Down))
            {
                dh_y = (model.heighmap[cur.Top] - model.heighmap[cur.Down]) / cellSize;
            }
            else
            {
                if (model.heighmap.Contains(cur.Top))
                {
                    dh_y = (model.heighmap[cur.Top] - model.heighmap[cur]) / cellSize;
                }
                else
                {
                    dh_y = (model.heighmap[cur] - model.heighmap[cur.Down]) / cellSize;
                }
            }
            return (float)(1 / (Math.Sqrt(1 + dh_x * dh_x + dh_y * dh_y)));
        }

        /// <summary>
        /// Calc sediment transport capacity
        /// </summary>
        private float CalcSedTranspCap(WorldModel model, IntCoord cur, float cellSize)
        {
            float cos = CalcSlopeCos(model, cur, cellSize);
            float velMod = (float)Math.Sqrt(
                Math.Pow(model.erosionMap[cur].xVelocity, 2) +
                Math.Pow(model.erosionMap[cur].yVelocity, 2));
            return (float)(settings.k * Math.Sqrt(1 - Math.Pow(cos, 2)) * velMod);
        }

        /// <summary>
        /// Calculate new terrain height
        /// </summary>
        private void CalcErosionAndDeposition(WorldModel model, IntCoord cur, float cellSize)
        {
            float sedCap = CalcSedTranspCap(model, cur, cellSize);
            if (sedCap > model.erosionMap[cur].suspendedSediment)
            {
                model.heighmap[cur] -= settings.ks * (sedCap - model.erosionMap[cur].suspendedSediment);
                model.erosionMap[cur].suspendedSediment += settings.ks * (sedCap - model.erosionMap[cur].suspendedSediment);
            }
            else
            {
                model.heighmap[cur] += settings.kd * (model.erosionMap[cur].suspendedSediment - sedCap);
                model.erosionMap[cur].suspendedSediment -= settings.kd * (model.erosionMap[cur].suspendedSediment - sedCap);
            }
        }

        /// <summary>
        /// Calculate erosion for chunk
        /// </summary>
        public void CalcChunkErosion(Chunk chunk,
            WorldModel model,
            int iterations)
        {
            float cellSize = model.CoordTransformer.ModelDistToGlobal(1);
            model.erosionMap.Initialize(chunk);
            // Delta time
            float dt = 50f;
            for (int i = 0; i < iterations; i++)
            {
                for (int y = chunk.DownBorder; y <= chunk.TopBorder; y++)
                    for (int x = chunk.LeftBorder; x <= chunk.RightBorder; x++)
                    {
                        IntCoord cur = new IntCoord(x, y);

                        // Increase water level (from rain)
                        model.erosionMap[cur].waterHeight1 += dt * settings.rainy * (float)rand.NextDouble();

                        // Recalculate flows
                        CalcFlows(model, cur, dt, cellSize);

                        // Recalculate water height (with flow correction)
                        CalcWaterHeight2(model, cur, dt, cellSize);

                        // Recalculate water velocity
                        CalcVelocity(model, cur, cellSize);

                        // Add erosion and deposition
                        CalcErosionAndDeposition(model, cur, cellSize);
                    }
            }
        }
    }
}
