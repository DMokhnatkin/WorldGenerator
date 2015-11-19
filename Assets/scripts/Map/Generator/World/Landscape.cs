using System;
using System.Collections.Generic;
using UnityEngine;
using Map.Generator.MapModels;
using Map.Generator.Algorithms;

namespace Map.Generator.World
{

    [RequireComponent(typeof(LandscapeSettings))]
    public class Landscape : MonoBehaviour
    {
        public Generator generator = new Generator();

        /// <summary>
        /// Player. World will be generated for this player
        /// </summary>
        public GameObject player;

        // Represents a model of full map
        private AreaTree model;

        private Chunk curChunk;

        private LandscapeSettings settings;

        System.Random rand = new System.Random();

        public Area CurArea
        {
            get
            {
                if (curChunk == null)
                    return null;
                else
                    return curChunk.Area;
            }
        }

        public GameObject CurChunkModel { get { return curChunk.GeneratedObject; } }

        public AreaTree MapModel { get { return model; } }

        void Start()
        {
            settings = GetComponent<LandscapeSettings>();

            model = new AreaTree();

            curChunk = generator.TryGenerateSingleChunk(model.Root,
                                new Vector2(player.transform.position.x - (int)settings.chunkSize / 2.0f,
                                player.transform.position.z - (int)settings.chunkSize / 2.0f),
                                settings.HightQualityDepth,
                                settings);

            CurArea.CreateTopNeighbor();
            generator.TryGenerateSingleChunk(CurArea.TopNeighbor,
                new Vector2(player.transform.position.x - (int)settings.chunkSize / 2.0f,
                                player.transform.position.z - (int)settings.chunkSize / 2.0f + (int)settings.chunkSize),
                                settings.HightQualityDepth,
                                settings);

            StartCoroutine(generator.GenerateAround(curChunk.Area, settings));
        }

        void FixedUpdate()
        {
            if (curChunk == null)
                return;
            // y coord - z coord in unity units
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);

            Terrain curTerrain = CurChunkModel.GetComponent<Terrain>();
            Vector2 leftDownPt = new Vector2(CurChunkModel.transform.position.x, CurChunkModel.transform.position.z);
            Vector2 rigthTopPt = leftDownPt +
                new Vector2(curTerrain.terrainData.size.x, curTerrain.terrainData.size.z);

            // 4 - curRight chunk, try find new curRight chunk
            // 0 1 2
            // 3 4 5
            // 6 7 8
            int xRelChunkCoord = 1;
            int yRelChunkCoord = 1;
            if (playerPos.x < leftDownPt.x)
                xRelChunkCoord = 0;
            else
            {
                if (playerPos.x > rigthTopPt.x)
                    xRelChunkCoord = 2;
            }
            if (playerPos.y > rigthTopPt.y)
                yRelChunkCoord = 0;
            else
            {
                if (playerPos.y < leftDownPt.y)
                    yRelChunkCoord = 2;
            }

            if (xRelChunkCoord == 1 && yRelChunkCoord == 1)
                return; // Cur chunk wasn't changed
            // Cur chunk was changed
            int newChunkId = yRelChunkCoord * 3 + xRelChunkCoord;
            switch (newChunkId)
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        StartCoroutine(generator.GenerateAround(CurArea.TopNeighbor, settings));
                        StartCoroutine(generator.WaitForChunkGenerated(CurArea.TopNeighbor, (x) => { curChunk = x; }));
                        break;
                    }
                case 2:
                    {
                        break;
                    }
                case 3:
                    {
                        StartCoroutine(generator.GenerateAround(CurArea.LeftNeighbor, settings));
                        StartCoroutine(generator.WaitForChunkGenerated(CurArea.LeftNeighbor, (x) => { curChunk = x; }));
                        break;
                    }
                case 5:
                    {
                        StartCoroutine(generator.GenerateAround(CurArea.RightNeighbor, settings));
                        StartCoroutine(generator.WaitForChunkGenerated(CurArea.RightNeighbor, (x) => { curChunk = x; }));
                        break;
                    }
                case 6:
                    {
                        break;
                    }
                case 7:
                    {
                        StartCoroutine(generator.GenerateAround(CurArea.DownNeighbor, settings));
                        StartCoroutine(generator.WaitForChunkGenerated(CurArea.DownNeighbor, (x) => { curChunk = x; }));
                        break;
                    }
                case 8:
                    {
                        break;
                    }
            }
        }
    }
}
