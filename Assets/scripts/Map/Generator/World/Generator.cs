﻿using System;
using System.Collections.Generic;
using Map.Generator.MapModels;
using Map.Generator.Algorithms;
using UnityEngine;
using System.Collections;

namespace Map.Generator.World
{
    public class Generator
    {
        /// <summary>
        /// Is we generate smth now
        /// (needs for coroutines)
        /// </summary>
        bool _generating = false;

        /// <summary>
        /// Stores info about current generated depth, linked gameobject, area for each area
        /// </summary>
        Dictionary<Area, Chunk> chunksInfo = new Dictionary<Area, Chunk>();

        /// <summary>
        /// Is area for area was generated before
        /// </summary>
        public bool IsChunkGenerated(Area area)
        {
            return (chunksInfo.ContainsKey(area) && chunksInfo[area].GeneratedObject != null);
        }

        /// <summary>
        /// Get chunk for area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public Chunk GetChunk(Area area)
        {
            if (!chunksInfo.ContainsKey(area))
                return null;
            return chunksInfo[area];
        }

        /// <summary>
        /// Wait while chunk generated and call callback
        /// </summary>
        public IEnumerator WaitForChunkGenerated(Area area, Action<Chunk> callback)
        {
            while (_generating && !chunksInfo.ContainsKey(area))
                yield return null;
            callback(GetChunk(area));
        }

        /// <summary>
        /// Try generate single area. If exists return it
        /// </summary>
        public Chunk TryGenerateSingleChunk(Area areaChunk, Vector2 pos, byte depth, LandscapeSettings sett)
        {
            if (IsChunkGenerated(areaChunk))
            {
                return chunksInfo[areaChunk];
            }

            if (!chunksInfo.ContainsKey(areaChunk))
            {
                chunksInfo.Add(areaChunk, new Chunk(areaChunk, null));
            }

            DiamondSquare sq = new DiamondSquare(sett.HightQualityDepth) { strength = 0.2f, minHeight = 0, maxHeight = 1 };
            sq.ExtendResolution(areaChunk, depth);

            // If depth == maxChunkDepth, create new gameobject
            GameObject terr = null;
            if (depth == sett.HightQualityDepth)
            {
                TerrainData tData = new TerrainData();
                float[,] h = ToHeightMap(areaChunk);
                tData.heightmapResolution = h.GetLength(0);
                tData.size = new Vector3((int)sett.chunkSize, sett.height, (int)sett.chunkSize);
                tData.SetHeights(0, 0, h);

                SplatPrototype newSplat = new SplatPrototype();
                newSplat.texture = sett.baseTexture;

                tData.splatPrototypes = new SplatPrototype[] { newSplat };

                terr = Terrain.CreateTerrainGameObject(tData);
                //terr.GetComponent<Terrain>().heightmapPixelError = 1;
                terr.transform.position = new Vector3(pos.x, 0, pos.y);
            }

            chunksInfo[areaChunk].GeneratedObject = terr;
            //chunksInfo[areaChunk].GeneratedDepth = depth;

            return chunksInfo[areaChunk];
        }

        /// <summary>
        /// area must be generated before. Use TryGenerateSingleChunk to generate it.
        /// </summary>
        public IEnumerator GenerateAround(Area area, LandscapeSettings sett)
        {
            _generating = true;

            Chunk chunk;
            if (IsChunkGenerated(area))
                chunk = chunksInfo[area];
            else
            {
                _generating = false;
                throw new ArgumentException("area in GenerateAround must be generated before. Use TryGenerateSingleChunk instead.");
            }

            Vector2 leftDownPt = new Vector2(chunk.GeneratedObject.transform.position.x, chunk.GeneratedObject.transform.position.z);
            Vector2 rigthTopPt = leftDownPt +
                new Vector2((int)sett.chunkSize, (int)sett.chunkSize);

            Area curRight = area;
            Area curLeft = area;
            for (int i = 0; i < sett.HightQualityWidth + sett.MediumQualityWidth + sett.LowQualityWidth; i++)
            {
                int _hightQualityCt = 0;
                if (sett.HightQualityWidth - i > 0)
                    _hightQualityCt = sett.HightQualityWidth - i;

                int _mediumQualityCt = 0;
                if ((sett.HightQualityWidth + sett.MediumQualityWidth - i) % sett.MediumQualityWidth > 0)
                    _mediumQualityCt = sett.MediumQualityWidth - i / sett.HightQualityDepth;

                int _lowQualityCt = 0;
                if ((sett.HightQualityWidth + sett.MediumQualityWidth + sett.LowQualityWidth - i) % sett.LowQualityWidth > 0)
                    _lowQualityCt = sett.LowQualityWidth - i / (sett.MediumQualityWidth + sett.HightQualityWidth);

                Area vertCurUp_Right = curRight;
                Area vertCurDown_Right = curRight;
                Area vertCurUp_Left = curLeft;
                Area vertCurDown_Left = curLeft;
                // Generate vertical chunks
                for (int j = 0; j < _hightQualityCt + _mediumQualityCt + _lowQualityCt; j++)
                {
                    byte curDepth;
                    if (j < _hightQualityCt)
                        curDepth = sett.HightQualityDepth;
                    else
                    {
                        if (j < _hightQualityCt + _mediumQualityCt)
                            curDepth = sett.MediumQualityDepth;
                        else
                            curDepth = sett.LowQualityDepth;
                    }

                    TryGenerateSingleChunk(vertCurUp_Right,
                        leftDownPt + new Vector2(i * (int)sett.chunkSize, j * (int)sett.chunkSize),
                        curDepth,
                        sett);
                    yield return null;
                    TryGenerateSingleChunk(vertCurDown_Right,
                        leftDownPt + new Vector2(i * (int)sett.chunkSize, -j * (int)sett.chunkSize),
                        curDepth,
                        sett);
                    // Left part
                    yield return null;
                    TryGenerateSingleChunk(vertCurUp_Left,
                        leftDownPt + new Vector2(-i * (int)sett.chunkSize, j * (int)sett.chunkSize),
                        curDepth,
                        sett);
                    yield return
                    TryGenerateSingleChunk(vertCurDown_Left,
                            leftDownPt + new Vector2(-i * (int)sett.chunkSize, -j * (int)sett.chunkSize),
                            curDepth,
                            sett);
                    yield return null;

                    if (vertCurUp_Right.TopNeighbor == null)
                        vertCurUp_Right.CreateTopNeighbor();
                    vertCurUp_Right = vertCurUp_Right.TopNeighbor;

                    if (vertCurDown_Right.DownNeighbor == null)
                        vertCurDown_Right.CreateDownNeighbor();
                    vertCurDown_Right = vertCurDown_Right.DownNeighbor;

                    if (vertCurUp_Left.TopNeighbor == null)
                        vertCurUp_Left.CreateTopNeighbor();
                    vertCurUp_Left = vertCurUp_Left.TopNeighbor;

                    if (vertCurDown_Left.DownNeighbor == null)
                        vertCurDown_Left.CreateDownNeighbor();
                    vertCurDown_Left = vertCurDown_Left.DownNeighbor;
                }

                if (curRight.RightNeighbor == null)
                    curRight.CreateRightNeighbor();
                curRight = curRight.RightNeighbor;

                if (curLeft.LeftNeighbor == null)
                    curLeft.CreateLeftNeighbor();
                curLeft = curLeft.LeftNeighbor;
            }
            _generating = false;
        }

        private float[,] ToHeightMap(Area chunkArea)
        {
            MapVertex[,] map = chunkArea.ToArray();
            float[,] heights = new float[map.GetLength(0), map.GetLength(1)];
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    heights[heights.GetLength(0) - 1 - i, j] = map[i, j].Height;
            return heights;
        }
    }
}
