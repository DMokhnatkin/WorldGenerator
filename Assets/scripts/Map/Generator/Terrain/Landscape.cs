using System;
using System.Collections.Generic;
using UnityEngine;
using Map.Generator.MapModels;
using Map.Generator.Algorithms;

namespace Map.Generator.Landscape
{
    public class Landscape : MonoBehaviour
    {
        // Represents a model of full map
        private AreaTree model;

        private Area curArea;

        public Texture2D baseTexture;

        public List<GameObject> chunks = new List<GameObject>();

        System.Random rand = new System.Random();

        void Start()
        {
            model = new AreaTree();
            curArea = model.Root;
            curArea.LeftTopPoint_Val.height = (float)rand.NextDouble() * 0.2f;
            curArea.RightTopPoint_Val.height = (float)rand.NextDouble() * 0.2f;
            curArea.LeftDownPoint_Val.height = (float)rand.NextDouble() * 0.2f;
            curArea.RightDownPoint_Val.height = (float)rand.NextDouble() * 0.2f;

            chunks.Add(GenerateChunk(curArea));
        }

        GameObject GenerateChunk(Area area)
        {
            DiamondSquare sq = new DiamondSquare() { strength = 0.1f, minHeight = 0, maxHeight = 1 };
            sq.ExtendResolution(area, 512);
            MapVertex[,] map = HeightMap.AreaToArray(area);
            float[,] heights = new float[513, 513];
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    heights[i, j] = map[i, j].height;

            TerrainData tData = new TerrainData();
            tData.heightmapResolution = 513;
            tData.size = new Vector3(500, 500, 500);
            tData.SetHeights(0, 0, heights);

            SplatPrototype newSplat = new SplatPrototype();
            newSplat.texture = baseTexture;

            tData.splatPrototypes = new SplatPrototype[] { newSplat };

            GameObject terr = Terrain.CreateTerrainGameObject(tData);
            terr.transform.position = new Vector3(Camera.main.transform.position.x - tData.size.x / 2.0f, 0, Camera.main.transform.position.z - tData.size.z / 2.0f);
            return terr;
        }
    }
}
