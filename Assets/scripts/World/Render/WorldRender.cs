using System;
using System.Collections.Generic;
using UnityEngine;
using World.Instance;
using World.Model;
using World.Model.Frames;
using World.Model.PointCollections;
using World.Common;

namespace World.Render
{
    /// <summary>
    /// This script renders world
    /// Square frame near player is rendered
    /// </summary>
    [RequireComponent(typeof(WorldInstance))]
    public class WorldRender : MonoBehaviour
    {
        /// <summary>
        /// World to render
        /// </summary>
        public WorldInstance World;

        /// <summary>
        /// Settings for render
        /// </summary>
        public RenderSettings settings = new RenderSettings();

        public Dictionary<ModelCoord, Chunk> renderedChunks;

        /// <summary>
        /// Chunks in this area will be created in scene
        /// </summary>
        public SquareFrame CurRenderFrame { get; private set; }

        ModelCoord prevChangedFrameCoord;

        void Awake()
        {
            int coordRad = (settings.chunkSize - 1) * settings.chunksToRender + 1;
            CurRenderFrame = new SquareFrame(
                new ModelCoord(-coordRad + 1, -coordRad + 1),
                    coordRad * 2 - 1);
        }

        /// <summary>
        /// Listen player moved in model
        /// </summary>
        public void PlayerMovedInModel(ModelCoord lastPos, ModelCoord newPos)
        {
            if (newPos.y - prevChangedFrameCoord.y >= settings.chunkSize)
            {
                MoveFrameTop();
            }
            if (prevChangedFrameCoord.y - newPos.y >= settings.chunkSize)
            {
                MoveFrameDown();
            }
        }

        /// <summary>
        /// Create chunks for curRender frame
        /// </summary>
        public void Initialize()
        {
            renderedChunks = new Dictionary<ModelCoord, Chunk>();
            for (int x = CurRenderFrame.LeftDown.x; x < CurRenderFrame.LeftDown.x + CurRenderFrame.Size - 1; x += settings.chunkSize - 1)
                for (int y = CurRenderFrame.LeftDown.y; y < CurRenderFrame.LeftDown.y + CurRenderFrame.Size - 1; y += settings.chunkSize - 1)
                {
                    Chunk newChunk = new Chunk();
                    newChunk.Initialize(new SquareFrame(new ModelCoord(x, y), settings.chunkSize), 
                        World.Model, settings.worldHeight);
                    newChunk.ApplyTexture(settings.baseTexture);
                    renderedChunks.Add(newChunk.Frame.LeftDown, newChunk);
                }
            prevChangedFrameCoord = new ModelCoord(0, 0);
        }

        /// <summary>
        /// Move curRenderFrame upper
        /// </summary>
        void MoveFrameTop()
        {
            for (int x = CurRenderFrame.LeftDown.x; x < CurRenderFrame.LeftDown.x + CurRenderFrame.Size - 1; x += settings.chunkSize - 1)
            {
                ModelCoord prevChunkCoord = new ModelCoord(x, CurRenderFrame.LeftDown.y);
                ModelCoord newChunkCoord = new ModelCoord(x, CurRenderFrame.LeftDown.y + CurRenderFrame.Size - 1);
                Chunk chunk = renderedChunks[prevChunkCoord];
                renderedChunks.Remove(prevChunkCoord);
                chunk.Initialize(new SquareFrame(newChunkCoord, settings.chunkSize), 
                    World.Model, settings.worldHeight);
                renderedChunks.Add(newChunkCoord, chunk);
            }
            CurRenderFrame = new SquareFrame(
                new ModelCoord(CurRenderFrame.LeftDown.x, CurRenderFrame.LeftDown.y + settings.chunkSize - 1),
                CurRenderFrame.Size);
            prevChangedFrameCoord = World.CurModelCoord;
        }

        /// <summary>
        /// Move curRenderFrame down
        /// </summary>
        void MoveFrameDown()
        {
            for (int x = CurRenderFrame.LeftDown.x; x < CurRenderFrame.LeftDown.x + CurRenderFrame.Size - 1; x += settings.chunkSize - 1)
            {
                ModelCoord prevChunkCoord = new ModelCoord(x, CurRenderFrame.LeftDown.y + CurRenderFrame.Size - settings.chunkSize);
                ModelCoord newChunkCoord = new ModelCoord(x, CurRenderFrame.LeftDown.y - settings.chunkSize + 1);
                Chunk chunk = renderedChunks[prevChunkCoord];
                renderedChunks.Remove(prevChunkCoord);
                chunk.Initialize(new SquareFrame(newChunkCoord, settings.chunkSize),
                    World.Model, settings.worldHeight);
                renderedChunks.Add(newChunkCoord, chunk);
            }
            CurRenderFrame = new SquareFrame(
                new ModelCoord(CurRenderFrame.LeftDown.x, CurRenderFrame.LeftDown.y - settings.chunkSize + 1),
                CurRenderFrame.Size);
            prevChangedFrameCoord = World.CurModelCoord;
        }
    }
}
