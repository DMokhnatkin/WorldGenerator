using UnityEngine;
using System.Collections;

namespace Map.Generator
{
    public class DiamondSquare
    {
        public float strength = 1;

        /// <summary>
        /// i0, j0, i1, j1 - current square in map
        /// Must be pow of 2
        /// </summary>
        void Square(float[,] map, int i0, int j0, int i1, int j1)
        {
            int i_middle = (i1 + i0) / 2;
            int j_middle = (j1 + j0) / 2;
            if (i_middle == i0 || i_middle == i1)
                return;
            float maxOffset = (i1 - i0) * strength;

            map[i0, j_middle] = (map[i0, j0] + map[i0, j1]) / 2.0f;
            map[i1, j_middle] = (map[i1, j0] + map[i1, j1]) / 2.0f;
            map[i_middle, j0] = (map[i0, j0] + map[i1, j0]) / 2.0f;
            map[i_middle, j1] = (map[i0, j1] + map[i1, j1]) / 2.0f;

            map[i_middle, j_middle] = (map[i0, j0] + map[i0, j1] + map[i1, j0] + map[i1, j1]) / 4.0f + Random.Range(-maxOffset, maxOffset);
            Square(map, i0, j0, i_middle, j_middle);
            Square(map, i_middle, j_middle, i1, j1);
            Square(map, i_middle, j0, i1, j_middle);
            Square(map, i0, j_middle, i_middle, j1);
        }

        void Diamond(float[,] map, int i0, int j0, int i1, int j1)
        {
            int i_middle = (i1 + i0) / 2;
            int j_middle = (j1 + j0) / 2;
            if (i_middle == i0 || i_middle == i1)
                return;
            float maxOffset = (i1 - i0) * strength;

            int j_left = 2 * j0 - j_middle;
            int j_right = 2 * j1 - j_middle;
            int i_up = 2 * i0 - i_middle;
            int i_down = 2 * i1 - i_middle;

            if (i_up >= 0)
                map[i0, j_middle] = (map[i_up, j_middle] + map[i_middle, j_middle] + map[i0, j0] + map[i0, j1]) / 4.0f;
            else
                map[i0, j_middle] = (map[i0, j0] + map[i0, j1]) / 2.0f;
            if (j_right < map.GetLength(1))
                map[i_middle, j1] = (map[i_middle, j_right] + map[i_middle, j_middle] + map[i0, j1] + map[i1, j1]) / 4.0f;
            else
                map[i_middle, j1] = (map[i0, j1] + map[i1, j1]) / 2.0f;
            if (i_down < map.GetLength(0))
                map[i1, j_middle] = (map[i_down, j_middle] + map[i_middle, j_middle] + map[i1, j0] + map[i1, j1]) / 4.0f;
            else
                map[i1, j_middle] = (map[i1, j0] + map[i1, j1]) / 2.0f;
            if (j_left >= 0)
                map[i_middle, j0] = (map[i_middle, j_left] + map[i_middle, j_middle] + map[i0, j0] + map[i1, j0]) / 4.0f;
            else
                map[i_middle, j0] = (map[i0, j0] + map[i1, j0]) / 2.0f;

            Diamond(map, i0, j0, i_middle, j_middle);
            Diamond(map, i_middle, j_middle, i1, j1);
            Diamond(map, i_middle, j0, i1, j_middle);
            Diamond(map, i0, j_middle, i_middle, j1);
        }

        public HeightMap Generate(int size)
        {
            // size = Pow(?,2) + 1
            float[,] res = new float[size, size];
            res[0, 0] = Random.Range(-100, 100);
            res[size - 1, size - 1] = Random.Range(-100, 100);
            res[0, size - 1] = Random.Range(-100, 100);
            res[size - 1, 0] = Random.Range(-100, 100);
            Square(res, 0, 0, size - 1, size - 1);
            //Diamond(res, 0, 0, size - 1, size - 1);
            return new HeightMap(res);
        }
    }

    public class HeightMap
    {
        public float[,] val;

        public HeightMap(float[,] val)
        {
            this.val = val;
        }

        /// <summary>
        /// Move up or down and scale map to make all heights to 0..maxHeight
        /// </summary>
        /// <param name="maxHeight"></param>
        public void Normilize(float maxHeight)
        {
            float curMin = float.MaxValue;
            float curMax = float.MinValue;
            for (int i = 0; i < val.GetLength(0); i++)
                for (int j = 0; j < val.GetLength(1); j++)
                {
                if (val[i, j] > curMax)
                    curMax = val[i, j];
                if (val[i, j] < curMin)
                    curMin = val[i, j];
            }
            float scale = maxHeight / (curMax - curMin);
            for (int i = 0; i < val.GetLength(0); i++)
                for (int j = 0; j < val.GetLength(1); j++)
                {
                    // Make min height in map = 0
                    val[i, j] -= curMin;
                    // Make max height in map = maxHeight
                    val[i, j] *= scale;
                }
        }
    }
}
