using System;
using System.Collections.Generic;
using UnityEngine;
using Map.MapModels;
using Map.Generator.Algorithms;
using Map.MapView;
using System.Linq;
using Map.MapModels.Areas;

namespace Map.World
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

        void Awake()
        {
            mapModel = new AreaTree();
            mapViewer = GetComponent<MapViewer>();

            settings = GetComponent<LandscapeSettings>();
            generator = new Generator(settings);
            CurArea = mapModel.Root;
        }

        void Start()
        {
            generator.GenerateAround(CurArea, settings);
            mapViewer.RenderStaticChunk(CurArea,
                new Vector3(player.transform.position.x, 0, player.transform.position.z) +
                new Vector3(-(int)settings.chunkSize / 2.0f, 0, -(int)settings.chunkSize / 2.0f));
            RenderAround();
        }

        void RenderAround()
        {
            int r = settings.depths.Sum((x) => { return x; });
            Area[,] z = CurArea.GetAreasAround(r);
            for (int i = 0; i < z.GetLength(0); i++)
                for (int j = 0; j < z.GetLength(1); j++)
                {
                    if (z[i, j] == null)
                        continue;
                    if (Math.Abs(i - z.GetLength(0) / 2) >= settings.depths[settings.depths.Length - 1] ||
                        Math.Abs(j - z.GetLength(1) / 2) >= settings.depths[settings.depths.Length - 1])
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
                        CurArea = CurArea.GetOrCreateTopNeighbor();
                        break;
                    }
                case 2:
                    {
                        break;
                    }
                case 3:
                    {
                        CurArea = CurArea.GetOrCreateLeftNeighbor();
                        break;
                    }
                case 5:
                    {
                        CurArea = CurArea.GetOrCreateRightNeighbor();
                        break;
                    }
                case 6:
                    {
                        break;
                    }
                case 7:
                    {
                        CurArea = CurArea.GetOrCreateDownNeighbor();
                        break;
                    }
                case 8:
                    {
                        break;
                    }
            }
            generator.GenerateAround(CurArea, settings);
            RenderAround();
        }
    }
}
