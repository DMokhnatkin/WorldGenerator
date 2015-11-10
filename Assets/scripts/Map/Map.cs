using UnityEngine;
using System.Collections;
using Map.Generator;
using System.Collections.Generic;
using Map.Generator.CommonMap;

namespace Map
{
    public class Map : MonoBehaviour
    {
        GameObject terr1;
        GameObject terr2;
        public Texture2D baseTexture;

        MapModel mapModel = new MapModel(512);

        void Start()
        {
            DiamondSquare sq = new DiamondSquare();
            HeightMap _map = new HeightMap();
            sq.ExtendResolution(_map, 512);
            MapVertex[,] map = _map.ToArray();
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

            terr1 = Terrain.CreateTerrainGameObject(tData);
            terr1.transform.position = new Vector3(Camera.main.transform.position.x - tData.size.x / 2.0f, 0, Camera.main.transform.position.z - tData.size.z / 2.0f);

            mapModel.AddTopNeighbor(0, 0);

            sq = new DiamondSquare();
            _map = new HeightMap();
            sq.ExtendResolution(_map, 512);
            map = _map.ToArray();
            heights = new float[513, 513];
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    heights[i, j] = map[i, j].height;

            tData = new TerrainData();
            tData.heightmapResolution = 513;
            tData.size = new Vector3(500, 500, 500);
            tData.SetHeights(0, 0, heights);

            newSplat = new SplatPrototype();
            newSplat.texture = baseTexture;

            tData.splatPrototypes = new SplatPrototype[] { newSplat };

            terr2 = Terrain.CreateTerrainGameObject(tData);
            terr2.transform.position = new Vector3(terr1.transform.position.x + terr1.GetComponent<Terrain>().terrainData.size.x, 0, terr2.transform.position.z - terr1.GetComponent<Terrain>().terrainData.size.z);
        }
    }
}
