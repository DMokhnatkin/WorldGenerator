using System;
using System.Collections.Generic;
using UnityEngine;
using Map.Generator.MapModels;
using Map.Generator.Algorithms;
using Map.Generator.MapView;

namespace Map.Generator.World
{

    [RequireComponent(typeof(LandscapeSettings))]
    public class Landscape : MonoBehaviour
    {
        // Model part of map
        private AreaTree mapModel;

        // Visual part of map
        public MapViewer mapViewer;

        public Generator generator;

        /// <summary>
        /// Player. World will be generated for this player
        /// </summary>
        public GameObject player;

        private LandscapeSettings settings;

        System.Random rand = new System.Random();

        public Area CurArea { get; private set; }

        public AreaTree MapModel { get { return mapModel; } }

        public Vector3 CurAreaLeftDownPos
        {
            get { return mapViewer.GetViewInfo(CurArea).LeftDownPos; }
        }

        void Start()
        {
            mapModel = new AreaTree();
            mapViewer = new MapViewer(GetComponent<LandscapeSettings>());

            settings = GetComponent<LandscapeSettings>();
            generator = new Generator(settings);
            CurArea = mapModel.Root;

            generator.Generate(CurArea, settings.HightQualityDepth, settings.HightQualityRadius);
            mapViewer.RenderStaticChunk(CurArea,
                new Vector3(player.transform.position.x, 0, player.transform.position.z) +
                new Vector3(-(int)settings.chunkSize / 2.0f, 0, -(int)settings.chunkSize / 2.0f));

            //mapViewer.RenderDynamicChunk(CurArea.LeftNeighbor, GetPosFromNeighbors(CurArea.LeftNeighbor).Value);
            RenderAround();
        }

        void RenderAround()
        {
            Area[,] z = CurArea.GetAreasAround(settings.HightQualityDepth + settings.HightQualityRadius);
            for (int i = 0; i < z.GetLength(0); i++)
                for (int j = 0; j < z.GetLength(1); j++)
                {
                    if (Math.Abs(i - z.GetLength(0) / 2) > settings.HightQualityRadius ||
                        Math.Abs(j - z.GetLength(1) / 2) > settings.HightQualityRadius)
                    {
                        mapViewer.RenderDynamicChunk(z[i, j],
                            mapViewer.GetViewInfo(CurArea).LeftDownPos +
                            new Vector3((j - z.GetLength(1) / 2) * (int)settings.chunkSize, 0, (z.GetLength(0) / 2 - i) * (int)settings.chunkSize));
                    }
                    else
                    {
                        mapViewer.RenderStaticChunk(z[i, j],
                            mapViewer.GetViewInfo(CurArea).LeftDownPos +
                            new Vector3((j - z.GetLength(1) / 2) * (int)settings.chunkSize, 0, (z.GetLength(0) / 2 - i) * (int)settings.chunkSize));
                    }
                }
        }

        // Try get position of chunk from neighbors
        Vector3? GetPosFromNeighbors(Area area)
        {
            if (area.TopNeighbor != null)
            {
                ChunkViewInfo z = mapViewer.GetViewInfo(area.TopNeighbor);
                if (z != null && z.LeftDownPos != null)
                    return z.LeftDownPos + new Vector3(0, 0, -(int)settings.chunkSize);
            }
            if (area.RightNeighbor != null)
            {
                ChunkViewInfo z = mapViewer.GetViewInfo(area.RightNeighbor);
                if (z != null && z.LeftDownPos != null)
                    return z.LeftDownPos + new Vector3(-(int)settings.chunkSize, 0, 0);
            }
            if (area.DownNeighbor != null)
            {
                ChunkViewInfo z = mapViewer.GetViewInfo(area.DownNeighbor);
                if (z != null && z.LeftDownPos != null)
                    return z.LeftDownPos + new Vector3(0, 0, (int)settings.chunkSize);
            }
            if (area.LeftNeighbor != null)
            {
                ChunkViewInfo z = mapViewer.GetViewInfo(area.LeftNeighbor);
                if (z != null && z.LeftDownPos != null)
                    return z.LeftDownPos + new Vector3((int)settings.chunkSize, 0, 0);
            }
            return null;
        }

        void FixedUpdate()
        {
            if (mapViewer.GetViewInfo(CurArea) == null)
                return;
            Vector3 playerPos = player.transform.position;

            Vector3 leftDownCorner = mapViewer.GetViewInfo(CurArea).LeftDownPos;
            Vector3 rigthTopCorner = leftDownCorner +
                new Vector3((int)settings.chunkSize, 0, (int)settings.chunkSize);

            // 4 - cur chunk, try find new cur chunk
            // 0 1 2
            // 3 4 5
            // 6 7 8
            int xRelChunkCoord = 1;
            int zRelChunkCoord = 1;
            if (playerPos.x < leftDownCorner.x)
                xRelChunkCoord = 0;
            else
            {
                if (playerPos.x > rigthTopCorner.x)
                    xRelChunkCoord = 2;
            }
            if (playerPos.z > rigthTopCorner.z)
                zRelChunkCoord = 0;
            else
            {
                if (playerPos.z < leftDownCorner.z)
                    zRelChunkCoord = 2;
            }

            if (xRelChunkCoord == 1 && zRelChunkCoord == 1)
                return; // Cur chunk wasn't changed
            // Cur chunk was changed
            int newChunkId = zRelChunkCoord * 3 + xRelChunkCoord;
            switch (newChunkId)
            {
                case 0:
                    {
                        break;
                    }
                case 1:
                    {
                        generator.Generate(CurArea.GetOrCreateTopNeighbor(), settings.HightQualityDepth, settings.HightQualityRadius + 1);
                        CurArea = CurArea.TopNeighbor;
                        mapViewer.RenderStaticChunk(CurArea, GetPosFromNeighbors(CurArea).Value);
                        break;
                    }
                case 2:
                    {
                        break;
                    }
                case 3:
                    {
                        generator.Generate(CurArea.GetOrCreateLeftNeighbor(), settings.HightQualityDepth, settings.HightQualityRadius + 1);
                        CurArea = CurArea.LeftNeighbor;
                        mapViewer.RenderStaticChunk(CurArea, GetPosFromNeighbors(CurArea).Value);
                        break;
                    }
                case 5:
                    {
                        generator.Generate(CurArea.GetOrCreateRightNeighbor(), settings.HightQualityDepth, settings.HightQualityRadius + 1);
                        CurArea = CurArea.RightNeighbor;
                        mapViewer.RenderStaticChunk(CurArea, GetPosFromNeighbors(CurArea).Value);
                        break;
                    }
                case 6:
                    {
                        break;
                    }
                case 7:
                    {
                        generator.Generate(CurArea.GetOrCreateDownNeighbor(), settings.HightQualityDepth, settings.HightQualityRadius);
                        CurArea = CurArea.DownNeighbor;
                        mapViewer.RenderStaticChunk(CurArea, GetPosFromNeighbors(CurArea).Value);
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
